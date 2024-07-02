using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeaderboardBackEnd.Services;

namespace LeaderboardBackEnd.Models;

[Table("Players")]
public class Player
{
    [Key]
    public int ID { get; set; }
    [BsonIgnore]
    public ICollection<Score> Scores { get; set; } // this.ID is FK for Score

    public string Username { get; set; }

    [BsonIgnore]
    private string _password;
    public string Password
    {
        get
        {
            return _password;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                _password = "";
            else
                _password = EncryptionHelper.Encrypt(value);
        }
    }

    [BsonIgnore]
    private string _email;
    public string Email
    {
        get
        {
            return _email;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                _email = "";
            else
                _email = EncryptionHelper.Encrypt(value);
        }
    }

    public TimeSpan TimePlayed { get; set; } = TimeSpan.Zero;
    public DateTime CreationDateTime { get; set; } = DateTime.Now;
    [NotMapped] [BsonId]
    public ObjectId MongoID { get; set; }

    public Player()
    { }
    public Player(string username, string password, string email)
    {
        Username = username;
        Password = password;
        Email = email;
    }
    public Player(int iD, string username, string password, string email, TimeSpan timePlayed, DateTime creationDateTime, ObjectId mongoID)
    {
        ID = iD;
        Username = username;
        Password = password;
        Email = email;
        TimePlayed = timePlayed;
        CreationDateTime = creationDateTime;
        MongoID = mongoID;
    }

    public bool AuthenticatePassword(string passwordAttempt)
    {
        string password = passwordAttempt.Trim();
        if (string.IsNullOrEmpty(password))
            return false;
        if (password == EncryptionHelper.Decrypt(_password))
            return true;
        return false;
    }

    public bool AuthenticateEmail(string emailAttempt)
    {
        string email = emailAttempt.Trim();
        if (string.IsNullOrEmpty(email))
            return false;
        if (email == EncryptionHelper.Decrypt(_email))
            return true;
        return false;
    }

    public override string ToString()
    {
        return $"ID {ID}, username {Username}, time played {TimePlayed.Hours:00}:{TimePlayed.Minutes:00}:{TimePlayed.Seconds:00} since {CreationDateTime}";
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)                            return false;
        Player player = (Player)obj;
        if (player.ID != this.ID)                   return false;
        if (player.Username != this.Username)       return false;
        if (player.Email != this.Email)             return false;
        if (player.TimePlayed != this.TimePlayed)   return false;

        return true;
    }
}
