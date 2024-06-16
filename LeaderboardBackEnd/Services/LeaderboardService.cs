using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Models;
using LeaderboardBackEnd.Databases;
using LeaderboardBackEnd.Repositories;
using LeaderboardBackEnd.Enums;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;
using System.Linq;
using Serilog;
using System.Numerics;
using static System.Formats.Asn1.AsnWriter;

namespace LeaderboardBackEnd.Services;

public class LeaderboardService : ILeaderboardService
{
    private IDatabaseRepository _database { get; set; }
    private IMongoDBRepository _cache { get; set; }
    private bool CacheCleaningON { get; set; } = false;

    public LeaderboardService(IDatabaseRepository databaseRepository, IMongoDBRepository mongoDBRepository)
    {
        _database = databaseRepository;
        _cache = mongoDBRepository;
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
        if (level != null)
        {
            return level;
        }
        else
        {
            level = _database.GetLevel(ID);
            if (level != null)
            {
                await _cache.InsertLevelAsync(level);
                return level;
            }
            return null;
        }
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
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<Level>?> GetAllLevelsAsync(string phrase) // Search
    {
        throw new NotImplementedException();
    }
    public async Task<bool> LevelIDExistsAsync(int ID)
    {
        throw new NotImplementedException();
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
        if (player != null)
        {
            return player;
        }
        else
        {
            player = _database.GetPlayer(ID);
            if (player != null)
            {
                await _cache.InsertPlayerAsync(player);
                return player;
            }
            return null;
        }
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
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<Player>?> GetAllPlayersAsync(string phrase) // Search
    {
        throw new NotImplementedException();
    }
    public async Task<bool> PlayerIDExistsAsync(int ID)
    {
        throw new NotImplementedException();
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
        if (score != null)
        {
            return score;
        }
        else
        {
            score = _database.GetScore(ID);
            if (score != null)
            {
                await _cache.InsertScoreAsync(score);
                return score;
            }
            return null;
        }
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
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<Score>?> GetAllScoresAsync(string phrase) // Search
    {
        throw new NotImplementedException();
    }
    public async Task<bool> ScoreIDExistsAsync(int ID)
    {
        throw new NotImplementedException();
    }
}
