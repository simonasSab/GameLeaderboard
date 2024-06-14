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

namespace LeaderboardBackEnd.Services
{
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
                Log.Information($"Cache Cleaning: ON ({cachePeriod} s)\n");
                CacheCleaningON = true;
                MongoDBRepo.TruncateDatabaseStart(cachePeriod * 1000);
            }
            else
            {
                Log.Information("Cache Cleaning: OFF\n");
                MongoDBRepo.TruncateDatabaseStop();
                CacheCleaningON = false;
            }
        }

    }
}
