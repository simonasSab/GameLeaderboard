using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaderboardBackEnd.Models;

[Table("Scores")]
public class Score
{
    [Key]
    public int ID { get; set; }

    [ForeignKey("Player")]
    public int PlayerID {  get; set; }
    public Player Player { get; set; }
    
    [ForeignKey("Level")]
    public int LevelID { get; set; }
    public Level Level { get; set; }

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

    public DateTime CreationDateTime { get; set; }
    
    [NotMapped] [BsonId]
    public ObjectId MongoID { get; set; }


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

    public override string ToString()
    {
        return $"ID {ID}, PlayerID {PlayerID}, LevelID {LevelID}, {Points} points, time: {Time:mm:ss:fff} ({CreationDateTime})";
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
