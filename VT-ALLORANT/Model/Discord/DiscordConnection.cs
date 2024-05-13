using Discord;
using Discord.WebSocket;

namespace VT_ALLORANT.Model.Discord;
public class DiscordConnection
{
    private DiscordSocketClient? _client;

    public async Task RunBotAsync()
    {
        DiscordSocketConfig config = new DiscordSocketConfig()
        {
            // Other config options can be presented here.
            GatewayIntents = GatewayIntents.AllUnprivileged
        };
        _client = new DiscordSocketClient(config);
        _client.Log += Log;
        _client.Ready += Client_Ready;
        _client.SlashCommandExecuted += SlashCommandHandler;
        await _client.LoginAsync(TokenType.Bot, "MTIzODQ1OTkwMDc2MjI2MzYwMw.GyMKKB.LvrgEdIf_QUS53Zo1hhS6b9sjnLb9Y3dd_ZoYg");
        await _client.StartAsync();
    }

    private Task Log(LogMessage arg)
    {
        Console.WriteLine(arg);
        return Task.CompletedTask;
    }

    public async Task Client_Ready()
    {
        try
        {
            var guild = _client.GetGuild(1238457984288296982);

            var guildCommands = await guild.GetApplicationCommandsAsync();
            var globalCommands = await _client.GetGlobalApplicationCommandsAsync();

            var deleteGuildCommandsTasks = guildCommands.Select(command => command.DeleteAsync());
            var deleteGlobalCommandsTasks = globalCommands.Select(command => command.DeleteAsync());

            await Task.WhenAll(deleteGuildCommandsTasks);
            await Task.WhenAll(deleteGlobalCommandsTasks);

            var guildCommand = new SlashCommandBuilder()
                .WithName("register")
                .WithDescription("Spieler Registrieren")
                .AddOption("name", ApplicationCommandOptionType.String, "Name des Valorant Accounts", isRequired: true)
                .AddOption("tag", ApplicationCommandOptionType.String, "Tag des Valorant Accounts", isRequired: true)
                .AddOption("vtname", ApplicationCommandOptionType.String, "VTuber Name", isRequired: true);

            await guild.CreateApplicationCommandAsync(guildCommand.Build());
        }
        catch (AggregateException ae)
        {
            foreach (var e in ae.InnerExceptions)
            {
                Console.WriteLine(e.Message);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        Console.WriteLine($"Event received at {DateTime.UtcNow}");
        try
        {
            Console.WriteLine($"Received command: {command.Data.Name}");
            await command.DeferAsync();
            switch (command.Data.Name)
            {
                case "register":
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.Register(command));
                    break;
                default:
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = "Command not found");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while executing the command: {ex.Message}");
        }
    }
}