namespace VT_ALLORANT.Model;
public class Tournament
{
    // Properties
    public int TournamentId { get; set; }  // Unique ID of the tournament
    public string Name { get; set; } = "unset"; // Default value "unset
    public List<Team> Teams { get; set; } = []; // Default value new List<Team>()
    public List<Player> Moderators { get; set; } = []; // Default value new List<Team>()
    public List<Player> Observers { get; set; } = []; // Default value new List<Team>()
    public List<Game> Games { get; set; } = []; // Default value new List<Game>()
    public List<Player> Players { get; set; } = []; // Default value new List<Player>()

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

    // Other methods and properties can be added as needed
}