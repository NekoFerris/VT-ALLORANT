namespace VT_ALLORANT.Model;
public class Team
{
    // Properties
    public int Id { get; set; }  // Unique ID of the team
    public string Name { get; set; } = "unset"; // Default value "unset
    public List<Player> Players { get; set; } = []; // Default value new List<Player>()

    // Constructor
    public Team()
    {
    }
    // Methods
    // Add a player to the team
    public void AddPlayer(Player player)
    {
        Players.Add(player);
    }

    // Remove a player from the team
    public void RemovePlayer(Player player)
    {
        Players.Remove(player);
    }
    // Methods
    // Add your custom methods here
}