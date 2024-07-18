using VT_ALLORANT.Model.Valorant;
using VT_ALLORANT.Model.Discord;
using VT_ALLORANT.Controller;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace VT_ALLORANT.Model;

public enum PlayerRanks
{
    Unranked = 1,
    Iron1 = 2,
    Iron2 = 3,
    Iron3 = 4,
    Bronze1 = 5,
    Bronze2 = 6,
    Bronze3 = 7,
    Silver1 = 8,
    Silver2 = 9,
    Silver3 = 10,
    Gold1 = 11,
    Gold2 = 12,
    Gold3 = 13,
    Platinum1 = 14,
    Platinum2 = 15,
    Platinum3 = 16,
    Diamond1 = 17,
    Diamond2 = 18,
    Diamond3 = 19,
    Ascendant1 = 20,
    Ascendant2 = 21,
    Ascendant3 = 22,
    Immortal1 = 23,
    Immortal2 = 24,
    Immortal3 = 25,
    Radiant = 26
}

[Table("Players")]
public class Player
{
    [Key]
    [ForeignKey("PlayerId")]
    public int PlayerId { get; set; }
    public string? Name { get; set; }
    public PlayerRanks Rank { get; set; } = PlayerRanks.Unranked;
    public float RankedScore => RankScore.GetRank((int)Rank).Score;
    public int DiscordUserId { get; set; }
    public DiscordUser DiscordUser { get; set; }
    public int ValorantUserId { get; set; }
    public ValorantUser ValorantUser { get; set; }
    public ICollection<Team>? Teams { get; set; } = [];
    public ICollection<Tournament>? Tournaments { get; set; } = [];
    public ICollection<Game>? ObserverInGames { get; set; } = [];
    public bool CanChangeRank { get; set; } = true;
    public bool IsInAnyTeam => Teams!.Count != 0;    

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
        return dBAccess.Players.Include(d => d.DiscordUser)
                               .Include(v => v.ValorantUser)
                               .Include(t => t.Teams)
                               .Include(t => t.Tournaments)
                               .Include(t => t.ObserverInGames)
                               .FirstOrDefault(predicate);
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
                                   .Include(t => t.Teams)
                                   .Include(t => t.Tournaments)
                                   .Include(t => t.ObserverInGames)
                                   .Where(player => player.Teams!.Contains(t))];
    }

    internal void SaveChanges()
    {
        using DBAccess dBAccess = new();
        dBAccess.Update(this);
        dBAccess.SaveChanges();
    }
}