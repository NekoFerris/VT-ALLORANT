using VT_ALLORANT.Model.Valorant;
using VT_ALLORANT.Model.Discord;

namespace VT_ALLORANT.Model;
public class Player
{
    // Properties
    public int Id { get; set; }  // Unique ID of the player
    public string Name { get; set; } = "unset"; // Default value "unset
    public DiscordUser DiscordUser { get; set; }  // Discord User
    public ValorantUser ValorantUser { get; set; }  // Valorant User

    // Constructor
    public Player()
    {

    }

}