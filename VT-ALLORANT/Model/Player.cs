using VT_ALLORANT.Model.Valorant;
using VT_ALLORANT.Model.Discord;
using VT_ALLORANT.Controller;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VT_ALLORANT.Model;

[Table("Player")]
public class Player
{
    // Properties
    [Key]
    [ForeignKey("PlayerId")]
    public int PlayerId { get; set; }  // Unique ID of the player
    public string Name { get; set; } = "unset"; // Default value "unset
    public int DiscordUserId { get; set; }  // Discord User ID
    public DiscordUser DiscordUser { get; set; }  // Discord User
    public int ValorantUserId { get; set; }  // Valorant User ID
    public ValorantUser ValorantUser { get; set; }  // Valorant User
    [NotMapped]
    public List<Team>? Teams { get; set; } // Team of the player
    // Constructor
    public Player()
    {

    }

    public static Player CreatePlayer(string name, DiscordUser discordUser, ValorantUser valorantUser)
    {
        return new Player()
        {
            Name = name,
            DiscordUser = discordUser,
            ValorantUser = valorantUser
        };
    }

    public void InsertPlayer()
    {
        DBPlayer dBAccess = new();
        dBAccess.Add(this);
    }

    public static Player LoadPlayer(int id)
    {
        DBPlayer dBAccess = new();
        return dBAccess.GetById(id);
    }

    internal static Player LoadPlayer(ulong id)
    {
        DBPlayer dBAccess = new();
        return dBAccess.GetByDiscordID(id);
    }

    public void SaveChanges()
    {
        DBPlayer dBAccess = new();
        dBAccess.Update(this);
    }
    public void DeletePlayer()
    {
       DBPlayer dBAccess = new();
        dBAccess.Delete(this);
    }

    public static List<Player> GetAll()
    {
        DBPlayer dBAccess = new();
        return dBAccess.GetAll();
    }
}