﻿using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Databases;
using LeaderboardBackEnd.Models;
using System.Data;
using Serilog;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Linq;

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
    public bool InsertLevel(Level item)
    {
        // Insert value into DB
        _dbContext.Update(item);
        // Check if item was updated
        if (_dbContext.Entry(item).State == EntityState.Unchanged)
            return false;
        _dbContext.SaveChanges();
        return true;
    }
    public bool UpdateLevel(Level item)
    {
        // Update value in DB
        _dbContext.Update(item);
        // Check if item was updated
        if (_dbContext.Entry(item).State == EntityState.Unchanged)
            return false;
        _dbContext.SaveChanges();
        return true;
    }
    public bool DeleteLevel(int ID)
    {
        Level? item = GetLevel(ID);
        if (item == null)
        {
            Log.Error($"ID: {ID} was not deleted from database");
            return false;
        }
        _dbContext.Levels.Remove(item);
        _dbContext.SaveChanges();

        if (!_dbContext.Scores.Any(x => x.ID == ID))
            return true;
        else
            Log.Error($"ID: {ID} was not deleted from database");
        return false;
    }
    public Level? GetLevel(int ID)
    {
        if (LevelIDExists(ID))
            return _dbContext.Levels.Find(ID);
        return null;
    }
    public IEnumerable<Level>? GetAllLevels()
    {
        if (_dbContext.Levels.Any())
            return _dbContext.Levels.ToList();
        return null;
    }
    public bool LevelIDExists(int ID)
    {
        if (_dbContext.Levels.Any(x => x.ID == ID))
        {
            return true;
        }
        else
        {
            Log.Information($"ID: {ID} does not exist in database");
            return false;
        }
    }
    public int LevelsCount()
    {
        return _dbContext.Levels.Count();
    }

    // Player
    public bool InsertPlayer(Player item)
    {
        // Insert value into DB
        _dbContext.Update(item);
        // Check if item was updated
        if (_dbContext.Entry(item).State == EntityState.Unchanged)
            return false;
        _dbContext.SaveChanges();
        return true;
    }
    public bool UpdatePlayer(Player item)
    {
        // Update value in DB
        _dbContext.Update(item);
        // Check if item was updated
        if (_dbContext.Entry(item).State == EntityState.Unchanged)
            return false;
        _dbContext.SaveChanges();
        return true;
    }
    public bool DeletePlayer(int ID)
    {
        Player? item = GetPlayer(ID);
        if (item == null)
        {
            Log.Error($"ID: {ID} was not deleted from database");
            return false;
        }
        _dbContext.Players.Remove(item);
        _dbContext.SaveChanges();

        if (!_dbContext.Players.Any(x => x.ID == ID))
            return true;
        else
            Log.Error($"ID: {ID} was not deleted from database");
        return false;
    }
    public Player? GetPlayer(int ID)
    {
        if (PlayerIDExists(ID))
            return _dbContext.Players.Find(ID);
        return null;
    }
    public IEnumerable<Player>? GetAllPlayers()
    {
        if (_dbContext.Players.Any())
            return _dbContext.Players.ToList();
        return null;
    }
    public IEnumerable<Player>? GetAllPlayers(string phrase) // Search
    {
        if (_dbContext.Players.Any())
        {
            //string pattern = @"\s+";
            //string replacement = "";

            IEnumerable<Player>? players = _dbContext.Players.ToList();

            Regex rx = new($".*{phrase}.*", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            return players.Where(x => rx.IsMatch(Regex.Replace(x.Username, @"\s+", ""))).ToList();
        }
        return null;
    }
    public bool PlayerIDExists(int ID)
    {
        if (_dbContext.Players.Any(x => x.ID == ID))
        {
            return true;
        }
        else
        {
            Log.Information($"ID: {ID} does not exist in database");
            return false;
        }
    }
    public async Task<bool> UsernameIsTaken(string username)
    {
        return await _dbContext.Players.AnyAsync(x => x.Username == username);
    }
    public int PlayersCount()
    {
        return _dbContext.Players.Count();
    }

    // Score
    public bool InsertScore(Score item)
    {
        // Insert value into DB
        _dbContext.Update(item);
        // Check if item was updated
        if (_dbContext.Entry(item).State == EntityState.Unchanged)
            return false;
        _dbContext.SaveChanges();
        return true;
    }
    public bool UpdateScore(Score item)
    {
        // Update value in DB
        _dbContext.Update(item);
        // Check if item was updated
        if (_dbContext.Entry(item).State == EntityState.Unchanged)
            return false;
        _dbContext.SaveChanges();
        return true;
    }
    public bool DeleteScore(int ID)
    {
        Score? item = GetScore(ID);
        if (item == null)
        {
            Log.Error($"ID: {ID} was not deleted from database");
            return false;
        }
        _dbContext.Scores.Remove(item);
        _dbContext.SaveChanges();

        if (!_dbContext.Scores.Any(x => x.ID == ID))
            return true;
        else
            Log.Error($"ID: {ID} was not deleted from database");
        return false;
    }
    public Score? GetScore(int ID)
    {
        if (ScoreIDExists(ID))
            return _dbContext.Scores.Find(ID);
        return null;
    }
    public IEnumerable<Score>? GetAllScores()
    {
        if (_dbContext.Scores.Any())
            return _dbContext.Scores.ToList();
        return null;
    }
    public IEnumerable<Score>? GetAllScores(int searchID, bool playerOrLevel) // Search by player OR level ID
    {
        if (playerOrLevel && PlayerIDExists(searchID))
            return _dbContext.Scores.Where(x => x.PlayerID == searchID).ToList();
        else if (!playerOrLevel && LevelIDExists(searchID))
            return _dbContext.Scores.Where(x => x.LevelID == searchID).ToList();
        return null;
    }
    public IEnumerable<Score>? GetAllScores(int playerID, int levelID) // Search by player AND level ID
    {
        if (PlayerIDExists(playerID) && LevelIDExists(levelID))
            return _dbContext.Scores.Where(x => x.PlayerID == playerID && x.LevelID == levelID).ToList();
        return null;
    }
    public bool ScoreIDExists(int ID)
    {
        if (_dbContext.Scores.Any(x => x.ID == ID))
        {
            return true;
        }
        else
        {
            Log.Information($"ID: {ID} does not exist in database");
            return false;
        }
    }
    public int ScoresCount()
    {
        return _dbContext.Scores.Count();
    }
}
