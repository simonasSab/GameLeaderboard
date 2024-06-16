using LeaderboardBackEnd.Models;

namespace LeaderboardBackEnd.Contracts;

public interface ILeaderboardService
{
    void ToggleCacheCleaning(int cachePeriod);
    bool GetCacheCleaningON();

    // Level
    Task<bool> InsertLevelAsync(Level level);
    Task<Level?> GetLevelAsync(int ID);
    Task<bool> UpdateLevelAsync(Level level);
    Task<bool> DeleteLevelAsync(Level level);
    Task<IEnumerable<Level>?> GetAllLevelsAsync();
    Task<IEnumerable<Level>?> GetAllLevelsAsync(string phrase); // Search
    Task<bool> LevelIDExistsAsync(int ID);

    // Player
    Task<bool> InsertPlayerAsync(Player player);
    Task<Player?> GetPlayerAsync(int ID);
    Task<bool> UpdatePlayerAsync(Player player);
    Task<bool> DeletePlayerAsync(Player player);
    Task<IEnumerable<Player>?> GetAllPlayersAsync();
    Task<IEnumerable<Player>?> GetAllPlayersAsync(string phrase); // Search
    Task<bool> PlayerIDExistsAsync(int ID);

    // Score
    Task<bool> InsertScoreAsync(Score score);
    Task<Score?> GetScoreAsync(int ID);
    Task<bool> UpdateScoreAsync(Score score);
    Task<bool> DeleteScoreAsync(Score score);
    Task<IEnumerable<Score>?> GetAllScoresAsync();
    Task<IEnumerable<Score>?> GetAllScoresAsync(string phrase); // Search
    Task<bool> ScoreIDExistsAsync(int ID);
}
