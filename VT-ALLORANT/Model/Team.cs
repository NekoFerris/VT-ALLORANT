using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VT_ALLORANT.Controller;
using System.Reactive.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace VT_ALLORANT.Model;

[Table("Teams")]
public class Team
{

    [Key]
    [ForeignKey("TeamId")]
    public int TeamId { get; set; }
    public string Name { get; set; } = "unset";
    public byte MaxPlayers { get; set; } = 5;
    public int? LeaderId { get; set; }
    public Player Leader { get; set; } = null!;
    public ICollection<Player> Players { get; set; } = [];
    public ICollection<Tournament> Tournaments { get; set; } = [];
    public int PlayerCount => Players.Count;
    public int TeamRank => Players.Sum(p => (int)p.RankedScore);
    public static ulong DiscordRoleId { get; set; }

    public static void Create(string name, Player leader)
    {
        using DBAccess dBAccess = new();
        Team teamToAdd = new()
        {
            Name = name,
            Leader = leader
        };
        dBAccess.Teams.Attach(teamToAdd);
        dBAccess.SaveChanges();
        teamToAdd.AddPlayer(leader);
    }

    public void AddPlayer(Player player)
    {
        using DBAccess dBAccess = new();
        dBAccess.TeamPlayers.Attach(new TeamPlayer()
        {
            Team = this,
            Player = player
        });
        dBAccess.SaveChanges();
    }

    public void RemovePlayer(Player player)
    {
        using DBAccess dBAccess = new();
        dBAccess.TeamPlayers.Remove(dBAccess.TeamPlayers.Find(this.TeamId, player.PlayerId)!);
        dBAccess.SaveChanges();
    }

    public static Team? LoadTeam(int id)
    {
        using DBAccess dBAccess = new();
        Team? t = dBAccess.Teams.Find(id)!;
        t.Leader = Player.Load(player => player.PlayerId == t.LeaderId)!;
        t.Players = Player.GetPlayersForTeam(t);
        return t;
    }

    public static Team? LoadTeam(string name)
    {
        using DBAccess dBAccess = new();
        Team? t = dBAccess.Teams .Include(t => t.Leader)
                                .Include(t => t.Players)
                                    .ThenInclude(p => p.ValorantUser)
                                .Include(t => t.Players)
                                    .ThenInclude(p => p.DiscordUser)
                                .FirstOrDefault(t => t.Name == name);
        return t;
    }

    public static Team? Load(Func<Team, bool> predicate)
    {
        using DBAccess dBAccess = new();
        Team? t = dBAccess.Teams .Include(t => t.Leader)
                                .Include(t => t.Players)
                                    .ThenInclude(p => p.ValorantUser)
                                .Include(t => t.Players)
                                    .ThenInclude(p => p.DiscordUser)
                                .FirstOrDefault(predicate);
        return t;
    }

    public void Delete()
    {
        using DBAccess dBAccess = new();
        dBAccess.Remove(this);
        dBAccess.SaveChanges();
    }

    public static List<Team> GetAll()
    {
        using DBAccess dBAccess = new();
        return [.. dBAccess .Teams
                            .Include(t => t.Leader)
                            .Include(t => t.Players)
                                .ThenInclude(p => p.ValorantUser)
                            .Include(t => t.Players)
                                .ThenInclude(p => p.DiscordUser)];;
    }

    public void SetLeader(Player player)
    {
        using DBAccess dBAccess = new();
        Player? existingPlayer = Players.FirstOrDefault(p => p.PlayerId == player.PlayerId);
        Leader = existingPlayer!;
        LeaderId = existingPlayer!.PlayerId;
        dBAccess.Teams.Find(this.TeamId)!.LeaderId = LeaderId;
        dBAccess.SaveChanges();
    }
}