using LeaderboardBackEnd.Models;

namespace LeaderboardBackEnd.Contracts;

public interface ILeaderboardService
{
    void ToggleCacheCleaning(int cachePeriod);
    bool GetCacheCleaningON();

    // Level
    Task<bool> InsertLevel(Level level);
    Task<Level?> GetLevel(int ID);
    Task<bool> UpdateLevel(Level level);
    Task<bool> DeleteLevel(Level level);
    Task<IEnumerable<Level>?> GetAllLevels();
    Task<IEnumerable<Level>?> GetAllLevels(string phrase); // Search
    Task<bool> LevelIDExists(int ID);

    // Player
    Task<bool> InsertPlayer(Player player);
    Task<Player?> GetPlayer(int ID);
    Task<bool> UpdatePlayer(Player player);
    Task<bool> DeletePlayer(Player player);
    Task<IEnumerable<Player>?> GetAllPlayers();
    Task<IEnumerable<Player>?> GetAllPlayers(string phrase); // Search
    Task<bool> PlayerIDExists(int ID);

    // Score
    Task<bool> InsertScore(Score score);
    Task<Score?> GetScore(int ID);
    Task<bool> UpdateScore(Score score);
    Task<bool> DeleteScore(Score score);
    Task<IEnumerable<Score>?> GetAllScores();
    Task<IEnumerable<Score>?> GetAllScores(string phrase); // Search
    Task<bool> ScoreIDExists(int ID);
}
