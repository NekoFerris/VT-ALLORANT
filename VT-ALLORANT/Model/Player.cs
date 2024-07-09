using VT_ALLORANT.Model.Valorant;
using VT_ALLORANT.Model.Discord;
using VT_ALLORANT.Controller;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace VT_ALLORANT.Model;

public enum PlayerRankedScore
{
    None,
    Iron1,
    Iron2,
    Iron3,
    Bronze1,
    Bronze2,
    Bronze3,
    Silver1,
    Silver2,
    Silver3,
    Gold1,
    Gold2,
    Gold3,
    Platinum1,
    Platinum2,
    Platinum3,
    Diamond1,
    Diamond2,
    Diamond3,
    Immortal1,
    Immortal2,
    Immortal3,
    Radiant
}

[Table("Players")]
public class Player
{
    // Properties
    [Key]
    [ForeignKey("PlayerId")]
    public int PlayerId { get; set; }
    public string Name { get; set; }
    public PlayerRankedScore RankedScore { get; set; } = PlayerRankedScore.None;
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