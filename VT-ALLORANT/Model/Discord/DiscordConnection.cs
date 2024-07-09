using System.Net.WebSockets;
using System.Threading.Tasks;
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
        _client.SlashCommandExecuted += SlashCommandHandler;
        _client.Ready += async () =>
        {
            await CreateCommands();
        };
        Config configFile = new();
        await _client.LoginAsync(TokenType.Bot, configFile.DiscordApiKey);
        await _client.StartAsync();
    }

    private Task Log(LogMessage arg)
    {
        Console.WriteLine(arg);
        return Task.CompletedTask;
    }

    public async Task CreateCommands()
    {

        await Task.Run(async () =>
        {
            try
            {
                Config configFile = new();
                SocketGuild guild = _client!.GetGuild(configFile.GuildId);

                IReadOnlyCollection<SocketApplicationCommand> guildCommands = await guild.GetApplicationCommandsAsync();
                IReadOnlyCollection<SocketApplicationCommand> globalCommands = await _client.GetGlobalApplicationCommandsAsync();

                IEnumerable<Task> deleteGuildCommandsTasks = guildCommands.Select(command => command.DeleteAsync());
                IEnumerable<Task> deleteGlobalCommandsTasks = globalCommands.Select(command => command.DeleteAsync());

                await Task.WhenAll(deleteGuildCommandsTasks);
                await Task.WhenAll(deleteGlobalCommandsTasks);

                List<Task> tasks = [];

                SlashCommandBuilder guildCommand = new SlashCommandBuilder()
                    .WithName("register")
                    .WithDescription("Spieler Registrieren")
                    .AddOption("name", ApplicationCommandOptionType.String, "Name des Valorant Accounts", isRequired: true)
                    .AddOption("tag", ApplicationCommandOptionType.String, "Tag des Valorant Accounts", isRequired: true)
                    .AddOption("vtname", ApplicationCommandOptionType.String, "VTuber Name", isRequired: true);
                tasks.Add(guild.CreateApplicationCommandAsync(guildCommand.Build()));

                guildCommand = new SlashCommandBuilder()
                    .WithName("unregister")
                    .WithDescription("Registrierung aufheben");
                tasks.Add(guild.CreateApplicationCommandAsync(guildCommand.Build()));

                guildCommand = new SlashCommandBuilder()
                    .WithName("create-team")
                    .WithDescription("Team erstellen")
                    .AddOption("name", ApplicationCommandOptionType.String, "Name des Teams", isRequired: true);
                tasks.Add(guild.CreateApplicationCommandAsync(guildCommand.Build()));

                guildCommand = new SlashCommandBuilder()
                    .WithName("delete-team")
                    .WithDescription("Team auflösen");
                tasks.Add(guild.CreateApplicationCommandAsync(guildCommand.Build()));

                guildCommand = new SlashCommandBuilder()
                    .WithName("change-leader")
                    .WithDescription("Anführer wechseln")
                    .AddOption("name", ApplicationCommandOptionType.User, "Name des neuen Anführers", isRequired: true);
                tasks.Add(guild.CreateApplicationCommandAsync(guildCommand.Build()));

                guildCommand = new SlashCommandBuilder()
                    .WithName("add-player")
                    .WithDescription("Spieler hinzufügen")
                    .AddOption("name", ApplicationCommandOptionType.User, "Name des Spielers", isRequired: true);
                tasks.Add(guild.CreateApplicationCommandAsync(guildCommand.Build()));

                guildCommand = new SlashCommandBuilder()
                    .WithName("remove-player")
                    .WithDescription("Spieler entfernen")
                    .AddOption("name", ApplicationCommandOptionType.User, "Name des Spielers", isRequired: true);
                tasks.Add(guild.CreateApplicationCommandAsync(guildCommand.Build()));

                guildCommand = new SlashCommandBuilder()
                    .WithName("start-matchmaking")
                    .WithDescription("Matchmaking starten");
                tasks.Add(guild.CreateApplicationCommandAsync(guildCommand.Build()));

                guildCommand = new SlashCommandBuilder()
                    .WithName("send-friend-request")
                    .WithDescription("Freundschaftsanfrage senden")
                    .AddOption("name", ApplicationCommandOptionType.User, "Name des Spielers", isRequired: true);
                tasks.Add(guild.CreateApplicationCommandAsync(guildCommand.Build()));

                Parallel.ForEach(tasks, async task => await task);


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
        });
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        try
        {
            await command.DeferAsync();
            switch (command.Data.Name)
            {
                case "register":
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.Register(command));
                    break;
                case "unregister":
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.Unregister(command));
                    break;
                case "create-team":
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.CreateTeam(command));
                    break;
                case "delete-team":
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.DeleteTeam(command));
                    break;
                case "change-leader":
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.ChangeLeader(command));
                    break;
                case "add-player":
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.AddPlayer(command));
                    break;
                case "remove-player":
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.RemovePlayer(command));
                    break;
                case "start-matchmaking":
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.StartMatchmaking(command));
                    break;
                case "send-friend-request":
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.SendFriendRequest(command));
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