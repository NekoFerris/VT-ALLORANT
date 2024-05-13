using VT_ALLORANT.Model.Valorant;
using VT_ALLORANT.Model.Discord;
using VT_ALLORANT.Controller;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VT_ALLORANT.Model;

[Table("DiscordPlayer")]
public class Player
{
    // Properties
    [Key]
    public int Id { get; set; }  // Unique ID of the player
    public string Name { get; set; } = "unset"; // Default value "unset
    public DiscordUser DiscordUser { get; set; }  // Discord User
    public ValorantUser ValorantUser { get; set; }  // Valorant User


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

    public void LoadPlayer(int id)
    {
        DBPlayer dBAccess = new();
        dBAccess.GetById(id);
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

    public List<Player> GetAll()
    {
        DBPlayer dBAccess = new();
        return dBAccess.GetAll();
    }
}