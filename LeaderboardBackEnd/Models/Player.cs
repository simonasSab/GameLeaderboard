using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaderboardBackEnd.Models
{
    [Table("Players")]
    public class Player
    {
        [Key]
        public int ID { get; set; }
        public int Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public TimeSpan TimePlayed { get; set; } = TimeSpan.Zero;
        public DateTime CreationDateTime { get; set; } = DateTime.Now;
        [NotMapped] [BsonId]
        public ObjectId MongoID { get; set; }

        public Player()
        { }
        public Player(int username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
        }
        public Player(int iD, int username, string password, string email, TimeSpan timePlayed, DateTime creationDateTime, ObjectId mongoID)
        {
            ID = iD;
            Username = username;
            Password = password;
            Email = email;
            TimePlayed = timePlayed;
            CreationDateTime = creationDateTime;
            this.MongoID = mongoID;
        }
    }
}
