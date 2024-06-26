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
    private string? _password;
    public string? Password
    {
        get
        {
            if (_password != null)
                return EncryptionHelper.Decrypt(_password);
            else
                return null;
        }
        set
        {
            if (value != null)
                _password = EncryptionHelper.Encrypt(value);
            else
                _password = null;
        }
    }

    [BsonIgnore]
    private string? _email;
    public string? Email
    {
        get
        {
            if (_email != null)
                return EncryptionHelper.Decrypt(_email);
            else
                return null;
        }
        set
        {
            if (value != null)
                _email = EncryptionHelper.Encrypt(value);
            else
                _email = null;
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

    public bool AuthenticateEmail(string? emailAttempt)
    {
        if (string.IsNullOrEmpty(emailAttempt))
            return false;
        if (emailAttempt == Email)
            return true;
        return false;
    }

    public bool AuthenticatePassword(string? passwordAttempt)
    {
        if (string.IsNullOrEmpty(passwordAttempt))
            return false;
        if (passwordAttempt == Password)
            return true;
        return false;
    }
}
