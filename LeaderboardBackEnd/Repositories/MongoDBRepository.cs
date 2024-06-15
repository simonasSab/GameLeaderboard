using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Enums;
using LeaderboardBackEnd.Models;
using MongoDB.Driver;
using System.Data;
using System.Text.RegularExpressions;
using Serilog;
using MongoDB.Bson;
using System.Numerics;


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
            Log.Information($"{DateTime.Now} --- MongoDB cache cleared!\n");
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

        Log.Information($"Deleted all items in MongoDB collection \"{collectionNameEnum.ToString()}\"\n");
    }

    // Import to cache
    public async Task ImportPlayersAsync(IEnumerable<Player> players)
    {
        try
        {
            await _players.InsertManyAsync(players);
        }
        catch (MongoWriteException)
        {
            Log.Error("Insertion into MongoDB failed.\n");
        }
    }
    public async Task ImportLevelsAsync(IEnumerable<Level> levels)
    {
        try
        {
            await _levels.InsertManyAsync(levels);
        }
        catch (MongoWriteException)
        {
            Log.Error("Insertion into MongoDB failed.\n");
        }
    }
    public async Task ImportScoresAsync(IEnumerable<Score> scores)
    {
        try
        {
            await _scores.InsertManyAsync(scores);
        }
        catch (MongoWriteException)
        {
            Log.Error("Insertion into MongoDB failed.\n");
        }
    }

    // Players
    public async Task InsertPlayerAsync(Player player)
    {
        try
        {
            await _players.InsertOneAsync(player);
        }
        catch (MongoWriteException)
        {
            Log.Error("Insertion into MongoDB failed.\n");
        }
    }
    public async Task UpdatePlayerAsync(Player player)
    {
        try
        {
            await _players.UpdateOneAsync(Builders<Player>.Filter.Eq(x => x.MongoID, player.MongoID), Builders<Player>.Update.Set(x => x, player));
        }
        catch (MongoWriteException)
        {
            Log.Error("Update at MongoDB failed.\n");
        }
    }
    public async Task DeletePlayerAsync(int ID)
    {
        try
        {
            await _players.DeleteOneAsync(Builders<Player>.Filter.Eq(x => x.ID, ID));
        }
        catch (MongoWriteException)
        {
            Log.Error("Deletion from MongoDB failed.\n");
        }
    }
    public async Task<Player?> GetPlayerAsync(int ID)
    {
        try
        {
            return await _players.Find(x => x.ID == ID).FirstAsync();
        }
        catch (MongoWriteException)
        {
            Log.Error("Single retrieval from MongoDB failed.\n");
            return null;
        }
    }
    public async Task<IEnumerable<Player>>? GetAllPlayersAsync()
    {
        try
        {
            return await _players.Find(_ => true).ToListAsync();
        }
        catch (MongoWriteException)
        {
            Log.Error("Retrieval from MongoDB failed.\n");
            return null;
        }
    }
    public async Task<IEnumerable<Player>> GetAllPlayersAsync(string phrase) // Search
    {
        FilterDefinition<Player> filter = Builders<Player>.Filter.Regex(Regex.Replace("Username", @"\s+", ""), new BsonRegularExpression(Regex.Replace(phrase, @"\s+", ""), "i"));

        Log.Information($"Trying to find \"{phrase}\" in Players...\n");

        return await _players.Find(filter).SortBy(x => x.Username).ToListAsync();
    }


    // Levels
    public async Task InsertLevelAsync(Level level)
    {
        try
        {
            await _levels.InsertOneAsync(level);
        }
        catch (MongoWriteException)
        {
            Log.Error("Insertion into MongoDB failed.\n");
        }
    }
    public async Task UpdateLevelAsync(Level level)
    {
        try
        {
            await _levels.UpdateOneAsync(Builders<Level>.Filter.Eq(x => x.MongoID, level.MongoID), Builders<Level>.Update.Set(x => x, level));
        }
        catch (MongoWriteException)
        {
            Log.Error("Update at MongoDB failed.\n");
        }
    }
    public async Task DeleteLevelAsync(int ID)
    {
        try
        {
            await _levels.DeleteOneAsync(Builders<Level>.Filter.Eq(x => x.ID, ID));
        }
        catch (MongoWriteException)
        {
            Log.Error("Deletion from MongoDB failed.\n");
        }
    }
    public async Task<Level?> GetLevelAsync(int ID)
    {
        try
        {
            return await _levels.Find(x => x.ID == ID).FirstAsync();
        }
        catch (MongoWriteException)
        {
            Log.Error("Single retrieval from MongoDB failed.\n");
            return null;
        }
    }
    public async Task<IEnumerable<Level>> GetAllLevelsAsync()
    {
        try
        {
            return await _levels.Find(_ => true).ToListAsync();
        }
        catch (MongoWriteException)
        {
            Log.Error("Retrieval from MongoDB failed.\n");
            return null;
        }
    }


    // Scores
    public async Task InsertScoreAsync(Score score)
    {
        try
        {
            await _scores.InsertOneAsync(score);
        }
        catch (MongoWriteException)
        {
            Log.Error("Insertion into MongoDB failed.\n");
        }
    }
    public async Task UpdateScoreAsync(Score score)
    {
        try
        {
            await _scores.UpdateOneAsync(Builders<Score>.Filter.Eq(x => x.MongoID, score.MongoID), Builders<Score>.Update.Set(x => x, score));
        }
        catch (MongoWriteException)
        {
            Log.Error("Update at MongoDB failed.\n");
        }
    }
    public async Task DeleteScoreAsync(int ID)
    {
        try
        {
            await _scores.DeleteOneAsync(Builders<Score>.Filter.Eq(x => x.ID, ID));
        }
        catch (MongoWriteException)
        {
            Log.Error("Deletion from MongoDB failed.\n");
        }
    }
    public async Task<Score?> GetScoreAsync(int ID)
    {
        try
        {
            return await _scores.Find(x => x.ID == ID).FirstAsync();
        }
        catch (MongoWriteException)
        {
            Log.Error("Single retrieval from MongoDB failed.\n");
            return null;
        }
    }
    public async Task<IEnumerable<Score>> GetAllScoresAsync()
    {
        try
        {
            return await _scores.Find(_ => true).ToListAsync();
        }
        catch (MongoWriteException)
        {
            Log.Error("Retrieval from MongoDB failed.\n");
            return null;
        }
    }
}
