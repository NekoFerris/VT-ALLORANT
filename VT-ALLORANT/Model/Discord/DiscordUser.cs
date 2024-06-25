using VT_ALLORANT.Controller;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VT_ALLORANT.Model.Discord;

[Table("DiscordUser")]
public class DiscordUser
{
    // Properties
    [Key]
    [ForeignKey("DiscordUserId")]
    public int DiscordUserId { get; set; }
    public string Username { get; set; }
    public ulong DiscordId { get; set; }
    // Constructor
    public DiscordUser()
    {

    }

    // Methods
    public void SendMessage(string message)
    {
        // Code to send a message to the user
    }
    public void InsertUser()
    {
        DBAccess dBAccess = new();
        dBAccess.Add(this);
        dBAccess.SaveChanges();
    }
    public void SaveChanges()
    {
        DBAccess dBAccess = new();
        dBAccess.Update(this);
        dBAccess.SaveChanges();
    }

    public void DeleteUser()
    {
        DBAccess dBAccess = new();
        dBAccess.Remove(this);
        dBAccess.SaveChanges();
    }

    public static DiscordUser LoadUser(int Id)
    {
        DBAccess dBAccess = new();
        return dBAccess.DiscordUsers.Find(Id);
    }

    public static DiscordUser LoadUser(ulong Id)
    {
        DBAccess dBAccess = new();
        return dBAccess.DiscordUsers.Find(Id);
    }

    public static List<DiscordUser> GetAll()
    {
        DBAccess dBAccess = new();
        return dBAccess.DiscordUsers.ToList();
    }
}