using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Enums;
using LeaderboardBackEnd.Models;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using Serilog;
using MongoDB.Bson;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardBackEnd.Repositories;

public class MongoDBRepository : IMongoDBRepository
{
    private IMongoDatabase MongoDatabase { get; set; }
    private CancellationTokenSource CacheCleanCTSource { get; set; }
    private CancellationToken CToken { get; set; }

    private readonly IMongoCollection<Level> _levels;
    private readonly IMongoCollection<Player> _players;
    private readonly IMongoCollection<Score> _scores;

    public MongoDBRepository(IMongoClient mongoClient)
    {
        MongoDatabase = mongoClient.GetDatabase("GameLeaderboard");

        _levels = MongoDatabase.GetCollection<Level>("Levels");
        _players = MongoDatabase.GetCollection<Player>("Players");
        _scores = MongoDatabase.GetCollection<Score>("Scores");

        CacheCleanCTSource = new();
        CToken = CacheCleanCTSource.Token;
    }
    // Cleaning
    public async Task TruncateDatabaseStop()
    {
        // Cancel current Cache Clean cycle and dispose of the Token Source
        await CacheCleanCTSource.CancelAsync();
        CacheCleanCTSource.Dispose();
        // Create new token Source and new Token
        CacheCleanCTSource = new();
        CToken = CacheCleanCTSource.Token;
    }
    public async Task TruncateDatabaseStart(int cachePeriod)
    {
        await Task.Delay(cachePeriod, CToken);

        if (!CToken.IsCancellationRequested)
        {
            await _levels.DeleteManyAsync(new BsonDocument());
            await _players.DeleteManyAsync(new BsonDocument());
            await _scores.DeleteManyAsync(new BsonDocument());
            Log.Information($"{DateTime.Now} --- MongoDB cache cleared!");
            await TruncateDatabaseStart(cachePeriod);
        }
    }
    public async Task TruncateCollection(MongoDBCollectionName collectionNameEnum)
    {
        if (collectionNameEnum.ToString() == "Levels")
            await _levels.DeleteManyAsync(new BsonDocument());
        else if (collectionNameEnum.ToString() == "Players")
            await _players.DeleteManyAsync(new BsonDocument());
        else if (collectionNameEnum.ToString() == "Scores")
            await _scores.DeleteManyAsync(new BsonDocument());

        Log.Information($"Deleted all items in MongoDB collection \"{collectionNameEnum.ToString()}\"");
    }

    // Import to cache
    public async Task ImportPlayersAsync(IEnumerable<Player> players)
    {
        try
        {
            await _players.InsertManyAsync(players);
            Log.Information($"Inserted {players.Count()} items into cache");
        }
        catch (MongoWriteException e)
        {
            Log.Error($"Insertion into cache failed ({e.Message}).");
        }
    }
    public async Task ImportLevelsAsync(IEnumerable<Level> levels)
    {
        try
        {
            await _levels.InsertManyAsync(levels);
            Log.Information($"Inserted {levels.Count()} items into cache");
        }
        catch (MongoWriteException e)
        {
            Log.Error($"Insertion into cache failed ({e.Message}).");
        }
    }
    public async Task ImportScoresAsync(IEnumerable<Score> scores)
    {
        try
        {
            await _scores.InsertManyAsync(scores);
            Log.Information($"Inserted {scores.Count()} items into cache");
        }
        catch (MongoWriteException e)
        {
            Log.Error($"Insertion into cache failed ({e.Message}).");
        }
    }

    // Level
    public async Task InsertLevelAsync(Level level)
    {
        try
        {
            await _levels.InsertOneAsync(level);
            Log.Information($"Inserted into cache");
        }
        catch (MongoWriteException e)
        {
            Log.Error($"Insertion into cache failed ({e.Message}).");
        }
    }
    public async Task UpdateLevelAsync(Level level)
    {
        try
        {
            await _levels.UpdateOneAsync(Builders<Level>.Filter.Eq(x => x.MongoID, level.MongoID), Builders<Level>.Update.Set(x => x, level));
            Log.Information($"Updated in cache");
        }
        catch (MongoWriteException e)
        {
            Log.Error($"Update at cache failed ({e.Message}).");
        }
    }
    public async Task DeleteLevelAsync(int ID)
    {
        if (await LevelIDExists(ID))
        {
            try
            {
                await _levels.DeleteOneAsync(Builders<Level>.Filter.Eq(x => x.ID, ID));
                Log.Information($"Deleted from cache");
            }
            catch (Exception e)
            {
                Log.Error($"Deletion from cache failed ({e.Message}).");
            }
        }
    }
    public async Task<Level?> GetLevelAsync(int ID)
    {
        if (await LevelIDExists(ID))
        {
            try
            {
                return await _levels.Find(x => x.ID == ID).FirstAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Single retrieval from cache failed ({e.Message}).");
                return null;
            }
        }
        return null;
    }
    public async Task<IEnumerable<Level>?> GetAllLevelsAsync()
    {
        try
        {
            return await _levels.Find(_ => true).ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error($"Retrieval from cache failed ({e.Message}).");
            return null;
        }
    }
    public async Task<bool> LevelIDExists(int ID)
    {
        if (await _levels.Find(x => x.ID == ID).Limit(1).FirstOrDefaultAsync() != null)
            return true;
        Log.Information($"ID: {ID} does not exist in cache.");
        return false;
    }


    // Player
    public async Task InsertPlayerAsync(Player player)
    {
        try
        {
            await _players.InsertOneAsync(player);
            Log.Information($"Inserted into cache");
        }
        catch (MongoWriteException e)
        {
            Log.Error($"Insertion into cache failed ({e.Message}).");
        }
    }
    public async Task UpdatePlayerAsync(Player player)
    {
        try
        {
            await _players.UpdateOneAsync(Builders<Player>.Filter.Eq(x => x.MongoID, player.MongoID), Builders<Player>.Update.Set(x => x, player));
            Log.Information($"Updated in cache");
        }
        catch (MongoWriteException e)
        {
            Log.Error($"Update at cache failed ({e.Message}).");
        }
    }
    public async Task DeletePlayerAsync(int ID)
    {
        if (await PlayerIDExists(ID))
        {
            try
            {
                await _players.DeleteOneAsync(Builders<Player>.Filter.Eq(x => x.ID, ID));
                Log.Information($"Deleted from cache");
            }
            catch (Exception e)
            {
                Log.Error($"Deletion from cache failed ({e.Message}).");
            }
        }
    }
    public async Task<Player?> GetPlayerAsync(int ID)
    {
        if (await PlayerIDExists(ID))
        {
            try
            {
                return await _players.Find(x => x.ID == ID).FirstAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Single retrieval from cache failed ({e.Message}).");
                return null;
            }
        }
        return null;
    }
    public async Task<IEnumerable<Player>?> GetAllPlayersAsync()
    {
        try
        {
            return await _players.Find(_ => true).ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error($"Retrieval from cache failed: ({e.Message}).");
            return null;
        }
    }
    public async Task<IEnumerable<Player>?> GetAllPlayersAsync(string phrase) // Search
    {
        string cleanPhrase = Regex.Replace(phrase, @"\s+", "");
        FilterDefinition<Player> filter = Builders<Player>.Filter.Regex("Username", new BsonRegularExpression(cleanPhrase, "i"));

        Log.Information($"Trying to find \"{phrase}\" in Players...");

        try
        {
            return await _players.Find(filter).SortBy(x => x.Username).ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error($"Retrieval from cache failed ({e.Message}).");
            return null;
        }
    }
    public async Task<bool> PlayerIDExists(int ID)
    {
        if (await _players.Find(x => x.ID == ID).Limit(1).FirstOrDefaultAsync() != null)
            return true;
        Log.Information($"ID: {ID} does not exist in cache.");
        return false;
    }


    // Score
    public async Task InsertScoreAsync(Score score)
    {
        try
        {
            await _scores.InsertOneAsync(score);
            Log.Information($"Inserted into cache");
        }
        catch (MongoWriteException e)
        {
            Log.Error($"Insertion into cache failed ({e.Message}).");
        }
    }
    public async Task UpdateScoreAsync(Score score)
    {
        try
        {
            await _scores.UpdateOneAsync(Builders<Score>.Filter.Eq(x => x.MongoID, score.MongoID), Builders<Score>.Update.Set(x => x, score));
            Log.Information($"Updated in cache");
        }
        catch (MongoWriteException e)
        {
            Log.Error($"Update at cache failed ({e.Message}).");
        }
    }
    public async Task DeleteScoreAsync(int ID)
    {
        if (await ScoreIDExists(ID))
        {
            try
            {
                await _scores.DeleteOneAsync(Builders<Score>.Filter.Eq(x => x.ID, ID));
                Log.Information($"Deleted from cache");
            }
            catch (Exception e)
            {
                Log.Error($"Deletion from cache failed ({e.Message}).");
            }
        }
    }
    public async Task<Score?> GetScoreAsync(int ID)
    {
        if (await ScoreIDExists(ID))
        {
            try
            {
                return await _scores.Find(x => x.ID == ID).FirstAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Single retrieval from cache failed ({e.Message}).");
                return null;
            }
        }
        return null;
    }
    public async Task<IEnumerable<Score>?> GetAllScoresAsync()
    {
        try
        {
            return await _scores.Find(_ => true).ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error($"Retrieval from cache failed ({e.Message}).");
            return null;
        }
    }
    public async Task<IEnumerable<Score>?> GetAllScoresAsync(int searchID, bool playerOrLevel) // Search by player OR level ID
    {
        try
        {
            if (playerOrLevel && await PlayerIDExists(searchID))
            {
                Log.Information($"Trying to find Scores by Player ID: {searchID}...");
                return await _scores.Find(x => x.PlayerID == searchID).SortBy(x => x.LevelID).ThenBy(x => x.Points).ThenBy(x => x.Time).ToListAsync();
            }
            else if (!playerOrLevel && await LevelIDExists(searchID))
            {
                Log.Information($"Trying to find Scores by Level ID: {searchID}...");
                return await _scores.Find(x => x.LevelID == searchID).SortBy(x => x.PlayerID).ThenBy(x => x.Points).ThenBy(x => x.Time).ToListAsync();
            }
            return null;
        }
        catch (Exception e)
        {
            Log.Error($"Search in cache failed ({e.Message}).");
            return null;
        }
    }
    public async Task<IEnumerable<Score>?> GetAllScoresAsync(int playerID, int levelID) // Search by player AND level ID
    {
        if (await PlayerIDExists(playerID) && await LevelIDExists(levelID))
        {
            try
            {
                Log.Information($"Trying to find Scores by Player ID: {playerID} and Level ID: {levelID}...");
                return await _scores.Find(x => x.PlayerID == playerID && x.LevelID == levelID).SortBy(x => x.Points).ThenBy(x => x.Time).ToListAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Search in cache failed ({e.Message}).");
                return null;
            }
        }
        return null;
    }
    public async Task<bool> ScoreIDExists(int ID)
    {
        if (await _scores.Find(x => x.ID == ID).Limit(1).FirstOrDefaultAsync() != null)
            return true;
        Log.Information($"ID: {ID} does not exist in cache.");
        return false;
    }
}
