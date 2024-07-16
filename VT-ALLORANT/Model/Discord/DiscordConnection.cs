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
        DiscordRole.SetupRoles();
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
            );

        commands.Add(guildCommand.Build());

        guildCommand = new SlashCommandBuilder()
            .WithName("tournament")
            .WithDescription("Turnier Verwaltung")
            .AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("create")
                .WithDescription("Turnier erstellen")
                .AddOption("name", ApplicationCommandOptionType.String, "Name des Teams", isRequired: true)
            ).AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("delete")
                .WithDescription("Turnier löschen")
            )
            .AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("join")
                .WithDescription("Turnier beitreten")
            )
            .AddOption(new SlashCommandOptionBuilder()
                .WithType(ApplicationCommandOptionType.SubCommand)
                .WithName("leave")
                .WithDescription("Turnier verlassen")
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
            case "register":
                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.Register(command, _client!));
                break;
            case "unregister":
                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.Unregister(command, _client!));
                break;
            case "team":
                switch (command.Data.Options.First().Name)
                {
                    case "create":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.CreateTeam(command));
                        break;
                    case "delete":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.DeleteTeam(command));
                        break;
                    case "set":
                        switch (command.Data.Options.First().Options.First().Name)
                        {
                            case "leader":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.ChangeLeader(command, _client!));
                                break;
                        }
                        break;
                    case "player":
                        switch (command.Data.Options.First().Options.First().Name)
                        {
                            case "add":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.AddPlayer(command, _client!));
                                break;
                            case "remove":
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.RemovePlayer(command, _client!));
                                break;
                        }
                        break;
                }
                break;
            case "match-game":
                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.MatchGame(command));
                break;
            case "update-ranking":
                await SlashCommands.UpdateRanking(command, _client!);
                break;
            case "tournament":
                switch (command.Data.Options.First().Name)
                {
                    case "create":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.CreateTournament(command));
                        break;
                    case "delete":
                        await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.DeleteTournament(command));
                        break;
                    case "join":
                        await SlashCommands.JoinTournament(command);
                        break;
                    case "leave":
                        await SlashCommands.LeaveTournament(command);
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
                                await command.ModifyOriginalResponseAsync(properties => properties.Content = SlashCommands.SetRole(command, _client!));
                                break;
                        }
                        break;
                }
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