using VT_ALLORANT.Model.Discord;
using VT_ALLORANT.Model.Valorant;

namespace VT_ALLORANT.Model;

public class BotBase
{
    // Properties
    public DiscordConnection DiscordConnection { get; set; }

    // Constructor
    public BotBase()
    {
        DiscordConnection = new DiscordConnection();
    }

    // Methods
    public async Task Start()
    {
        await DiscordConnection.RunBotAsync();
    }

    public void Stop()
    {
        // Code to stop the bot
    }

    // Other methods and properties can be added as needed
}