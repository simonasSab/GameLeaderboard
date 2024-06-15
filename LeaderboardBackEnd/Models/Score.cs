using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaderboardBackEnd.Models;

[Table("Scores")]
public class Score
{
    [Key]
    private int _id;
    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }
    [ForeignKey("Player")]
    private int _playerID;
    public int PlayerID
    {
        get { return _playerID; }
        set { _playerID = value; }
    }
    [ForeignKey("Level")]
    private int _levelID;
    public int LevelID
    {
        get { return _levelID; }
        set { _levelID = value; }
    }
    private int _points;
    public int Points
    {
        get { return _points; }
        set { _points = value; }
    }
    private TimeSpan _time;
    public TimeSpan Time
    {
        get { return _time; }
        set { _time = value; }
    }
    private DateTime _creationDateTime = DateTime.Now;
    public DateTime CreationDateTime
    {
        get { return _creationDateTime; }
        set { _creationDateTime = value; }
    }
    private ObjectId _mongoID;
    [NotMapped] [BsonId]
    public ObjectId MongoID
    {
        get { return _mongoID; }
        set { _mongoID = value; }
    }

    public Score()
    { }
    public Score(int iD)
    {
        ID = iD;
    }
    public Score(int playerID, int levelID, int points, TimeSpan time)
    {
        PlayerID = playerID;
        LevelID = levelID;
        Points = points;
        Time = time;
    }
    public Score(int iD, int playerID, int levelID, int points, TimeSpan time, DateTime creationDateTime, ObjectId mongoID)
    {
        ID = iD;
        PlayerID = playerID;
        LevelID = levelID;
        Points = points;
        Time = time;
        CreationDateTime = creationDateTime;
        this.MongoID = mongoID;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)                        return false;
        Score score = (Score)obj;
        if (score.ID != this.ID)                return false;
        if (score.PlayerID != this.PlayerID)    return false;
        if (score.LevelID != this.LevelID)      return false;
        if (score.Points != this.Points)        return false;
        if (score.Time != this.Time)            return false;

        return true;
    }
}
