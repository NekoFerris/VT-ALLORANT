namespace VT_ALLORANT.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using VT_ALLORANT.Controller;

public class Tournament
{
    // Properties
    public int TournamentId { get; set; }
    public string Name { get; set; } = "unset";
    public ICollection<Team> Teams { get; set; } = [];
    public ICollection<Player> Moderators { get; set; } = [];
    public ICollection<Player> Observers { get; set; } = [];
    public ICollection<Game> Games { get; set; } = [];
    public bool OpenForRegistration { get; set; } = false;
    public int MaxTeamRank { get; set; } = 0;
    public int CurrentStage { get; set; } = 0;
    public int MaxTeams { get; set; }
    public PlayerRanks MinRank { get; set; } = PlayerRanks.Unranked;
    public PlayerRanks MaxRank { get; set; } = PlayerRanks.Radiant;

    // Constructor
    public Tournament()
    {
        TournamentId = 1;
    }

    // Methods
    public void AddTeam(Team team)
    {
        using DBAccess dBAccess = new();
        dBAccess.TournamentTeams.Attach(new TournamentTeam()
        {
            Tournament = this,
            Team = team
        });
        dBAccess.SaveChanges();
    }

    public void RemoveTeam(Team team)
    {
        using DBAccess dBAccess = new();
        dBAccess.TournamentTeams.Remove(dBAccess.TournamentTeams.Find(team.TeamId, TournamentId) ?? throw new Exception($"Team {team.Name} nicht im Turnier {this.Name} gefunden"));
        dBAccess.SaveChanges();
    }

    public static Tournament Load(int tournamentId)
    {
        using DBAccess dBAccess = new();
        return dBAccess.Tournaments.Include(t => t.Teams).Include(t => t.Moderators).Include(t => t.Observers).Include(t => t.Games).FirstOrDefault(t => t.TournamentId == tournamentId) ?? throw new Exception("Kein Turnier gefunden");
    }

    public void AddModerator(Player moderator)
    {
        using DBAccess dBAccess = new();
        dBAccess.TournamentModerators.Attach(new TournamentModerator()
        {
            Tournament = this,
            Moderator = moderator
        });
        dBAccess.SaveChanges();
    }

    public void RemoveModerator(Player moderator)
    {
        using DBAccess dBAccess = new();
        dBAccess.TournamentModerators.Remove(dBAccess.TournamentModerators.Find(this.TournamentId, moderator.PlayerId)!);
        dBAccess.SaveChanges();
    }

    public void AddObserver(Player player)
    {
        using DBAccess dBAccess = new();
        dBAccess.TournamentObservers.Attach(new TournamentObserver()
        {
            Tournament = this,
            Observer = player
        });
        dBAccess.SaveChanges();
    }

    public void RemoveObserver(Player player)
    {
        DBAccess dBAccess = new();
        dBAccess.TournamentObservers.Remove(dBAccess.TournamentObservers.Find(this.TournamentId, player.PlayerId)!);
        dBAccess.SaveChanges();
    }

    internal static void Create(string v)
    {
        using DBAccess dBAccess = new();
        Tournament tournamentToAdd = new()
        {
            Name = v,
            OpenForRegistration = true,
            MaxTeamRank = 11,
            MaxTeams = 128,
            CurrentStage = 0
        };
        dBAccess.Tournaments.Add(tournamentToAdd);
        dBAccess.SaveChanges();
    }

    internal static ICollection<Tournament> GetAll()
    {
        using DBAccess dBAccess = new();
        return [.. dBAccess.Tournaments.Include(t => t.Teams).Include(t => t.Moderators).Include(t => t.Observers).Include(t => t.Games)];
    }
}