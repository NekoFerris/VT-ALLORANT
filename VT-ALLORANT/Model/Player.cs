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
    public ICollection<Team>? Teams { get; set; } // Team of the player
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
        DBAccess dBAccess = new();
        dBAccess.Add(this);
        dBAccess.SaveChanges();
    }

    public void DeletePlayer()
    {
        DBAccess dBAccess = new();
        dBAccess.Remove(this);
        dBAccess.SaveChanges();
    }

    public static Player LoadPlayer(int? id)
    {
        DBAccess dBAccess = new();
        Player player = dBAccess.Players.Find(id);
        player.DiscordUser = DiscordUser.LoadUser(player.DiscordUserId);
        player.ValorantUser = ValorantUser.LoadUser(player.ValorantUserId);
        return player;
    }

    internal static Player LoadPlayer(ulong id)
    {
        DBAccess dBAccess = new();
        Player player = dBAccess.Players.FirstOrDefault(player => player.DiscordUser.DiscordId == id);
        player.DiscordUser = DiscordUser.LoadUser(player.DiscordUserId);
        player.ValorantUser = ValorantUser.LoadUser(player.ValorantUserId);
        return player;
    }

    public static List<Player> GetAll()
    {
        DBAccess dBAccess = new();
        List<Player> players = dBAccess.Players.ToList();
        foreach (Player player in players)
        {
            player.DiscordUser = DiscordUser.LoadUser(player.DiscordUserId);
            player.ValorantUser = ValorantUser.LoadUser(player.ValorantUserId);
        }
        return players;
    }

    public static Player GetPlayerByDiscordUserName(string name)
    {
        DBAccess dBAccess = new();
        Player player = dBAccess.Players.FirstOrDefault(player => player.DiscordUser.Username == name);
        player.DiscordUser = DiscordUser.LoadUser(player.DiscordUserId);
        player.ValorantUser = ValorantUser.LoadUser(player.ValorantUserId);
        return player;
    }

    internal static List<Player> GetPlayersForTeam(Team t)
    {
        DBAccess dBAccess = new();
        List<Player> players = dBAccess.Players.Where(player => player.Teams.Contains(t)).ToList();
        foreach (Player player in players)
        {
            player.DiscordUser = DiscordUser.LoadUser(player.DiscordUserId);
            player.ValorantUser = ValorantUser.LoadUser(player.ValorantUserId);
        }
        return players;
    }
}