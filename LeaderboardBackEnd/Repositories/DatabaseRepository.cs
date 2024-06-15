using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Databases;
using LeaderboardBackEnd.Models;
using System.Data;
using System.Text.RegularExpressions;
using Serilog;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardBackEnd.Repositories;

public class DatabaseRepository : IDatabaseRepository
{
    private readonly string _connectionString;
    private readonly LeaderboardDBContext _dbContext;
    public DatabaseRepository(string connectionString)
    {
        _connectionString = connectionString;
        _dbContext = new();
    }

    // Level
    public bool InsertLevel(Level item, out Level newItem)
    {
        newItem = item;
        if (item == null)
            return false;

        _dbContext.Add(item);
        _dbContext.SaveChanges();
        if (item.ID > 0)
        {
            newItem = item;
            return true;
        }
        _dbContext.Remove(item);
        _dbContext.SaveChanges();
        return false;
    }
    public bool UpdateLevel(Level item, out Level updatedItem)
    {
        // Keep updated object as updatedItem
        updatedItem = item;
        if (item == null)
            return false;
        // Find current object from DB and keep as item
        item = _dbContext.Levels.Find(item.ID);
        // Update value in DB
        _dbContext.Update(updatedItem);
        _dbContext.SaveChanges();
        // Check if updatedItem (returned from DB) was updated
        if (!updatedItem.Equals(item))
            return true;
        return false;
    }
    public bool DeleteLevel(int ID)
    {
        Level? item = GetLevel(ID);
        if (item == null)
        {
            Log.Error("ERROR: ID was not deleted from database");
            return false;
        }
        _dbContext.Levels.Remove(item);
        _dbContext.SaveChanges();

        if (!_dbContext.Scores.Any(x => x.ID == ID))
            return true;
        else
            Log.Error("ERROR: ID was not deleted from database");
        return false;
    }
    public Level? GetLevel(int ID)
    {
        return _dbContext.Levels.Find(ID);
    }
    public IEnumerable<Level> GetAllLevels()
    {
        return _dbContext.Levels.ToList();
    }
    public bool LevelIDExists(int ID)
    {
        return _dbContext.Levels.Any(x => x.ID == ID);
    }

    // Player
    public bool InsertPlayer(Player item, out Player newItem)
    {
        newItem = item;
        if (item == null)
            return false;

        _dbContext.Add(item);
        _dbContext.SaveChanges();
        if (item.ID > 0)
        {
            newItem = item;
            return true;
        }
        _dbContext.Remove(item);
        _dbContext.SaveChanges();
        return false;
    }
    public bool UpdatePlayer(Player item, out Player updatedItem)
    {
        // Keep updated object as updatedItem
        updatedItem = item;
        if (item == null)
            return false;
        // Find current object from DB and keep as item
        item = _dbContext.Players.Find(item.ID);
        // Update value in DB
        _dbContext.Update(updatedItem);
        _dbContext.SaveChanges();
        // Check if updatedItem (returned from DB) was updated
        if (!updatedItem.Equals(item))
            return true;
        return false;
    }
    public bool DeletePlayer(int ID)
    {
        Player? item = GetPlayer(ID);
        if (item == null)
        {
            Log.Error("ERROR: ID was not deleted from database");
            return false;
        }
        _dbContext.Players.Remove(item);
        _dbContext.SaveChanges();

        if (!_dbContext.Players.Any(x => x.ID == ID))
            return true;
        else
            Log.Error("ERROR: ID was not deleted from database");
        return false;
    }
    public Player? GetPlayer(int ID)
    {
        return _dbContext.Players.Find(ID);
    }
    public IEnumerable<Player> GetAllPlayers()
    {
        return _dbContext.Players.ToList();
    }
    public IEnumerable<Player> GetAllPlayers(string phrase) // Search
    {
        Regex rx = new("*phrase*", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        return _dbContext.Players.Where(x => rx.IsMatch(Regex.Replace(x.Username, @"\s+", ""))).ToList();
    }
    public bool PlayerIDExists(int ID)
    {
        return _dbContext.Players.Any(x => x.ID == ID);
    }

    // Score
    public bool InsertScore(Score item, out Score newItem)
    {
        newItem = item;
        if (item == null)
            return false;

        _dbContext.Add(item);
        _dbContext.SaveChanges();
        if (item.ID > 0)
        {
            newItem = item;
            return true;
        }
        _dbContext.Remove(item);
        _dbContext.SaveChanges();
        return false;
    }
    public bool UpdateScore(Score item, out Score updatedItem)
    {
        // Keep updated object as updatedItem
        updatedItem = item;
        if (item == null)
            return false;
        // Find current object from DB and keep as item
        item = _dbContext.Scores.Find(item.ID);
        // Update value in DB
        _dbContext.Update(updatedItem);
        _dbContext.SaveChanges();
        // Check if updatedItem (returned from DB) was updated
        if (!updatedItem.Equals(item))
            return true;
        return false;
    }
    public bool DeleteScore(int ID)
    {
        Score? item = GetScore(ID);
        if (item == null)
        {
            Log.Error("ERROR: ID was not deleted from database");
            return false;
        }
        _dbContext.Scores.Remove(item);
        _dbContext.SaveChanges();

        if (!_dbContext.Scores.Any(x => x.ID == ID))
            return true;
        else
            Log.Error("ERROR: ID was not deleted from database");
        return false;
    }
    public Score? GetScore(int ID)
    {
        return _dbContext.Scores.Find(ID);
    }
    public IEnumerable<Score> GetAllScores()
    {
        return _dbContext.Scores.ToList();
    }
    public IEnumerable<Score> GetAllScores(int searchID, bool playerOrLevel) // Search by player OR level ID
    {
        if (playerOrLevel)
            return _dbContext.Scores.Where(x => x.PlayerID == searchID).ToList();
        else
            return _dbContext.Scores.Where(x => x.LevelID == searchID).ToList();
    }
    public IEnumerable<Score> GetAllScores(int playerID, int levelID) // Search by player AND level ID
    {
        return _dbContext.Scores.Where(x => x.PlayerID == playerID || x.LevelID == levelID).ToList();
    }
    public bool ScoreIDExists(int ID)
    {
        return _dbContext.Scores.Any(x => x.ID == ID);
    }
}
