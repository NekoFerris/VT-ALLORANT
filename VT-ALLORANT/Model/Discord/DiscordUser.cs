namespace VT_ALLORANT.Model.Discord;

public class DiscordUser
{
    // Properties
    public string Username { get; set; }
    public string UserId { get; set; }

    // Constructor
    public DiscordUser(string userId)
    {
        UserId = userId;
    }

    // Methods
    public void SendMessage(string message)
    {
        // Code to send a message to the user
    }

    public void AddFriend(DiscordUser friend)
    {
        // Code to add a friend to the user's friend list
    }

    // Other methods and properties can be added as needed
}