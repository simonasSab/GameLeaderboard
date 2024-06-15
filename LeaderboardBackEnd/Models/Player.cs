using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaderboardBackEnd.Models;

[Table("Players")]
public class Player
{
    [Key]
    private int _id;
    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }
    private string _username;
    public string Username
    {
        get { return _username; }
        set { _username = value; }
    }
    private string _password;
    public string Password
    {
        get { return _password; }
        set { _password = value; }
    }
    private string _email;
    public string Email
    {
        get { return _email; }
        set { _email = value; }
    }
    private TimeSpan _timePlayed = TimeSpan.Zero;
    public TimeSpan TimePlayed
    {
        get { return _timePlayed; }
        set { _timePlayed = value; }
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
        this.MongoID = mongoID;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)                        return false;
        Player player = (Player)obj;
        if (player.ID != this.ID)               return false;
        if (player.Username != this.Username)   return false;
        if (player.Password != this.Password)   return false;
        if (player.Email != this.Email)         return false;

        return true;
    }
}
