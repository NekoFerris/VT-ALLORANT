using System.Diagnostics;
using Discord;
using Discord.WebSocket;
using VT_ALLORANT.Controller;
using VT_ALLORANT.Model.Discord;

namespace VT_ALLORANT.Model.Discord;
public class DiscordConnection
{
    private DiscordSocketClient? _client;

    public async Task RunBotAsync()
    {
        DiscordSocketConfig config = new()
        {
            GatewayIntents = GatewayIntents.All
        };
        _client = new DiscordSocketClient(config)!;
        _client.Log += Log;
        _client.SlashCommandExecuted += SlashCommandHandler;
        _client.SelectMenuExecuted += SelectMenuHandler;
        _client.Ready += CreateCommands;
        Config configFile = new();
        await _client.LoginAsync(TokenType.Bot, configFile.DiscordApiKey);
        await _client.StartAsync();
    }

    private Task Log(LogMessage arg)
    {
        Debug.WriteLine(arg);
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

    private static async Task RecreateCommands(SocketGuild guild)
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
        if (ExistingSocketApplicationCommands.Any(command => command.Name == socketSlashCommand.Name.Value))
        {
            return;
        }
        else
        {
            await guild.CreateApplicationCommandAsync(socketSlashCommand);
        }
    }

    public static List<SlashCommandProperties> AvailabaleGuildCommands(SocketGuild guild)
    {
        List<SlashCommandProperties> commands = [];
        SlashCommandBuilder guildCommand = new SlashCommandBuilder()
            .WithName("player")
            .WithDescription("Spieler Verwaltung")
            .AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("register")
                .WithDescription("Spieler Registrieren")
                .AddOption("name", ApplicationCommandOptionType.String, "Name des Valorant Accounts", isRequired: true)
                .AddOption("tag", ApplicationCommandOptionType.String, "Tag des Valorant Accounts", isRequired: true)
                .AddOption("vtname", ApplicationCommandOptionType.String, "VTuber Name", isRequired: true)
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("unregister")
                .WithDescription("Registrierung aufheben")
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("list")
                .WithDescription("Spieler auflisten")
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommandGroup)
                .WithName("set")
                .WithDescription("Einstellungen ändern")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("name")
                    .WithDescription("Spieler hinzufügen")
                    .AddOption("name", ApplicationCommandOptionType.User, "Neuer Name", isRequired: true)
                )
                .AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("rank")
                    .WithDescription("Rang ändern")
                )
            );
        commands.Add(guildCommand.Build());

        guildCommand = new SlashCommandBuilder()
            .WithName("game")
            .WithDescription("Spiel Verwaltung")
            .AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("create")
                .WithDescription("Spiel erstellen")
                .AddOption("team1", ApplicationCommandOptionType.Integer, "Id des ersten Teams", isRequired: true)
                .AddOption("team2", ApplicationCommandOptionType.Integer, "Id des zweiten Teams", isRequired: true)
                .AddOption("tournament", ApplicationCommandOptionType.Integer, "Id des Tuniers", isRequired: true)
                .AddOption("moderator", ApplicationCommandOptionType.User, "Moderator", isRequired: true)
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("delete")
                .WithDescription("Spiel löschen")
                .AddOption("id", ApplicationCommandOptionType.Integer, "Id des Spiels", isRequired: true)
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("list")
                .WithDescription("Liste aller Spiele")
                .AddOption("tournament", ApplicationCommandOptionType.Integer, "Id des Tuniers", isRequired: true)
            );
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
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("list")
                .WithDescription("Liste aller Teams")
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommandGroup)
                .WithName("set")
                .WithDescription("Einstellungen ändern")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("leader")
                    .WithDescription("Team erstellen")
                    .AddOption("name", ApplicationCommandOptionType.String, "Name des neuen Anführers", isRequired: true)
                ).AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("rank")
                    .WithDescription("Rang auswählen")
                )
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("show")
                .WithDescription("Team anzeigen")
                .AddOption("id", ApplicationCommandOptionType.Integer, "Id des Teams", isRequired: true)
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

        List<ApplicationCommandOptionChoiceProperties> roleChoices = [];
        foreach (long roleId in Enum.GetValues(typeof(RoleType)))
        {
            roleChoices.Add(new ApplicationCommandOptionChoiceProperties() { Name = Enum.GetName(typeof(RoleType), roleId)!.ToString(), Value = roleId.ToString() });
        }
        guildCommand = new SlashCommandBuilder()
            .WithName("setup")
            .WithDescription("Bot Verwaltung")
            .WithDefaultPermission(false)
            .WithDefaultMemberPermissions(GuildPermission.Administrator)
            .AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("player-ranks")
                .WithDescription("Rolle anzeigen")
            )
            .AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommandGroup)
                .WithName("set")
                .WithDescription("Einstellungen ändern")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("role")
                    .WithDescription("Rolle einstellen")
                    .AddOption("role", ApplicationCommandOptionType.String, "Rolle", isRequired: true, choices: [.. roleChoices])
                    .AddOption("discord-role", ApplicationCommandOptionType.Role, "Discord Rolle die zugewiesen werden soll", isRequired: true)
                )
                .AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("rank")
                    .WithDescription("Rang einstellen")
                    .AddOption("role", ApplicationCommandOptionType.Integer, "Rang", isRequired: true)
                    .AddOption("score", ApplicationCommandOptionType.String, "Punktzahl des Ranges", isRequired: true)
                )
            );

        commands.Add(guildCommand.Build());

        guildCommand = new SlashCommandBuilder()
            .WithName("tournament")
            .WithDescription("Turnier Verwaltung")
            .AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("create")
                .WithDescription("Turnier erstellen")
                .AddOption("name", ApplicationCommandOptionType.String, "Name des Tuniers", isRequired: true)
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("delete")
                .WithDescription("Turnier löschen")
                .AddOption("id", ApplicationCommandOptionType.Integer, "Id des Tuniers", isRequired: true)
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("join")
                .WithDescription("Turnier beitreten")
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("leave")
                .WithDescription("Turnier verlassen")
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("list")
                .WithDescription("Liste aller Turniere")
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("show")
                .WithDescription("Turnier anzeigen")
                .AddOption("id", ApplicationCommandOptionType.Integer, "Id des Turniers", isRequired: true)
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommandGroup)
                .WithName("set")
                .WithDescription("Einstellungen ändern")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("stage")
                    .WithDescription("Aktuelle Stage eines Tunieres ändern")
                    .AddOption("id", ApplicationCommandOptionType.Integer, "Id des Turniers", isRequired: true)
                    .AddOption("stage", ApplicationCommandOptionType.Integer, "Neue Stage", isRequired: true)
                ).AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("max-teams")
                    .WithDescription("Maximale Anzahl an Teams ändern")
                    .AddOption("id", ApplicationCommandOptionType.Integer, "Id des Turniers", isRequired: true)
                    .AddOption("max-teams", ApplicationCommandOptionType.Integer, "Maximale Anzahl an Teams", isRequired: true)
                ).AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("max-player-rank")
                    .WithDescription("Maximalen Spieler Rang ändern")
                    .AddOption("id", ApplicationCommandOptionType.Integer, "Id des Turniers", isRequired: true)
                    .AddOption("max-player-rank", ApplicationCommandOptionType.Integer, "Maximaler Spieler Rang", isRequired: true)
                ).AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("min-player-rank")
                    .WithDescription("Minimalen Spieler Rang ändern")
                    .AddOption("id", ApplicationCommandOptionType.Integer, "Id des Turniers", isRequired: true)
                    .AddOption("min-player-rank", ApplicationCommandOptionType.Integer, "Minimaler Spieler Rang", isRequired: true)
                ).AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("max-team-rank")
                    .WithDescription("Maximalen Team Rang ändern")
                    .AddOption("id", ApplicationCommandOptionType.Integer, "Id des Turniers", isRequired: true)
                    .AddOption("max-team-rank", ApplicationCommandOptionType.String, "Maximaler Teamrang", isRequired: true)
                )
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommandGroup)
                .WithName("observer")
                .WithDescription("Beobachter hinzufügen oder entfernen")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("add")
                    .WithDescription("Beobachter hinzufügen")
                    .AddOption("id", ApplicationCommandOptionType.Integer, "Id des Turniers", isRequired: true)
                    .AddOption("name", ApplicationCommandOptionType.User, "Hinzuzufügender Beobachter", isRequired: true)
                ).AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("remove")
                    .WithDescription("Beobachter entfernen")
                    .AddOption("id", ApplicationCommandOptionType.Integer, "Id des Turniers", isRequired: true)
                    .AddOption("name", ApplicationCommandOptionType.User, "Zu entferndender Beobachter", isRequired: true)
                )
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommandGroup)
                .WithName("moderator")
                .WithDescription("Moderator hinzufügen oder entfernen")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("add")
                    .WithDescription("Moderator hinzufügen")
                    .AddOption("id", ApplicationCommandOptionType.Integer, "Id des Turniers", isRequired: true)
                    .AddOption("name", ApplicationCommandOptionType.User, "Hinzuzufügender Moderator", isRequired: true)
                ).AddOption(new SlashCommandOptionBuilder()
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .WithName("remove")
                    .WithDescription("Moderator entfernen")
                    .AddOption("id", ApplicationCommandOptionType.Integer, "Id des Turniers", isRequired: true)
                    .AddOption("name", ApplicationCommandOptionType.User, "Zu entferndender Moderator", isRequired: true)
                )
            );
        commands.Add(guildCommand.Build());

        guildCommand = new SlashCommandBuilder()
            .WithName("recreate-commands")
            .WithDescription("Alle Bestehenden Commands neu erstellen")
            .WithDefaultPermission(false)
            .WithDefaultMemberPermissions(GuildPermission.Administrator);
        commands.Add(guildCommand.Build());
        return commands;
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        await command.DeferAsync();
        switch (command.Data.Name)
        {
            case "team":
                switch (command.Data.Options.First().Name)
                {
                    case "create":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.CreateTeam(command));
                        break;
                    case "delete":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.DeleteTeam(command));
                        break;
                    case "list":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.ListTeams(command, _client!));
                        break;
                    case "show":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.ShowTeam(command, _client!));
                        break;
                    case "set":
                        switch (command.Data.Options.First().Options.First().Name)
                        {
                            case "leader":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.ChangeLeaderFromTeam(command, _client!));
                                break;
                        }
                        break;
                    case "player":
                        switch (command.Data.Options.First().Options.First().Name)
                        {
                            case "add":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.AddPlayerToTeam(command, _client!));
                                break;
                            case "remove":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.RemovePlayerFromTeam(command, _client!));
                                break;
                        }
                        break;
                }
                break;
            case "player":
                switch (command.Data.Options.First().Name)
                {
                    case "register":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.RegisterPlayer(command, _client!));
                        break;
                    case "unregister":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.UnregisterPlayer(command, _client!));
                        break;
                    case "list":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.ListPlayers(command, _client!));
                        break;
                    case "set":
                        switch (command.Data.Options.First().Options.First().Name)
                        {
                            case "name":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.SetPlayerName(command, _client!));
                                break;
                            case "rank":
                                await SlashCommands.SetPlayerRank(command, _client!);
                                break;
                        }
                        break;
                }
                break;
            case "tournament":
                switch (command.Data.Options.First().Name)
                {
                    case "create":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.CreateTournament(command, _client!));
                        break;
                    case "delete":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.DeleteTournament(command, _client!));
                        break;
                    case "join":
                        await SlashCommands.TeamJoinTournament(command);
                        break;
                    case "leave":
                        await SlashCommands.TeamLeaveTournament(command);
                        break;
                    case "list":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.ListTournaments(command, _client!));
                        break;
                    case "show":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.ShowTournament(command, _client!));
                        break;
                    case "set":
                        switch (command.Data.Options.First().Options.First().Name)
                        {
                            case "stage":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.SetTournamentStage(command, _client!));
                                break;
                            case "max-teams":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.SetTournamentMaxTeams(command, _client!));
                                break;
                            case "max-player-rank":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.SetTournamentMaxPlayerRank(command, _client!));
                                break;
                            case "min-player-rank":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.SetTournamentMinPlayerRank(command, _client!));
                                break;
                            case "max-team-rank":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.SetTournamentMaxTeamRank(command, _client!));
                                break;
                        }
                        break;
                    case "moderator":
                        switch (command.Data.Options.First().Options.First().Name)
                        {
                            case "add":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.AddModeratorToTournament(command, _client!));
                                break;
                            case "remove":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.RemoveModeratorFromTournament(command, _client!));
                                break;
                        }
                        break;
                    case "observer":
                        switch (command.Data.Options.First().Options.First().Name)
                        {
                            case "add":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.AddObserverToTournament(command, _client!));
                                break;
                            case "remove":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.RemoveObserverFromTournament(command, _client!));
                                break;
                        }
                        break;
                }
                break;
            case "setup":
                switch (command.Data.Options.First().Name)
                {
                    case "set":
                        switch (command.Data.Options.First().Options.First().Name)
                        {
                            case "role":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.SetRoleToDiscordRole(command, _client!));
                                break;
                            case "rank":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.SetRankScore(command, _client!));
                                break;
                        }
                        break;
                    case "player-ranks":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.ListRanks(command));
                        break;
                }
                break;
            case "recreate-commands":
                SocketGuild guild = _client!.GetGuild(command.GuildId!.Value);
                await RecreateCommands(guild);
                await command.ModifyOriginalResponseAsync(properties => properties.Content = "Alle Befehle werden neu erstellt");
                break;
            case "game":
                switch (command.Data.Options.First().Name)
                {
                    case "create":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.CreateGame(command, _client!));
                        break;
                    case "delete":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.DeleteGame(command, _client!));
                        break;
                    case "list":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.ListGames(command));
                        break;
                }
                break;
            default:
                await command.ModifyOriginalResponseAsync(properties => properties.Content = "Befehl wurde nicht gefunden, bitte bei @NekoFerris melden");
                return;
        }
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
}