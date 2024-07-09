using System.Numerics;
using Newtonsoft.Json;

public class Config
{
    public string ValorantApiKey { get; set; } = "VALORANT-1234";
    public string DiscordApiKey { get; set; } = "DISCORD-1234";
    public ulong GuildId { get; set; } = 123456789012345678;
    public Config()
    {
        string DefaultPath = "config.json";
        if (!File.Exists(DefaultPath))
        {
            File.WriteAllText(DefaultPath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
        string json = File.ReadAllText(DefaultPath);
        JsonConvert.PopulateObject(json, this);
    }
}