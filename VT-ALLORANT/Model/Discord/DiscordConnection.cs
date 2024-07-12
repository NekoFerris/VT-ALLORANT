using Discord;
using Discord.WebSocket;

namespace VT_ALLORANT.Model.Discord;
public class DiscordConnection
{
    private DiscordSocketClient? _client;

    public async Task RunBotAsync()
    {
        DiscordSocketConfig config = new();
        _client = new DiscordSocketClient(config);
        _client.Log += Log;
        _client.SlashCommandExecuted += SlashCommandHandler;
        _client.SelectMenuExecuted += SelectMenuHandler;
        _client.Ready += CreateCommands;
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
            case "rank-for":
                await SelectMenuCommands.RankSelectMenu(component);
                break;
            case "join-tournament":
                await SelectMenuCommands.JoinTournamentSelectMenu(component);
                break;
            case "leave-tournament":
                await SelectMenuCommands.LeaveTournamentSelectMenu(component);
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
                List<SlashCommandProperties> newGuildCommands = AvailabaleGuildCommands(guild);
                IReadOnlyCollection<SocketApplicationCommand> oldGuildCommands = await guild.GetApplicationCommandsAsync();
                Parallel.ForEach(newGuildCommands, async task => await AddGuildCommand(oldGuildCommands, task, guild));
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

    public static List<SlashCommandProperties> AvailabaleGuildCommands(SocketGuild guild)
    {
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
                    .WithName("team")
                    .WithDescription("Team Verwaltung")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithName("create")
                        .WithDescription("Team erstellen")
                        .AddOption("name", ApplicationCommandOptionType.String, "Name des Teams", isRequired: true)
                    ).AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithName("delete")
                        .WithDescription("Team auflösen")
                    ).AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommandGroup)
                        .WithName("change")
                        .WithDescription("Einstellungen ändern")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.SubCommand)
                            .WithName("leader")
                            .WithDescription("Team erstellen")
                            .AddOption("name", ApplicationCommandOptionType.String, "Name des neuen Anführers", isRequired: true)
                        )
                    ).AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommandGroup)
                        .WithName("player")
                        .WithDescription("Spiler hinzufügen oder entfernen")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.SubCommand)
                            .WithName("add")
                            .WithDescription("Spieler hinzufügen")
                            .AddOption("name", ApplicationCommandOptionType.User, "Hinzuzufügender Spieler", isRequired: true)
                        )
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.SubCommand)
                            .WithName("remove")
                            .WithDescription("Spieler entfernen")
                            .AddOption("name", ApplicationCommandOptionType.User, "Zu entferndender Spieler", isRequired: true)
                        )
                    );
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
                    .AddOption("name", ApplicationCommandOptionType.String, "Name des Turniers", isRequired: true)
                    .WithDefaultPermission(false)
                    .WithDefaultMemberPermissions(GuildPermission.ManageEvents);
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("join-tournament")
                    .WithDescription("Tritt einem Turnier bei");
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("leave-tournament")
                    .WithDescription("Verlasse ein Turnier");
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("send-friend-request")
                    .WithDescription("Freundschaftsanfrage senden")
                    .AddOption("name", ApplicationCommandOptionType.User, "Name des Spielers", isRequired: true);
                commands.Add(guildCommand.Build());

                guildCommand = new SlashCommandBuilder()
                    .WithName("recreate-commands")
                    .WithDescription("Alle Bestehenden Commands neu erstellen")
                    .WithDefaultPermission(false)
                    .WithDefaultMemberPermissions(GuildPermission.Administrator);
                commands.Add(guildCommand.Build());
                return commands;
    }
     
    private async Task RecreateCommands(SocketGuild guild)
    {
        IReadOnlyCollection<SocketApplicationCommand> oldGuildCommands = await guild.GetApplicationCommandsAsync();
        IEnumerable<Task> deleteGuildCommandsTasks = oldGuildCommands.Select(command => command.DeleteAsync());
        foreach (Task task in deleteGuildCommandsTasks)
        {
            await task;
        }
        List<SlashCommandProperties> newGuildCommands = AvailabaleGuildCommands(guild);
        foreach (SlashCommandProperties command in newGuildCommands)
        {
            await guild.CreateApplicationCommandAsync(command);
        }
    }

    private static async Task AddGuildCommand(IReadOnlyCollection<SocketApplicationCommand> ExistingSocketApplicationCommands, SlashCommandProperties socketSlashCommand, SocketGuild guild)
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
                case "team":
                    switch(command.Data.Options.First().Name)
                    {
                        case "create":
                            await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.CreateTeam(command));
                            break;
                        case "delete":
                            await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.DeleteTeam(command));
                            break;
                        case "change":
                            switch(command.Data.Options.First().Options.First().Name)
                            {
                                case "leader":
                                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.ChangeLeader(command));
                                    break;
                            }
                            break;
                        case "player":
                            switch(command.Data.Options.First().Options.First().Name)
                            {
                                case "add":
                                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.AddPlayer(command));
                                    break;
                                case "remove":
                                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.RemovePlayer(command));
                                    break;
                            }
                            break;
                    }
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
                case "send-friend-request":
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.SendFriendRequest(command));
                    break;
                case "recreate-commands":
                    SocketGuild guild = _client!.GetGuild(command.GuildId!.Value);
                    await RecreateCommands(guild);
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = "Alle Befehle werden neu erstellt");
                    break;
                default:
                    await command.ModifyOriginalResponseAsync(properties => properties.Content = "Befehl wurde nicht gefunden, bitte bei @NekoFerris melden");
                    return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while executing the command: {ex.Message}");
        }
    }
}