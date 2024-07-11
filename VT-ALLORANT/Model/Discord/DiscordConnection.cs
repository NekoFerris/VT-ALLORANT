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
        _client.SelectMenuExecuted += SelectMenuHandler;
        _client.Ready += async () =>
        {
            await CreateCommands();
        };
        Config configFile = new();
        await _client.LoginAsync(TokenType.Bot, configFile.DiscordApiKey);
        await _client.StartAsync();
    }

    private async Task SelectMenuHandler(SocketMessageComponent component)
    {
        await component.DeferAsync();
        string type = component.Data.CustomId.Split(":")[0];
        switch (type)
        {
            case "team":
                await SelectMenuCommands.TeamSelectMenu(component);
                break;
            case "player":
                await SelectMenuCommands.PlayerSelectMenu(component);
                break;
            case "rank-for":
                await SelectMenuCommands.RankSelectMenu(component);
                break;
            default:
                break;
        }
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

                List<SlashCommandProperties> commands = [];

                SlashCommandBuilder guildCommand = new SlashCommandBuilder()
                    .WithName("register")
                    .WithDescription("Spieler Registrieren")
                    .AddOption("name", ApplicationCommandOptionType.String, "Name des Valorant Accounts", isRequired: true)
                    .AddOption("tag", ApplicationCommandOptionType.String, "Tag des Valorant Accounts", isRequired: true)
                    .AddOption("vtname", ApplicationCommandOptionType.String, "VTuber Name", isRequired: true);
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("unregister")
                    .WithDescription("Registrierung aufheben");
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("create-team")
                    .WithDescription("Team erstellen")
                    .AddOption("name", ApplicationCommandOptionType.String, "Name des Teams", isRequired: true);
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("delete-team")
                    .WithDescription("Team auflösen");
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("change-leader")
                    .WithDescription("Anführer wechseln")
                    .AddOption("name", ApplicationCommandOptionType.User, "Name des neuen Anführers", isRequired: true);
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("add-player")
                    .WithDescription("Spieler hinzufügen")
                    .AddOption("name", ApplicationCommandOptionType.User, "Name des Spielers", isRequired: true);
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("remove-player")
                    .WithDescription("Spieler entfernen")
                    .AddOption("name", ApplicationCommandOptionType.User, "Name des Spielers", isRequired: true);
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("match-game")
                    .WithDescription("Erstellt ein Spiel")
                    .AddOption("team1", ApplicationCommandOptionType.String, "Name des ersten Teams", isRequired: true)
                    .AddOption("team2", ApplicationCommandOptionType.String, "Name des zweiten Teams", isRequired: true);
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("update-ranking")
                    .WithDescription("Ändere den Rang eines Spielers")
                    .AddOption("name", ApplicationCommandOptionType.User, "Name des Spielers", isRequired: false);
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("create-tournament")
                    .WithDescription("Tritt einem Turnier bei")
                    .AddOption("name", ApplicationCommandOptionType.String, "Name des Turniers", isRequired: true);
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("join-tournament")
                    .WithDescription("Tritt einem Turnier bei");
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("leave-tournament")
                    .WithDescription("Verlasse ein Turnier");
                commands.Add(guildCommand.Build());

                //guildCommand = new SlashCommandBuilder()
                //    .WithName("send-friend-request")
                //    .WithDescription("Freundschaftsanfrage senden")
                //    .AddOption("name", ApplicationCommandOptionType.User, "Name des Spielers", isRequired: true);
                //tasks.Add(guild.CreateApplicationCommandAsync(guildCommand.Build()));

                Parallel.ForEach(commands, async task => await AddGuildCommand(guildCommands, task, guild));


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

    private async Task AddGuildCommand(IReadOnlyCollection<SocketApplicationCommand> ExistingSocketApplicationCommands, SlashCommandProperties socketSlashCommand, SocketGuild guild)
    {
        if(ExistingSocketApplicationCommands.Any(command => command.Name == socketSlashCommand.Name.Value))
        {
            return;
        }
        else
        {
            await guild.CreateApplicationCommandAsync(socketSlashCommand);
        }
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
                case "match-game":
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.MatchGame(command));
                    break;
                case "update-ranking":
                    await SlashCommands.UpdateRanking(command);
                    break;
                case "create-tournament":
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.CreateTournament(command));
                    break;
                case "leave-tournament":
                    await SlashCommands.LeaveTournament(command);
                    break;
                case "join-tournament":
                    await SlashCommands.JoinTournament(command);
                    break;
                //case "send-friend-request":
                //    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.SendFriendRequest(command));
                //    break;
                default:
                    return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while executing the command: {ex.Message}");
        }
    }
}