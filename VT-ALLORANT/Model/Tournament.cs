namespace VT_ALLORANT.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


public class Tournament
{
    // Properties
    public int TournamentId { get; set; }  // Unique ID of the tournament
    public string Name { get; set; } = "unset"; // Default value "unset
    public ICollection<Team> Teams { get; set; } = []; // Default value new List<Team>()
    public ICollection<Player> Moderators { get; set; } = []; // Default value new List<Team>()
    public ICollection<Player> Observers { get; set; } = []; // Default value new List<Team>()
    public ICollection<Game> Games { get; set; } = []; // Default value new List<Game>()

    // Constructor
    public Tournament()
    {
        TournamentId = 1;
    }

    // Methods
    public void AddTeam(Team team)
    {
        Teams.Add(team);
    }

    public void RemoveTeam(Team team)
    {
        Teams.Remove(team);
    }
}