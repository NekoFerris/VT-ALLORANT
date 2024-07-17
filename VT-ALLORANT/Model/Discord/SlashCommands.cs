using Discord;
using Discord.WebSocket;
using VT_ALLORANT.Controller;
using VT_ALLORANT.Model.Valorant;

namespace VT_ALLORANT.Model.Discord
{
    public static class SlashCommands
    {
        public static string Register(SocketSlashCommand command, DiscordSocketClient client)
        {
            List<SocketSlashCommandDataOption> options = [.. command.Data.Options.First().Options];
            ValorantUser valorantUser;
            string valoname = "";
            string tag = "";
            try
            {
                valoname = options[0]?.Value?.ToString()?.Trim() ?? throw new Exception("Kein Name angegeben");
                tag = options[1]?.Value?.ToString()?.Trim() ?? throw new Exception("Kein Tag angegeben");
                valorantUser = new()
                {
                    PUUID = ValorantConnection.GetUserUUIDByNameAndTag(valoname, tag),
                };
            }
            catch
            {
                return $"Riot Account {valoname}#{tag} nicht gefunden. Wenn du glaubst, dass es ein Fehler ist, kontaktiere bitte einen Admin";
            }
            DiscordUser discordUser = new()
            {
                DiscordId = command.User.Id,
            };
            string name = options[2]?.Value?.ToString()?.Trim() ?? throw new Exception("Kein Name angegeben");
            Player player = new(name.Trim(), discordUser, valorantUser);
            player.Insert();
            SocketGuildUser user = client.GetGuild(command.GuildId!.Value).GetUser(command.User.Id);
            //user.ModifyAsync(properties =>
            //{
            //    properties.Nickname = name;
            //}).Wait();
            user.AddRoleAsync(DiscordRole.GetDiscordRoleIdByType(RoleType.Player)).Wait();
            return $"Regestrierung für VTuber {name.Trim()} mit dem Valorant Account {valoname}#{tag} erfolgreich abgeschlossen";
        }

        public static string Unregister(SocketSlashCommand command, DiscordSocketClient client)
        {
            List<SocketSlashCommandDataOption> options = [.. command.Data.Options];
            Player player;
            try
            {
                player = Player.Load(player => player.DiscordUser.DiscordId == command.User.Id) ?? throw new Exception("Du bist nicht registriert");
                player.Delete();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            SocketGuildUser user = client.GetGuild(command.GuildId!.Value).GetUser(command.User.Id);
            //user.ModifyAsync(properties =>
            //{
            //    properties.Nickname = null;
            //}).Wait();
            user.RemoveRoleAsync(DiscordRole.GetDiscordRoleIdByType(RoleType.Player)).Wait();
            return $"Regestrierung für VTuber {player.Name} erfolgreich gelöscht";
        }

        public static string CreateTeam(SocketSlashCommand command)
        {
            string teamName = command.Data.Options.First().Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Kein Teamname angegeben");
            Team.Create(teamName, Player.Load(player => player.DiscordUser.DiscordId == command.User.Id) ?? throw new Exception("Du bist nicht registriert"));
            return $"Team {teamName} erstellt";
        }

        public static string DeleteTeam(SocketSlashCommand command)
        {
            Team team;
            try
            {
                team = Team.Load(team => team.Leader.PlayerId == Player.Load(player => player.DiscordUser.DiscordId == command.User.Id)!.PlayerId) ?? throw new Exception("Du bist nicht der Anführer eines Teams");
                team.Delete();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Team {team.Name} gelöscht";
        }

        public static string AddPlayer(SocketSlashCommand command, DiscordSocketClient client)
        {
            Team team;
            Player playerToAdd;
            try
            {
                team = Team.Load(team => team.Leader.PlayerId == Player.Load(player => player.DiscordUser.DiscordId == command.User.Id)!.PlayerId) ?? throw new Exception("Du bist nicht der Anführer eines Teams");
                if (team.Players.Count >= team.MaxPlayers)
                {
                    throw new Exception($"Das Team {team.Name} ist bereits voll");
                }
                playerToAdd = Player.Load(player => player.DiscordUser.DiscordId == GetUserId(client, command.Data.Options.First().Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"), command.GuildId!.Value)) ?? throw new Exception("Spieler nicht regestriert");
                if (team.Players.Any(p => p.PlayerId == playerToAdd.PlayerId))
                {
                    throw new Exception($"{playerToAdd.Name} ist bereits im Team {team.Name}");
                }
                team.AddPlayer(playerToAdd);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"{playerToAdd.Name} wurde zu Team {team.Name} hinzugefügt";
        }

        public static string RemovePlayer(SocketSlashCommand command, DiscordSocketClient client)
        {
            Team team;
            Player playerToRemove;
            try
            {
                team = Team.Load(team => team.Leader.PlayerId == Player.Load(player => player.DiscordUser.DiscordId == command.User.Id)!.PlayerId) ?? throw new Exception("Du bist nicht der Anführer eines Teams");
                playerToRemove = Player.Load(player => player.DiscordUser.DiscordId == GetUserId(client, command.Data.Options.First().Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"), command.GuildId!.Value))!;
                team.RemovePlayer(playerToRemove);
                if (!team.Players.Any(p => p.PlayerId == playerToRemove.PlayerId))
                {
                    throw new Exception($"{playerToRemove.Name} ist nicht im Team {team.Name}");
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"{playerToRemove.Name} wurde vom Team {team.Name} entfernt";
        }

        public static string ChangeLeader(SocketSlashCommand command, DiscordSocketClient client)
        {
            Team team;
            Player newLeader;
            try
            {
                team = Team.Load(team => team.Leader.PlayerId == Player.Load(player => player.DiscordUser.DiscordId == command.User.Id)!.PlayerId) ?? throw new Exception("Du bist nicht der Anführer eines Teams");
                newLeader = Player.Load(player => player.DiscordUser.DiscordId == GetUserId(client, command.Data.Options.First().Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"), command.GuildId!.Value))!;
                if (newLeader.PlayerId == team.Leader.PlayerId)
                {
                    throw new Exception("Du bist bereits der Anführer dieses Teams");
                }
                if (!team.Players.Any(p => p.PlayerId == newLeader.PlayerId))
                {
                    throw new Exception($"{newLeader.Name} ist nicht im Team {team.Name}");
                }
                team.SetLeader(newLeader);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"{newLeader.Name} ist jetzt der Anführer vom Team {team.Name}";
        }

        internal static string MatchGame(SocketSlashCommand command)
        {
            Team team1;
            Team team2;
            try
            {
                team1 = Team.LoadTeam(command.Data.Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Team 1 nicht angegeben"))!;
                team2 = Team.LoadTeam(command.Data.Options.ToList()[1].Value.ToString()?.Trim() ?? throw new Exception("Team 2 nicht angegeben"))!;
                if (team1.TeamId == team2.TeamId)
                {
                    throw new Exception("Zwei mal das selbe Team ausgewählt");
                }
                Game game = new(team1, team2, Player.Load(player => player.DiscordUser.DiscordId == command.User.Id)!, [], 0);
                game.Insert();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Spiel zwischen {team1.Name} und {team2.Name} erstellt";
        }

        internal static async Task SetPlayerRank(SocketSlashCommand command, DiscordSocketClient client)
        {
            Player player;
            try
            {
                if (command.Data.Options.Count == 0)
                {
                    player = Player.Load(player => player.DiscordUser.DiscordId == command.User.Id)!;
                }
                else
                {
                    player = Player.Load(player => player.DiscordUser.DiscordId == GetUserId(client, command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"), command.GuildId!.Value))!;
                }
                MessageComponent rankList = new ComponentBuilder()
                .WithSelectMenu(new SelectMenuBuilder()
                    .WithPlaceholder("Rang auswählen")
                    .WithCustomId($"rank-for:{player.PlayerId}")
                    .AddOption("Eisen 1", "1")
                    .AddOption("Eisen 2", "2")
                    .AddOption("Eisen 3", "3")
                    .AddOption("Bronze 1", "4")
                    .AddOption("Bronze 2", "5")
                    .AddOption("Bronze 3", "6")
                    .AddOption("Silber 1", "7")
                    .AddOption("Silber 2", "8")
                    .AddOption("Silber 3", "9")
                    .AddOption("Gold 1", "10")
                    .AddOption("Gold 2", "11")
                    .AddOption("Gold 3", "12")
                    .AddOption("Platin 1", "13")
                    .AddOption("Platin 2", "14")
                    .AddOption("Platin 3", "15")
                    .AddOption("Diamant 1", "16")
                    .AddOption("Diamant 2", "17")
                    .AddOption("Diamant 3", "18")
                    .AddOption("Aufgestiegen 1", "19")
                    .AddOption("Aufgestiegen 2", "20")
                    .AddOption("Aufgestiegen 3", "21")
                    .AddOption("Unsterblich 1", "22")
                    .AddOption("Unsterblich 2", "23")
                    .AddOption("Unsterblich 3", "24")
                    .AddOption("Radiant", "25"), 0)
                    .Build();
                await command.FollowupAsync("Wähle den neuen Rang aus", components: rankList);
            }
            catch (Exception e)
            {
                if (command.HasResponded == false)
                    await command.RespondAsync(e.Message);
                else
                    await command.FollowupAsync(e.Message);
            }
        }

        internal static async Task JoinTournament(SocketSlashCommand command)
        {
            DBAccess dBAccess = new();
            try
            {
                Team joiningTeam = Team.GetAll().FirstOrDefault(t => t.Leader.DiscordUser.DiscordId == command.User.Id) ?? throw new Exception("Du bist in kein Anführer eines Teams");
                List<Tournament> tournaments = [.. Tournament.GetAll().Where(t => t.OpenForRegistration)];
                if (tournaments.Count == 0)
                {
                    await command.FollowupAsync("Es gibt keine offenen Turniere");
                }
                SelectMenuBuilder tournamentListSelectMenuBuilder = new SelectMenuBuilder()
                    .WithPlaceholder("Turnier auswählen")
                    .WithCustomId($"join-tournament:{joiningTeam.TeamId}");
                foreach (Tournament tournament in tournaments)
                {
                    tournamentListSelectMenuBuilder.AddOption(tournament.Name, tournament.TournamentId.ToString());
                }
                MessageComponent tournamentList = new ComponentBuilder()
                .WithSelectMenu(tournamentListSelectMenuBuilder, 0).Build();
                await command.FollowupAsync("Wähle ein Turnier aus", components: tournamentList);
            }
            catch (Exception e)
            {
                if (command.HasResponded == false)
                    await command.RespondAsync(e.Message);
                else
                    await command.FollowupAsync(e.Message);
            }
        }

        internal static async Task LeaveTournament(SocketSlashCommand command)
        {
            using DBAccess dBAccess = new();
            try
            {
                Team joiningTeam = Team.GetAll().FirstOrDefault(t => t.Leader.DiscordUser.DiscordId == command.User.Id) ?? throw new Exception("Du bist in kein Anführer eines Teams");
                List<Tournament> tournaments = [.. Tournament.GetAll().Where(t => t.Teams.Any(t => t.TeamId == joiningTeam.TeamId))];
                if (tournaments.Count == 0)
                {
                    await command.FollowupAsync("Ihr seit in keinem Turnier angemeldet");
                    return;
                }
                SelectMenuBuilder tournamentListSelectMenuBuilder = new SelectMenuBuilder()
                    .WithPlaceholder("Turnier auswählen")
                    .WithCustomId($"leave-tournament:{joiningTeam.TeamId}");
                foreach (Tournament tournament in tournaments)
                {
                    tournamentListSelectMenuBuilder.AddOption(tournament.Name, tournament.TournamentId.ToString());
                }
                MessageComponent tournamentList = new ComponentBuilder()
                .WithSelectMenu(tournamentListSelectMenuBuilder, 0).Build();
                await command.FollowupAsync("Wähle ein Turnier aus", components: tournamentList);
            }
            catch (Exception e)
            {
                if (command.HasResponded == false)
                    await command.RespondAsync(e.Message);
                else
                    await command.FollowupAsync(e.Message);
            }
        }

        internal static string CreateTournament(SocketSlashCommand command)
        {
            try
            {
                Tournament.Create(command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Turniername angegeben"));
                return $"Turnier {command.Data.Options.First().Options.First().Value.ToString()?.Trim()} erstellt";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        internal static string DeleteTournament(SocketSlashCommand command)
        {
            try
            {
                Tournament.Load(Int32.Parse(command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Turniername angegeben"))).Delete();
                return $"Turnier {command.Data.Options.First().Options.First().Value.ToString()?.Trim()} gelöscht";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        internal static string SetRole(SocketSlashCommand command, DiscordSocketClient client)
        {
            ReadOnlySpan<char> roleId = command.Data.Options.First().Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Rollenname angegeben");
            RoleType roleType = (RoleType)Enum.Parse(typeof(RoleType), roleId.ToString());
            SocketGuild guild = client.GetGuild(command.GuildId!.Value);
            SocketRole role = client.GetGuild(command.GuildId!.Value).Roles.FirstOrDefault(r => r.Name == command.Data.Options.First().Options.First().Options.ToList()[1].Value.ToString()) ?? throw new Exception("Rolle nicht gefunden");
            DBAccess dBAccess = new();
            DiscordRole discordRole = dBAccess.DiscordRoles.FirstOrDefault(r => r.RoleType == roleType) ?? throw new Exception("Rolle nicht gefunden");
            discordRole.RoleId = role.Id;
            dBAccess.Update(discordRole);
            dBAccess.SaveChanges();
            return $"Rolle {Enum.GetName(typeof(RoleType), roleType)} zu {role!.Name} zugewiesen";
        }

        internal static string SetPlayerName(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            Player player;
            try
            {
                player = Player.Load(player => player.DiscordUser.DiscordId == command.User.Id)!;
                player.Name = command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Name angegeben");
                player.SaveChanges();
                discordSocketClient.GetGuild(command.GuildId!.Value).GetUser(command.User.Id).ModifyAsync(properties =>
                {
                    properties.Nickname = player.Name;
                }).Wait();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Name zu {player.Name} geändert";
        }

        internal static ulong GetUserId(DiscordSocketClient client, String username, ulong guildId)
        {
            return client.GetGuild(guildId).Users.FirstOrDefault(u => u.Username == username)!.Id;
        }

        internal static string ListTeams(SocketSlashCommand command)
        {
            List<Team> teams = Team.GetAll();
            string teamList = "```";
            teamList += "Teams:\n";
            foreach (Team team in teams)
            {
                teamList += $"{team.TeamId,-4} - {team.Name}\n";
            }
            teamList += "```";
            return teamList;
        }

        internal static string ListTeamMembers(SocketSlashCommand command)
        {
            Team team;
            try
            {
                team = Team.LoadTeam(Int32.Parse(command.Data.Options.First().Options.First().Value.ToString()?.Trim()!))!;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            string teamList = "```";
            teamList += $"Team {team.Name}:\n";
            foreach (Player player in team.Players)
            {
                if(team.Leader.PlayerId == player.PlayerId)
                    teamList += $"{player.Name} 👑\n";
                else
                    teamList += $"{player.Name}\n";
            }
            teamList += "```";
            return teamList;
        }

        internal static string ListPlayers(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            List<Player> players = Player.GetAll();
            string playerList = "```";
            playerList += "Spieler:\n";
            foreach (Player player in players)
            {
                playerList += $"{discordSocketClient.GetUser(player.DiscordUser.DiscordId).Username.PadRight(20)} - {player.Name}\n";
            }
            playerList += "```";
            return playerList;
        }

        internal static string ListTournaments(SocketSlashCommand command)
        {
            ICollection<Tournament> tournaments = Tournament.GetAll();
            string tournamentList = "```";
            tournamentList += "Turniere:\n";
            foreach (Tournament tournament in tournaments)
            {
                tournamentList += $"{tournament.TournamentId,-4} - {tournament.Name}\n";
            }
            tournamentList += "```";
            return tournamentList;
        }

        internal static string CreateGame(SocketSlashCommand command)
        {
            throw new NotImplementedException();
        }

        internal static string DeleteGame(SocketSlashCommand command)
        {
            throw new NotImplementedException();
        }

        internal static string ListGames(SocketSlashCommand command)
        {
            throw new NotImplementedException();
        }
    }
}