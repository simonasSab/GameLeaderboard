using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaderboardBackEnd.Models;

[Table("Levels")]
public class Level
{
    [Key]
    private int _id;
    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }

    private int _maxScore;
    public int MaxScore
    {
        get { return _maxScore; }
        set { _maxScore = value; }
    }

    [NotMapped] [BsonId]
    private ObjectId _mongoID;
    public ObjectId MongoID
    {
        get { return _mongoID; }
        set { _mongoID = value; }
    }

    public Level()
    { }
    public Level(int maxScore)
    {
        MaxScore = maxScore;
    }
    public Level(int iD, int maxScore, ObjectId mongoID)
    {
        ID = iD;
        MaxScore = maxScore;
        this.MongoID = mongoID;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)                        return false;
        Level level = (Level)obj;
        if (level.ID != this.ID)                return false;
        if (level.MaxScore != this.MaxScore)    return false;

        return true;
    }
}
