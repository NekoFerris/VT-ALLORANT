namespace VT_ALLORANT.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using VT_ALLORANT.Controller;

public class Tournament
{
    // Properties
    public int TournamentId { get; set; }  // Unique ID of the tournament
    public string Name { get; set; } = "unset"; // Default value "unset
    public ICollection<Team> Teams { get; set; } = []; // Default value new List<Team>()
    public ICollection<Player> Moderators { get; set; } = []; // Default value new List<Team>()
    public ICollection<Player> Observers { get; set; } = []; // Default value new List<Team>()
    public ICollection<Game> Games { get; set; } = []; // Default value new List<Game>()
    public int CurrentStage { get; set; } = 0; // Default value 0

    // Constructor
    public Tournament()
    {
        TournamentId = 1;
    }

    // Methods
    public void AddTeam(Team team)
    {
        DBAccess dBAccess = new();
        dBAccess.TournamentTeams.Attach(new TournamentTeam()
        {
            Tournament = this,
            Team = team
        });
        dBAccess.SaveChanges();
    }

    public void RemoveTeam(Team team)
    {
        DBAccess dBAccess = new();
        dBAccess.TournamentTeams.Remove(dBAccess.TournamentTeams.Find(this.TournamentId, team.TeamId) ?? throw new Exception($"Team {team.Name} nicht im Turnier {this.Name} gefunden"));
        dBAccess.SaveChanges();
    }

    public static Tournament Load(int tournamentId)
    {
        DBAccess dBAccess = new();
        return dBAccess.Tournaments.Include(t => t.Teams).Include(t => t.Moderators).Include(t => t.Observers).Include(t => t.Games).FirstOrDefault(t => t.TournamentId == tournamentId) ?? throw new Exception("Kein Turnier gefunden");
    }

    public void AddModerator(Player moderator)
    {
        DBAccess dBAccess = new();
        dBAccess.TournamentModerators.Attach(new TournamentModerator()
        {
            Tournament = this,
            Moderator = moderator
        });
        dBAccess.SaveChanges();
    }

    public void RemoveModerator(Player moderator)
    {
        DBAccess dBAccess = new();
        dBAccess.TournamentModerators.Remove(dBAccess.TournamentModerators.Find(this.TournamentId, moderator.PlayerId) ?? throw new Exception($"Spieler {moderator.Name} nicht als Moderator im Turnier {this.Name} gefunden"));
        dBAccess.SaveChanges();
    }

    public void AddObserver(Player player)
    {
        DBAccess dBAccess = new();
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
        dBAccess.TournamentObservers.Remove(dBAccess.TournamentObservers.Find(this.TournamentId, player.PlayerId) ?? throw new Exception($"Spieler {player.Name} nicht als Beobachter im Turnier {this.Name} gefunden"));
        dBAccess.SaveChanges();
    }

    public void StartMatchmaking()
    {
        foreach (Team team in Teams)
        {
            if (team.Players.Count < 5)
            {
                throw new Exception($"Team {team.Name} hat zu wenig Spieler");
            }
        }
    }
}