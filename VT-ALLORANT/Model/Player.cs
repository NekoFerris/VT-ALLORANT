using VT_ALLORANT.Model.Valorant;
using VT_ALLORANT.Model.Discord;
using VT_ALLORANT.Controller;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace VT_ALLORANT.Model;

public enum PlayerRankedScore
{
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
public class Player(string name, DiscordUser discordUser, ValorantUser valorantUser)
{
    // Properties
    [Key]
    [ForeignKey("PlayerId")]
    public int PlayerId { get; set; }
    public string Name { get; set; } = name;
    public PlayerRankedScore RankedScore { get; set; } = PlayerRankedScore.Iron1;
    public int DiscordUserId { get; set; }
    public DiscordUser DiscordUser { get; set; } = discordUser;
    public int ValorantUserId { get; set; }
    public ValorantUser ValorantUser { get; set; } = valorantUser;
    public ICollection<Team>? Teams { get; set; }
    public ICollection<Tournament>? Tournaments { get; set; }

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
        Player player = dBAccess.Players.Include(d => d.DiscordUser)
                                        .Include(v => v.ValorantUser)
                                        .FirstOrDefault(player => player.PlayerId == id) ?? throw new Exception("Player not found");
        return player;
    }

    internal static Player LoadPlayer(ulong id)
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