using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Models;
using MongoDB.Driver;
using Serilog;

namespace LeaderboardBackEnd.Services;

public class LeaderboardService : ILeaderboardService
{
    private IDatabaseRepository _database { get; set; }
    private IMongoDBRepository _cache { get; set; }
    private ICreationService _creationService { get; set; }
    private bool CacheCleaningON { get; set; } = false;

    public LeaderboardService(IDatabaseRepository databaseRepository, IMongoDBRepository mongoDBRepository, ICreationService creationService)
    {
        _database = databaseRepository;
        _cache = mongoDBRepository;
        _creationService = creationService;
    }

    // Cache cleaning
    public bool GetCacheCleaningON()
    {
        return CacheCleaningON;
    }
    public void ToggleCacheCleaning(int cachePeriod)
    {
        if (!CacheCleaningON)
        {
            Log.Information($"Cache Cleaning: ON ({cachePeriod} s)");
            CacheCleaningON = true;
            _cache.TruncateDatabaseStart(cachePeriod * 1000);
        }
        else
        {
            Log.Information("Cache Cleaning: OFF");
            _cache.TruncateDatabaseStop();
            CacheCleaningON = false;
        }
    }

    // Random object insertion
    public async Task InsertRandomLevelAsync()
    {
        Random random = new();
        Level level = new(random.Next(10, 51));

        _database.InsertLevel(level, out Level newLevel);
        Log.Information($"New level: {newLevel.ToString()}");
        await _cache.InsertLevelAsync(newLevel);
    }
    public async Task InsertRandomPlayerAsync()
    {
        _database.InsertPlayer(await _creationService.CreateRandomPlayerAsync(), out Player newPlayer);
        Log.Information($"New player: {newPlayer.ToString()}");
        await _cache.InsertPlayerAsync(newPlayer);
    }
    public async Task InsertRandomScoreAsync()
    {
        if (_database.InsertScore(await _creationService.CreateRandomScoreAsync(), out Score newScore))
        {
            Log.Information($"New score: {newScore.ToString()}");
            await _cache.InsertScoreAsync(newScore);
        }
        else
        {
            Log.Error($"New score not inserted");
        }
    }

    // Level
    public async Task<bool> InsertLevelAsync(Level level)
    {
        if (_database.InsertLevel(level, out Level newLevel))
        {
            Log.Information($"New level: {newLevel.ToString()}");
            await _cache.InsertLevelAsync(newLevel);
            return true;
        }
        Log.Error($"Something went wrong while inserting into database.\n");
        return false;
    }
    public async Task<Level?> GetLevelAsync(int ID)
    {
        Level? level = await _cache.GetLevelAsync(ID);
        if (level == null)
        {
            level = _database.GetLevel(ID);
            if (level != null)
                await _cache.InsertLevelAsync(level);
            return level;
        }
        return level;
    }
    public async Task<bool> UpdateLevelAsync(Level level)
    {
        if (_database.UpdateLevel(level, out Level updatedLevel))
        {
            Log.Information($"Updated Level: {updatedLevel.ToString()}");
            await _cache.UpdateLevelAsync(level);
            return true;
        }
        Log.Error($"Level update in database failed.");
        return false;
    }
    public async Task<bool> DeleteLevelAsync(int ID)
    {
        if (_database.DeleteLevel(ID))
        {
            Log.Information($"Deleted Level with ID:{ID} from database");
            await _cache.DeleteLevelAsync(ID);
            return true;
        }
        Log.Error($"Level deletion from database failed.");
        return false;
    }
    public async Task<IEnumerable<Level>?> GetAllLevelsAsync()
    {
        IEnumerable<Level>? cacheLevels = await _cache.GetAllLevelsAsync();
        if (cacheLevels == null)
        {
            Log.Information($"Cache is empty, synchronizing...\n");
        }
        else
        {
            Log.Information($"Successfully retrieved {cacheLevels.Count()} levels from cache\n");
            return cacheLevels;
        }

        IEnumerable<Level>? levels = _database.GetAllLevels();
        if (levels == null)
        {
            Log.Information("There are no levels in database\n");
            return null;
        }
        await _cache.ImportLevelsAsync(levels);
        return levels;
    }

    // Player
    public async Task<bool> InsertPlayerAsync(Player player)
    {
        if (_database.InsertPlayer(player, out Player newPlayer))
        {
            Log.Information($"New player: {newPlayer.ToString()}");
            await _cache.InsertPlayerAsync(newPlayer);
            return true;
        }
        Log.Error($"Something went wrong while inserting into database.\n");
        return false;
    }
    public async Task<Player?> GetPlayerAsync(int ID)
    {
        Player? player = await _cache.GetPlayerAsync(ID);
        if (player == null)
        {
            player = _database.GetPlayer(ID);
            if (player != null)
                await _cache.InsertPlayerAsync(player);
            return player;
        }
        return player;
    }
    public async Task<bool> UpdatePlayerAsync(Player player)
    {
        if (_database.UpdatePlayer(player, out Player updatedPlayer))
        {
            Log.Information($"Updated Player: {updatedPlayer.ToString()}");
            await _cache.UpdatePlayerAsync(player);
            return true;
        }
        Log.Error($"Player update in database failed.");
        return false;
    }
    public async Task<bool> DeletePlayerAsync(int ID)
    {
        if (_database.DeletePlayer(ID))
        {
            Log.Information($"Deleted Player with ID:{ID} from database");
            await _cache.DeletePlayerAsync(ID);
            return true;
        }
        Log.Error($"Player deletion from database failed.");
        return false;
    }
    public async Task<IEnumerable<Player>?> GetAllPlayersAsync()
    {
        IEnumerable<Player>? cachePlayers = await _cache.GetAllPlayersAsync();
        if (cachePlayers == null)
        {
            Log.Information($"Cache is empty, synchronizing...\n");
        }
        else
        {
            Log.Information($"Successfully retrieved {cachePlayers.Count()} players from cache\n");
            return cachePlayers;
        }

        IEnumerable<Player>? players = _database.GetAllPlayers();
        if (players == null)
        {
            Log.Information("There are no players in database\n");
            return null;
        }
        await _cache.ImportPlayersAsync(players);
        return players;
    }
    public async Task<IEnumerable<Player>?> GetAllPlayersAsync(string phrase) // Search
    {
        IEnumerable<Player>? cachePlayers = await _cache.GetAllPlayersAsync(phrase);
        if (cachePlayers != null)
        {
            Log.Information($"Found {cachePlayers.Count()} players by phrase \"{phrase}\" in cache\n");
            return cachePlayers;
        }
        else
        {
            Log.Information("Nothing found in cache, trying database...\n");
        }

        IEnumerable<Player>? players = _database.GetAllPlayers(phrase);
        if (players != null)
            return players;
        Log.Information($"No players found by phrase \"{phrase}\" in database\n");
        return null;
    }

    // Score
    public async Task<bool> InsertScoreAsync(Score score)
    {
        if (_database.InsertScore(score, out Score newScore))
        {
            Log.Information($"New score: {newScore.ToString()}");
            await _cache.InsertScoreAsync(newScore);
            return true;
        }
        Log.Error($"Something went wrong while inserting into database.\n");
        return false;
    }
    public async Task<Score?> GetScoreAsync(int ID)
    {
        Score? score = await _cache.GetScoreAsync(ID);
        if (score == null)
        {
            score = _database.GetScore(ID);
            if (score != null)
                await _cache.InsertScoreAsync(score);
            return score;
        }
        return score;
    }
    public async Task<bool> UpdateScoreAsync(Score score)
    {
        if (_database.UpdateScore(score, out Score updatedScore))
        {
            Log.Information($"Updated Score: {updatedScore.ToString()}");
            await _cache.UpdateScoreAsync(score);
            return true;
        }
        Log.Error($"Score update in database failed.");
        return false;
    }
    public async Task<bool> DeleteScoreAsync(int ID)
    {
        if (_database.DeleteScore(ID))
        {
            Log.Information($"Deleted Score with ID:{ID} from database");
            await _cache.DeleteScoreAsync(ID);
            return true;
        }
        Log.Error($"Score deletion from database failed.");
        return false;
    }
    public async Task<IEnumerable<Score>?> GetAllScoresAsync()
    {
        IEnumerable<Score>? cacheScores = await _cache.GetAllScoresAsync();
        if (cacheScores == null)
        {
            Log.Information($"Cache is empty, synchronizing...\n");
        }
        else
        {
            Log.Information($"Successfully retrieved {cacheScores.Count()} scores from cache\n");
            return cacheScores;
        }

        IEnumerable<Score>? scores = _database.GetAllScores();
        if (scores == null)
        {
            Log.Information("There are no scores in database\n");
            return null;
        }
        await _cache.ImportScoresAsync(scores);
        return scores;
    }
    public async Task<IEnumerable<Score>?> GetAllScoresAsync(int searchID, bool playerOrLevel) // Search by player OR level ID
    {
        IEnumerable<Score>? cacheScores = await _cache.GetAllScoresAsync(searchID, playerOrLevel);
        if (cacheScores == null)
        {
            Log.Information($"Nothing found in cache, trying database...\n");
        }
        else
        {
            Log.Information($"Found {cacheScores.Count()} scores from cache\n");
            return cacheScores;
        }

        IEnumerable<Score>? scores = _database.GetAllScores(searchID, playerOrLevel);
        if (scores == null)
            Log.Information("No scores found in database\n");
        return scores;
    }
    public async Task<IEnumerable<Score>?> GetAllScoresAsync(int playerID, int levelID) // Search by player AND level ID
    {
        IEnumerable<Score>? cacheScores = await _cache.GetAllScoresAsync(playerID, levelID);
        if (cacheScores == null)
        {
            Log.Information($"Nothing found in cache, trying database...\n");
        }
        else
        {
            Log.Information($"Found {cacheScores.Count()} scores from cache\n");
            return cacheScores;
        }

        IEnumerable<Score>? scores = _database.GetAllScores(playerID, levelID);
        if (scores == null)
            Log.Information("No scores found in database\n");
        return scores;
    }
}
