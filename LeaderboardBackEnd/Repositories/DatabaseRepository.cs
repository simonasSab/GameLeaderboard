using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Models;

namespace LeaderboardBackEnd.Repositories;

public class DatabaseRepository : IDatabaseRepository
{
    // Player
    public bool InsertPlayer(Player player, out Player newPlayer)
    {
        throw new NotImplementedException();
    }
    public bool UpdatePlayer(Player? player, out Player updatedPlayer)
    {
        throw new NotImplementedException();
    }
    public bool DeletePlayer(int ID)
    {
        throw new NotImplementedException();
    }
    public Player? GetPlayer(int ID)
    {
        throw new NotImplementedException();
    }
    public IEnumerable<Player> GetAllPlayers()
    {
        throw new NotImplementedException();
    }
    public IEnumerable<Player> GetAllPlayers(string phrase) // Search
    {
        throw new NotImplementedException();
    }

    // Score
    public bool InsertScore(Score score, out Score newScore)
    {
        throw new NotImplementedException();
    }
    public bool UpdateScore(Score? score, out Score updatedScore)
    {
        throw new NotImplementedException();
    }
    public bool DeleteScore(int ID)
    {
        throw new NotImplementedException();
    }
    public Score? GetScore(int ID)
    {
        throw new NotImplementedException();
    }
    public IEnumerable<Score> GetAllScores()
    {
        throw new NotImplementedException();
    }
    public IEnumerable<Score> GetAllScores(string phrase) // Search
    {
        throw new NotImplementedException();
    }

    // Level
    public bool InsertLevel(Level level, out Level newLevel)
    {
        throw new NotImplementedException();
    }
    public bool UpdateLevel(Level? level, out Level updatedLevel)
    {
        throw new NotImplementedException();
    }
    public bool DeleteLevel(int ID)
    {
        throw new NotImplementedException();
    }
    public Level? GetLevel(int ID)
    {
        throw new NotImplementedException();
    }
    public IEnumerable<Level> GetAllLevels()
    {
        throw new NotImplementedException();
    }
    public IEnumerable<Level> GetAllLevels(string phrase) // Search
    {
        throw new NotImplementedException();
    }
}
