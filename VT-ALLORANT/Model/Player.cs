using VT_ALLORANT.Model.Valorant;
using VT_ALLORANT.Model.Discord;
using VT_ALLORANT.Controller;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace VT_ALLORANT.Model;

public enum PlayerRanks
{
    Unranked = 0,
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
    Ascendant1 = 19,
    Ascendant2 = 20,
    Ascendant3 = 21,
    Immortal1 = 22,
    Immortal2 = 23,
    Immortal3 = 24,
    Radiant = 25
}

[Table("Players")]
public class Player
{
    // Properties
    [Key]
    [ForeignKey("PlayerId")]
    public int PlayerId { get; set; }
    public string? Name { get; set; }
    public PlayerRanks Rank { get; set; } = PlayerRanks.Unranked;
    public int RankedScore => RankScore.GetRank((int)Rank).Score;
    public int DiscordUserId { get; set; }
    public DiscordUser DiscordUser { get; set; }
    public int ValorantUserId { get; set; }
    public ValorantUser ValorantUser { get; set; }
    public ICollection<Team>? Teams { get; set; }
    public ICollection<Tournament>? Tournaments { get; set; }
    public ICollection<Game>? ObserverInGames { get; set; }
    public bool CanChangeRank { get; set; } = true;

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
        using DBAccess dBAccess = new();
        if (dBAccess.Players.Any(p => p.DiscordUser!.DiscordId == DiscordUser!.DiscordId))
        {
            throw new Exception($"{Name} bereits registriert");
        }
        dBAccess.Add(this);
        dBAccess.SaveChanges();
    }

    public void Delete()
    {
        using DBAccess dBAccess = new();
        if (!dBAccess.Players.Any(p => p.DiscordUser!.DiscordId == DiscordUser!.DiscordId))
        {
            throw new Exception($"{Name} ist nicht registriert");
        }
        dBAccess.Remove(this);
        dBAccess.Remove(DiscordUser);
        dBAccess.Remove(ValorantUser);
        dBAccess.SaveChanges();
    }

    public static Player? Load(Func<Player, bool> predicate)
    {
        using DBAccess dBAccess = new();
        Player? player = dBAccess.Players.Include(d => d.DiscordUser)
                                         .Include(v => v.ValorantUser)
                                         .FirstOrDefault(predicate);
        return player;
    }

    public static List<Player> GetAll()
    {
        using DBAccess dBAccess = new();
        return [.. dBAccess.Players.Include(d => d.DiscordUser).Include(v => v.ValorantUser)];
    }

    internal static List<Player> GetPlayersForTeam(Team t)
    {
        using DBAccess dBAccess = new();
        return [.. dBAccess.Players.Include(d => d.DiscordUser)
                                   .Include(v => v.ValorantUser)
                                   .Where(player => player.Teams!.Contains(t))];
    }

    internal void SaveChanges()
    {
        using DBAccess dBAccess = new();
        dBAccess.Update(this);
        dBAccess.SaveChanges();
    }
}