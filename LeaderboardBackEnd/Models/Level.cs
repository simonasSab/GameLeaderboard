using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaderboardBackEnd.Models;

[Table("Levels")]
public class Level
{
    [Key]
    public int ID { get; set; }
    [BsonIgnore]
    public ICollection<Score> Scores { get; set; } // this.ID is FK for Score

    public int MaxScore { get; set; }

    [NotMapped] [BsonId]
    public ObjectId MongoID { get; set; }

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

    public override string ToString()
    {
        return $"Level no. {ID}, max score {MaxScore}";
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
