using MongoDB.Driver;
using MongoDB.Bson;
using Serilog;
using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Repositories;
using LeaderboardBackEnd.Databases;
using LeaderboardBackEnd.Models;
using LeaderboardBackEnd.Services;
using Microsoft.Extensions.Hosting;

namespace LeaderboardBackEnd;

public class Program
{
    const string connectionUri = "mongodb+srv://simonasSab:HTAc7vsY9TJyfcwO+@gameleaderboard.zntvale.mongodb.net/?retryWrites=true&w=majority&appName=GameLeaderboard";
    static IDatabaseRepository _databaseRepository { get; set; }
    static IMongoDBRepository _mongoDBRepository { get; set; }
    static ILeaderboardService _leaderboardService { get; set; }
    static ICreationService _creationService { get; set; }

    public static void Main(string[] args)
    {
        // Create Serilog configuration
        ILogger log = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log_backend.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        Log.Logger = log;

        // Setup MongoDB
        // Create new client and connect to server
        var client = new MongoClient(connectionUri);
        _mongoDBRepository = new MongoDBRepository(client);
        // Ping to confirm successful connection
        try
        {
            var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Log.Information("Pinged your deployment. You successfully connected to MongoDB!");
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        // Initialize server DB and back end service
        _databaseRepository = new DatabaseRepository("Server=DESKTOP-OD4Q280;Database=GameLeaderboard;Integrated Security=True;TrustServerCertificate=true;");
        _creationService = new CreationService(_databaseRepository);
        _leaderboardService = new LeaderboardService(_databaseRepository, _mongoDBRepository, _creationService);

        RunTasks();
    }

    public static void RunTasks()
    {
        Log.Information("Hello! Console application is not implemented, please use API and Website.");
    }
}