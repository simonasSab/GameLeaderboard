using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaderboardBackEnd.Models
{
    [Table("Scores")]
    public class Score
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Player")]
        public int PlayerID { get; set; }
        [ForeignKey("Level")]
        public int LevelID { get; set; }
        public int Points { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime CreationDateTime { get; set; } = DateTime.Now;
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
    }
}
