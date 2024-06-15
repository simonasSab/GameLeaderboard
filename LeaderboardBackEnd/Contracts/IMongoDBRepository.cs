using LeaderboardBackEnd.Enums;
using LeaderboardBackEnd.Models;

namespace LeaderboardBackEnd.Contracts;

public interface IMongoDBRepository
{
    // Cleaning
    Task TruncateDatabaseStop();
    Task TruncateDatabaseStart(int cachePeriod);
    Task TruncateCollection(MongoDBCollectionName collectionNameEnum);

    // Import to cache
    Task ImportPlayersAsync(IEnumerable<Player> players);
    Task ImportLevelsAsync(IEnumerable<Level> levels);
    Task ImportScoresAsync(IEnumerable<Score> scores);

    // Level
    Task InsertLevelAsync(Level level);
    Task UpdateLevelAsync(Level level);
    Task DeleteLevelAsync(int ID);
    Task<Level?> GetLevelAsync(int ID);
    Task<IEnumerable<Level>?> GetAllLevelsAsync();
    Task<bool> LevelIDExists(int ID);

    // Player
    Task InsertPlayerAsync(Player player);
    Task UpdatePlayerAsync(Player player);
    Task DeletePlayerAsync(int ID);
    Task<Player?> GetPlayerAsync(int ID);
    Task<IEnumerable<Player>?> GetAllPlayersAsync();
    Task<IEnumerable<Player>?> GetAllPlayersAsync(string phrase); // Search
    Task<bool> PlayerIDExists(int ID);

    // Score
    Task InsertScoreAsync(Score score);
    Task UpdateScoreAsync(Score score);
    Task DeleteScoreAsync(int ID);
    Task<Score?> GetScoreAsync(int ID);
    Task<IEnumerable<Score>?> GetAllScoresAsync();
    Task<IEnumerable<Score>?> GetAllScoresAsync(int searchID, bool playerOrLevel); // Search by player OR level ID
    Task<IEnumerable<Score>?> GetAllScoresAsync(int playerID, int levelID); // Search by player AND level ID
    Task<bool> ScoreIDExists(int ID);
}
