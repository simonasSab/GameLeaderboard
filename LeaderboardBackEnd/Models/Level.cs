using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaderboardBackEnd.Models
{
    [Table("Levels")]
    public class Level
    {
        [Key]
        public int ID { get; set; }
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
    }
}
