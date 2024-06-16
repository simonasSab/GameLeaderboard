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
    Task<bool> DeleteLevelAsync(int ID);
    Task<IEnumerable<Level>?> GetAllLevelsAsync();

    // Player
    Task<bool> InsertPlayerAsync(Player player);
    Task<Player?> GetPlayerAsync(int ID);
    Task<bool> UpdatePlayerAsync(Player player);
    Task<bool> DeletePlayerAsync(int ID);
    Task<IEnumerable<Player>?> GetAllPlayersAsync();
    Task<IEnumerable<Player>?> GetAllPlayersAsync(string phrase); // Search

    // Score
    Task<bool> InsertScoreAsync(Score score);
    Task<Score?> GetScoreAsync(int ID);
    Task<bool> UpdateScoreAsync(Score score);
    Task<bool> DeleteScoreAsync(int ID );
    Task<IEnumerable<Score>?> GetAllScoresAsync();
    Task<IEnumerable<Score>?> GetAllScoresAsync(int searchID, bool playerOrLevel); // Search by player OR level ID
    Task<IEnumerable<Score>?> GetAllScoresAsync(int playerID, int levelID); // Search by player AND level ID
}
