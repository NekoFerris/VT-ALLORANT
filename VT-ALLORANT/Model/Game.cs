namespace VT_ALLORANT.Model;
public class Game
{
    // Class members (fields, properties, methods, etc.) go here
    // Fields
    public int Id { get; set; }  // Unique ID of the game
    public Team Team1{ get; set; } 
    public Team Team2{ get; set; } 

    public Team? Winner { get; set; } = null;
    public Player Moderator;

    public List<Player> Observers = new List<Player>();

    // Constructor
    public Game()
    {
    }

    // Methods
    // Define your methods here
    // Properties
    // Define your properties here

    // Fields
    // Define your fields here
}