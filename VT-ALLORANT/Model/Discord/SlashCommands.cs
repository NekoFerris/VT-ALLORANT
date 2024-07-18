using Discord;
using Discord.Commands;
using Discord.WebSocket;
using VT_ALLORANT.Controller;
using VT_ALLORANT.Model.Valorant;

namespace VT_ALLORANT.Model.Discord
{
    public static class SlashCommands
    {
        public static string RegisterPlayer(SocketSlashCommand command, DiscordSocketClient client)
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

        public static string UnregisterPlayer(SocketSlashCommand command, DiscordSocketClient client)
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
            try
            {
            string teamName = command.Data.Options.First().Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Kein Teamname angegeben");
            Player player = Player.Load(player => player.DiscordUser.DiscordId == command.User.Id) ?? throw new Exception("Du bist nicht registriert");
            if (player.IsInAnyTeam)
            {
                return "Du bist bereits in einem Team";
            }
            Team.Create(teamName, player);
            return $"Team {teamName} erstellt";
            }
            catch (Exception e)
            {
                return e.Message;
            };
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

        public static string AddPlayerToTeam(SocketSlashCommand command, DiscordSocketClient client)
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
                playerToAdd = Player.Load(player => player.DiscordUser.DiscordId == GetDiscordUserId(client, command.Data.Options.First().Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"), command.GuildId!.Value)) ?? throw new Exception("Spieler nicht regestriert");
                if (playerToAdd.IsInAnyTeam)
                {
                    throw new Exception($"{playerToAdd.Name} ist bereits in einem anderen Team");
                }
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

        public static string RemovePlayerFromTeam(SocketSlashCommand command, DiscordSocketClient client)
        {
            Team team;
            Player playerToRemove;
            try
            {
                team = Team.Load(team => team.Leader.PlayerId == Player.Load(player => player.DiscordUser.DiscordId == command.User.Id)!.PlayerId) ?? throw new Exception("Du bist nicht der Anführer eines Teams");
                playerToRemove = Player.Load(player => player.DiscordUser.DiscordId == GetDiscordUserId(client, command.Data.Options.First().Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"), command.GuildId!.Value))!;
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

        public static string ChangeLeaderFromTeam(SocketSlashCommand command, DiscordSocketClient client)
        {
            Team team;
            Player newLeader;
            try
            {
                team = Team.Load(team => team.Leader.PlayerId == Player.Load(player => player.DiscordUser.DiscordId == command.User.Id)!.PlayerId) ?? throw new Exception("Du bist nicht der Anführer eines Teams");
                newLeader = Player.Load(player => player.DiscordUser.DiscordId == GetDiscordUserId(client, command.Data.Options.First().Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"), command.GuildId!.Value))!;
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

        internal static string MatchGameForTournament(SocketSlashCommand command)
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
                if (command.Data.Options.First().Options.First().Options.Count == 0)
                {
                    player = Player.Load(player => player.DiscordUser.DiscordId == command.User.Id)!;
                }
                else
                {
                    player = Player.Load(player => player.DiscordUser.DiscordId == GetDiscordUserId(client, command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"), command.GuildId!.Value))!;
                }
                MessageComponent rankList = new ComponentBuilder()
                .WithSelectMenu(new SelectMenuBuilder()
                    .WithPlaceholder("Rang auswählen")
                    .WithCustomId($"rank-for:{player.PlayerId}")
                    .AddOption("Eisen 1", "2")
                    .AddOption("Eisen 2", "3")
                    .AddOption("Eisen 3", "4")
                    .AddOption("Bronze 1", "5")
                    .AddOption("Bronze 2", "6")
                    .AddOption("Bronze 3", "7")
                    .AddOption("Silber 1", "8")
                    .AddOption("Silber 2", "9")
                    .AddOption("Silber 3", "10")
                    .AddOption("Gold 1", "11")
                    .AddOption("Gold 2", "12")
                    .AddOption("Gold 3", "13")
                    .AddOption("Platin 1", "14")
                    .AddOption("Platin 2", "15")
                    .AddOption("Platin 3", "16")
                    .AddOption("Diamant 1", "17")
                    .AddOption("Diamant 2", "18")
                    .AddOption("Diamant 3", "19")
                    .AddOption("Aufgestiegen 1", "20")
                    .AddOption("Aufgestiegen 2", "21")
                    .AddOption("Aufgestiegen 3", "22")
                    .AddOption("Unsterblich 1", "23")
                    .AddOption("Unsterblich 2", "24")
                    .AddOption("Unsterblich 3", "25")
                    .AddOption("Radiant", "26"))
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

        internal static async Task TeamJoinTournament(SocketSlashCommand command)
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

        internal static async Task TeamLeaveTournament(SocketSlashCommand command)
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

        internal static string CreateTournament(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin) || r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Moderator)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
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

        internal static string DeleteTournament(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin) || r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Moderator)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
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

        internal static string SetRoleToDiscordRole(SocketSlashCommand command, DiscordSocketClient client)
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

        internal static ulong GetDiscordUserId(DiscordSocketClient client, String username, ulong guildId)
        {
            return client.GetGuild(guildId).Users.FirstOrDefault(u => u.Username == username)!.Id;
        }

        internal static List<SocketRole> GetDiscordUserRoles(DiscordSocketClient client, ulong userId, ulong guildId)
        {
            return [.. client.GetGuild(guildId).GetUser(userId).Roles];
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

        internal static string ShowTeam(SocketSlashCommand command)
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
            teamList += $"Team: {team.Name}\n";
            teamList += $"Gestamtpunkte: {team.TeamRank}\n";
            teamList += "Spieler:\n";
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
            List<Tournament> tournaments = [.. Tournament.GetAll()];
            string tournamentList = "```";
            tournamentList += "Turniere:\n";
            foreach (Tournament tournament in tournaments)
            {
                tournamentList += $"{tournament.TournamentId,-4} - {tournament.Name}\n";
            }
            tournamentList += "```";
            return tournamentList;
        }

        internal static string ShowTournament(SocketSlashCommand command)
        {
            Tournament tournament;
            try
            {
                tournament = Tournament.Load(Int32.Parse(command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Turnier angegeben")))!;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            string tournamentList = "```";
            tournamentList += $"Turnier: {tournament.Name}\n";
            if (tournament.OpenForRegistration)
            {
            tournamentList += $"Offen für Registrierung: Ja\n";
            }
            else
            {
            tournamentList += $"Offen für Registrierung: Nein\n";
            }
            tournamentList += $"Maximale Anzahl an Teams: {tournament.MaxTeams}\n";
            tournamentList += $"Minimale Spieler Rang: {tournament.MinPlayerRank}\n";
            tournamentList += $"Maximaler Spieler Rang: {tournament.MaxPlayerRank}\n";
            tournamentList += $"Maximaler Team Rang: {(float)tournament.MaxTeamRank/10}\n";
            tournamentList += $"Aktuelle Stage: {tournament.CurrentStage}\n";
            tournamentList += $"Teams:\n";
            foreach (Team team in tournament.Teams)
            {
                if(team.IsApproved(tournament))
                    tournamentList += $"{team.Name} ✅\n";
                else
                    tournamentList += $"{team.Name} ❌\n";
            }
            tournamentList += "```";
            return tournamentList;
        }

        internal static string SetTournamentOpenForRegistration(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin) || r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Moderator)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
            Tournament tournament;
            try
            {
                tournament = Tournament.Load(Int32.Parse(command.Data.Options.First().Options.ToList()[0].ToString()?.Trim() ?? throw new Exception("Kein Turnier angegeben")))!;
                tournament.OpenForRegistration = bool.Parse(command.Data.Options.First().Options.First().Options.ToList()[1].ToString()?.Trim() ?? throw new Exception("Kein Wert angegeben"));
                tournament.Update();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Turnier {tournament.Name} ist jetzt offen für Registrierung";
        }

        internal static string CreateGame(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin) || r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Moderator)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
            Team team1;
            Team team2;
            Tournament tournament;
            Player moderator;
            try
            {
                team1 = Team.LoadTeam(Int32.Parse(command.Data.Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Team 1 nicht angegeben")))!;
                team2 = Team.LoadTeam(Int32.Parse(command.Data.Options.ToList()[1].Value.ToString()?.Trim() ?? throw new Exception("Team 2 nicht angegeben")))!;
                tournament = Tournament.Load(Int32.Parse(command.Data.Options.ToList()[2].Value.ToString()?.Trim() ?? throw new Exception("Kein Turnier angegeben")))!;
                moderator = Player.Load(player => player.DiscordUser.DiscordId == GetDiscordUserId(discordSocketClient, command.Data.Options.ToList()[4].Value.ToString()?.Trim()!, command.GuildId!.Value)) ?? throw new Exception("Du bist nicht registriert");
                if (team1.TeamId == team2.TeamId)
                {
                    throw new Exception("Zwei mal das selbe Team ausgewählt");
                }
                Game game = new(team1, team2, moderator, [], tournament.CurrentStage);
                game.Insert();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Spiel zwischen {team1.Name} und {team2.Name} erstellt";
        }

        internal static string DeleteGame(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin) || r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Moderator)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
            try
            {
                Game gameToDelete = Game.Load(g => g.GameId == Int32.Parse(command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Spiel angegeben"))) ?? throw new Exception("Spiel nicht gefunden");
                gameToDelete.Delete();
                return $"Spiel {command.Data.Options.First().Options.First().Value.ToString()?.Trim()} gelöscht";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        internal static string ListGames(SocketSlashCommand command)
        {
            List<Game> games = [.. Game.GetAll()];
            string gameList = "```";
            gameList += "Spiele:\n";
            int stage = -1;
            foreach (Game game in games)
            {
                if (stage != game.Stage)
                {
                    stage = game.Stage;
                    gameList += $"\nStage {stage}:\n";
                }
                gameList += $"{game.GameId,-4} - {game.Team1.Name} vs {game.Team2.Name}\n";
            }
            gameList += "```";
            return gameList;
        }

        internal static string SetTournamentStage(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
            Tournament tournament;
            try
            {
                tournament = Tournament.Load(Int32.Parse(command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Turnier angegeben")))!;
                tournament.CurrentStage = Int32.Parse(command.Data.Options.First().Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Wert angegeben"));
                tournament.Update();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Turnier {tournament.Name} ist jetzt in Stage {tournament.CurrentStage}";
        }

        internal static string SetTournamentMaxTeams(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
            Tournament tournament;
            try
            {
                tournament = Tournament.Load(Int32.Parse(command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Turnier angegeben")))!;
                tournament.MaxTeams = Int32.Parse(command.Data.Options.First().Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Wert angegeben"));
                tournament.Update();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Turnier {tournament.Name} hat jetzt ein Maximum von {tournament.MaxTeams} Teams";
        }

        internal static string SetTournamentMaxPlayerRank(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
            Tournament tournament;
            try
            {
                tournament = Tournament.Load(Int32.Parse(command.Data.Options.First().Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Turnier angegeben")))!;
                tournament.MaxPlayerRank = Enum.Parse<PlayerRanks>(command.Data.Options.First().Options.First().Options.ToArray()[1].Value.ToString()?.Trim() ?? throw new Exception("Kein Wert angegeben"));
                tournament.Update();
                TournamentTeam.Load(tournament).ForEach(tt => tt.CheckApproval());
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Turnier {tournament.Name} hat jetzt einen maximalen Spielerrang von {tournament.MaxPlayerRank}";
        }

        internal static string SetTournamentMinPlayerRank(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
            Tournament tournament;
            try
            {
                tournament = Tournament.Load(Int32.Parse(command.Data.Options.First().Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Turnier angegeben")))!;
                tournament.MinPlayerRank = Enum.Parse<PlayerRanks>(command.Data.Options.First().Options.First().Options.ToArray()[1].Value.ToString()?.Trim() ?? throw new Exception("Kein Wert angegeben"));
                tournament.Update();
                TournamentTeam.Load(tournament).ForEach(tt => tt.CheckApproval());
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Turnier {tournament.Name} hat jetzt einen Minimalen Spielerrang von {tournament.MinPlayerRank}";
        }

        internal static string SetTournamentMaxTeamRank(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
            Tournament tournament;
            try
            {
                tournament = Tournament.Load(Int32.Parse(command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Turnier angegeben")))!;
                tournament.MaxTeamRank = Int32.Parse(command.Data.Options.First().Options.First().Options.ToArray()[1].Value.ToString()?.Trim() ?? throw new Exception("Kein Wert angegeben"));
                tournament.Update();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Turnier {tournament.Name} hat jetzt ein maximales Teamranking von {tournament.MaxTeamRank}";
        }

        internal static string AddObserverToTournament(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
            Tournament tournament;
            Player observer;
            try
            {
                tournament = Tournament.Load(Int32.Parse(command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Turnier angegeben")))!;
                observer = Player.Load(player => player.DiscordUser.DiscordId == GetDiscordUserId(discordSocketClient, command.Data.Options.First().Options.First().Options.ToArray()[1].Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"), command.GuildId!.Value)) ?? throw new Exception("Spieler nicht regestriert");
                tournament.AddObserver(observer);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"{observer.Name} ist jetzt Beobachter von {tournament.Name}";
        }

        internal static string RemoveObserverFromTournament(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
            Tournament tournament;
            Player observer;
            try
            {
                tournament = Tournament.Load(Int32.Parse(command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Turnier angegeben")))!;
                observer = Player.Load(player => player.DiscordUser.DiscordId == GetDiscordUserId(discordSocketClient, command.Data.Options.First().Options.First().Options.ToArray()[1].Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"), command.GuildId!.Value)) ?? throw new Exception("Spieler nicht regestriert");
                tournament.RemoveObserver(observer);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"{observer.Name} ist jetzt kein Beobachter von {tournament.Name} mehr";
        }

        internal static string AddModeratorToTournament(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
            Tournament tournament;
            Player moderator;
            try
            {
                tournament = Tournament.Load(Int32.Parse(command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Turnier angegeben")))!;
                moderator = Player.Load(player => player.DiscordUser.DiscordId == GetDiscordUserId(discordSocketClient, command.Data.Options.First().Options.First().Options.ToArray()[1].Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"), command.GuildId!.Value)) ?? throw new Exception("Spieler nicht regestriert");
                tournament.AddModerator(moderator);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"{moderator.Name} ist jetzt Moderator von {tournament.Name}";
        }

        internal static string RemoveModeratorFromTournament(SocketSlashCommand command, DiscordSocketClient discordSocketClient)
        {
            if (!GetDiscordUserRoles(discordSocketClient, command.User.Id, command.GuildId!.Value).Any(r => r.Id == DiscordRole.GetDiscordRoleIdByType(RoleType.Admin)))
            {
                return "Du hast nicht die Berechtigung für diesen Befehl";
            }
            Tournament tournament;
            Player moderator;
            try
            {
                tournament = Tournament.Load(Int32.Parse(command.Data.Options.First().Options.First().Value.ToString()?.Trim() ?? throw new Exception("Kein Turnier angegeben")))!;
                moderator = Player.Load(player => player.DiscordUser.DiscordId == GetDiscordUserId(discordSocketClient, command.Data.Options.First().Options.First().Options.ToArray()[1].Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"), command.GuildId!.Value)) ?? throw new Exception("Spieler nicht regestriert");
                tournament.RemoveModerator(moderator);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"{moderator.Name} ist jetzt kein Moderator von {tournament.Name} mehr";
        }

        internal static string ListRanks(SocketSlashCommand command)
        {
            string rankList = "```";
            rankList += "Ränge:\n";
            foreach (PlayerRanks rank in Enum.GetValues(typeof(PlayerRanks)))
            {
                rankList += $"{(int)rank,-4} - {rank}\n";
            }
            rankList += "```";
            return rankList;
        }
    }
}