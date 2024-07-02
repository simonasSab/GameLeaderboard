using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Models;
using LeaderboardBackEnd.Enums;

namespace LeaderboardBackEnd.Services;

public class CreationService : ICreationService
{
    private IDatabaseRepository _database { get; set; }

    public CreationService(IDatabaseRepository databaseRepository)
    {
        _database = databaseRepository;
    }

    public async Task<Player> CreateRandomPlayerAsync()
    {
        Random random = new();
        // Generate dope username
        string username = "";
        do
        {
            username = $"{Enum.GetName(typeof(RandomUsername), random.Next(91))}_{random.Next(1000):000}";
        }
        while (await _database.UsernameIsTaken(username));

        // Generate unmemorable password
        string password = $"{(char)random.Next(33, 127)}{(char)random.Next(33, 127)}{(char)random.Next(33, 127)}{(char)random.Next(33, 127)}{random.Next(100):00}" +
            $"{(char)random.Next(33, 127)}{(char)random.Next(33, 127)}{(char)random.Next(33, 127)}{(char)random.Next(33, 127)}{(char)random.Next(33, 127)}";

        // Generate sloppy email
        string email = $"{username}@xyz{random.Next(10)}.com";
        Player player = new(username, password, email);

        return new(username, password, email);
    }

    public Score CreateRandomScoreAsync()
    {
        Random random = new();

        int playerID;
        do
        {
            playerID = random.Next(1, _database.PlayersCount() + 1);
        }
        while (!_database.PlayerIDExists(playerID));

        int levelID;
        do
        {
            levelID = random.Next(1, _database.LevelsCount() + 1);
        }
        while (!_database.LevelIDExists(levelID));

        int levelMaxScore = _database.GetLevel(levelID).MaxScore;
        int points = random.Next(3, levelMaxScore + 1);
        TimeSpan time = TimeSpan.FromSeconds(points * 5 + (int)(10 * random.NextDouble()));

        return new(playerID, levelID, points, time);
    }
}
