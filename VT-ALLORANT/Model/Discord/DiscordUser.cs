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
        DBDiscordUser dBAccess = new();
        dBAccess.Add(this);
    }
    public void SaveChanges()
    {
        DBDiscordUser dBAccess = new();
        dBAccess.Update(this);
    }

    public void DeleteUser()
    {
        DBDiscordUser dBAccess = new();
        dBAccess.Delete(this);
    }

    public static DiscordUser LoadUser(int Id)
    {
        DBDiscordUser dBAccess = new();
        return dBAccess.GetById(Id);
    }

    public static DiscordUser LoadUser(ulong Id)
    {
        DBDiscordUser dBAccess = new();
        return dBAccess.GetByUUID(Id);
    }

    public static List<DiscordUser> GetAll()
    {
        DBDiscordUser dBAccess = new();
        return dBAccess.GetAll();
    }
}