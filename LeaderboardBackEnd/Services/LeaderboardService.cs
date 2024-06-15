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

namespace LeaderboardBackEnd.Services;

public class LeaderboardService : ILeaderboardService
{
    private IDatabaseRepository DatabaseRepo { get; set; }
    private IMongoDBRepository MongoDBRepo { get; set; }
    private bool CacheCleaningON { get; set; } = false;

    public LeaderboardService(IDatabaseRepository databaseRepository, IMongoDBRepository mongoDBRepository)
    {
        DatabaseRepo = databaseRepository;
        MongoDBRepo = mongoDBRepository;
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
            MongoDBRepo.TruncateDatabaseStart(cachePeriod * 1000);
        }
        else
        {
            Log.Information("Cache Cleaning: OFF");
            MongoDBRepo.TruncateDatabaseStop();
            CacheCleaningON = false;
        }
    }

    // Level
    public async Task<bool> InsertLevel(Level level)
    {
        throw new NotImplementedException();
    }
    public async Task<Level>? GetLevel(int ID)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> UpdateLevel(Level level)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> DeleteLevel(Level level)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<Level>>? GetAllLevels()
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<Level>>? GetAllLevels(string phrase) // Search
    {
        throw new NotImplementedException();
    }
    public async Task<bool> LevelIDExists(int ID)
    {
        throw new NotImplementedException();
    }

    // Player
    public async Task<bool> InsertPlayer(Player player)
    {
        throw new NotImplementedException();
    }
    public async Task<Player>? GetPlayer(int ID)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> UpdatePlayer(Player player)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> DeletePlayer(Player player)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<Player>>? GetAllPlayers()
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<Player>>? GetAllPlayers(string phrase) // Search
    {
        throw new NotImplementedException();
    }
    public async Task<bool> PlayerIDExists(int ID)
    {
        throw new NotImplementedException();
    }

    // Score
    public async Task<bool> InsertScore(Score score)
    {
        throw new NotImplementedException();
    }
    public async Task<Score>? GetScore(int ID)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> UpdateScore(Score score)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> DeleteScore(Score score)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<Score>>? GetAllScores()
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<Score>>? GetAllScores(string phrase) // Search
    {
        throw new NotImplementedException();
    }
    public async Task<bool> ScoreIDExists(int ID)
    {
        throw new NotImplementedException();
    }
}
