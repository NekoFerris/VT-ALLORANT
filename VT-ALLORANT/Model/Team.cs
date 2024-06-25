using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VT_ALLORANT.Controller;
using Microsoft.EntityFrameworkCore;
using System.Reactive.Linq;
using VT_ALLORANT.Model.Valorant;
using VT_ALLORANT.Model.Discord;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace VT_ALLORANT.Model;

[Table("Team")]
public class Team
{

    [Key]
    [ForeignKey("TeamId")]
    public int TeamId { get; set; }
    public string Name { get; set; } = "unset";
    public int? LeaderId { get; set; }
    public Player Leader { get; set; }
    public ICollection<Player> Players { get; set; } = [];

    public Team()
    {

    }

    public static void CreateTeam(string name, Player leader)
    {
        using DBAccess dBAccess = new();
        dBAccess.Teams.Add(new Team()
        {
            Name = name,
            Leader = leader
        });
        dBAccess.SaveChanges();
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
        dBAccess.TeamPlayers.Remove(dBAccess.TeamPlayers.Find(this.TeamId, player.PlayerId));
        dBAccess.SaveChanges();
    }

    public static Team LoadTeam(int id)
    {
        using DBAccess dBAccess = new();
        Team t = dBAccess.Teams.Find(id);
        t.Leader = Player.LoadPlayer(t.LeaderId);
        t.Players = Player.GetPlayersForTeam(t);
        return t;
    }

    public static Team LoadTeam(Player leader)
    {
        using DBAccess dBAccess = new();
        Team t = dBAccess.Teams.FirstOrDefault(t => t.LeaderId == leader.PlayerId) ?? throw new Exception($"Team für Spieler {leader.DiscordUser.Username} nicht gefunden");
        t.Players = Player.GetPlayersForTeam(t);
        t.Leader = t.Players.FirstOrDefault(p => p.PlayerId == t.LeaderId) ?? throw new Exception($"Anführer für Team {t.Name} nicht gefunden");
        return t;
    }

    public void Delete()
    {
        using DBAccess dBAccess = new();
        dBAccess.Remove(this);
    }

    public static List<Team> GetAll()
    {
        using DBAccess dBAccess = new();
        return dBAccess.Teams.ToList();
    }

    internal void Update()
    {
        using DBAccess dBAccess = new();
        dBAccess.Update(this);
    }
}