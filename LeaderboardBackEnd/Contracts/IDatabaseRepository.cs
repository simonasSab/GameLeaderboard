using LeaderboardBackEnd.Models;

namespace LeaderboardBackEnd.Contracts;

public interface IDatabaseRepository
{
    // Level
    bool InsertLevel(Level level, out Level newLevel);
    bool UpdateLevel(Level level, out Level updatedLevel);
    bool DeleteLevel(int ID);
    Level? GetLevel(int ID);
    IEnumerable<Level>? GetAllLevels();
    bool LevelIDExists(int ID);
    int LevelsCount();

    // Player
    bool InsertPlayer(Player player, out Player newPlayer);
    bool UpdatePlayer(Player player, out Player updatedPlayer);
    bool DeletePlayer(int ID);
    Player? GetPlayer(int ID);
    IEnumerable<Player>? GetAllPlayers();
    IEnumerable<Player>? GetAllPlayers(string phrase); // Search
    bool PlayerIDExists(int ID);
    Task<bool> UsernameIsTaken(string username);
    int PlayersCount();


    // Score
    bool InsertScore(Score score, out Score newScore);
    bool UpdateScore(Score score, out Score updatedScore);
    bool DeleteScore(int ID);
    Score? GetScore(int ID);
    IEnumerable<Score>? GetAllScores();
    IEnumerable<Score>? GetAllScores(int searchID, bool playerOrLevel); // Search by player OR level ID
    IEnumerable<Score>? GetAllScores(int playerID, int levelID); // Search by player AND level ID
    bool ScoreIDExists(int ID);
    int ScoresCount();
}