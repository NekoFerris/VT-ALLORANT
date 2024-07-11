using VT_ALLORANT.Model.Valorant;
using VT_ALLORANT.Model.Discord;
using VT_ALLORANT.Controller;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace VT_ALLORANT.Model;

public enum PlayerRankedScore
{
    Iron1 = 10,
    Iron2 = 10,
    Iron3 = 10,
    Bronze1 = 15,
    Bronze2 = 15,
    Bronze3 = 15,
    Silver1 = 20,
    Silver2 = 20,
    Silver3 = 20,
    Gold1 = 25,
    Gold2 = 25,
    Gold3 = 25,
    Platinum1 = 30,
    Platinum2 = 30,
    Platinum3 = 30,
    Diamond1 = 40,
    Diamond2 = 40,
    Diamond3 = 40,
    Immortal1 = 50,
    Immortal2 = 50,
    Immortal3 = 50,
    Radiant = 60
}

public enum PlayerRanks
{
    Iron1 = 1,
    Iron2 = 2,
    Iron3 = 3,
    Bronze1 = 4,
    Bronze2 = 5,
    Bronze3 = 6,
    Silver1 = 7,
    Silver2 = 8,
    Silver3 = 9,
    Gold1 = 10,
    Gold2 = 11,
    Gold3 = 12,
    Platinum1 = 13,
    Platinum2 = 14,
    Platinum3 = 15,
    Diamond1 = 16,
    Diamond2 = 17,
    Diamond3 = 18,
    Immortal1 = 19,
    Immortal2 = 20,
    Immortal3 = 21,
    Radiant = 22
}

[Table("Players")]
public class Player
{
    // Properties
    [Key]
    [ForeignKey("PlayerId")]
    public int PlayerId { get; set; }
    public string Name { get; set; }
    public PlayerRanks Rank { get; set; }
    public int DiscordUserId { get; set; }
    public DiscordUser DiscordUser { get; set; }
    public int ValorantUserId { get; set; }
    public ValorantUser ValorantUser { get; set; }
    public ICollection<Team>? Teams { get; set; }
    public ICollection<Tournament>? Tournaments { get; set; }
    public ICollection<Game>? ObserverInGames { get; set; }

    public Player()
    {
    }

    public Player(string name, DiscordUser discordUser, ValorantUser valorantUser)
    {
        Name = name;
        DiscordUser = discordUser;
        ValorantUser = valorantUser;
    }

    public void Insert()
    {
        DBAccess dBAccess = new();
        dBAccess.Add(this);
        dBAccess.SaveChanges();
    }

    public void Delete()
    {
        DBAccess dBAccess = new();
        dBAccess.Remove(this);
        dBAccess.SaveChanges();
    }

    public static Player Load(int? id)
    {
        DBAccess dBAccess = new();
        Player player = dBAccess.Players.Include(d => d.DiscordUser)
                                        .Include(v => v.ValorantUser)
                                        .FirstOrDefault(player => player.PlayerId == id) ?? throw new Exception("Player not found");
        return player;
    }

    internal static Player Load(ulong id)
    {
        DBAccess dBAccess = new();
        Player player = dBAccess.Players.Include(d => d.DiscordUser)
                                        .Include(v => v.ValorantUser)
                                        .FirstOrDefault(player => player.DiscordUser.DiscordId == id) ?? throw new Exception("Player not found");
        return player;
    }

    public static List<Player> GetAll()
    {
        DBAccess dBAccess = new();
        return [.. dBAccess.Players.Include(d => d.DiscordUser).Include(v => v.ValorantUser)];
    }

    public static Player GetPlayerByDiscordUserName(string name)
    {
        DBAccess dBAccess = new();
        Player player = dBAccess.Players.Include(d => d.DiscordUser)
                                        .Include(v => v.ValorantUser)
                                        .FirstOrDefault(player => player.DiscordUser.Username == name) ?? throw new Exception("Player not found");
        return player;
    }

    internal static List<Player> GetPlayersForTeam(Team t)
    {
        DBAccess dBAccess = new();
        return [.. dBAccess.Players.Include(d => d.DiscordUser)
                                   .Include(v => v.ValorantUser)
                                   .Where(player => player.Teams!.Contains(t))];
    }
}