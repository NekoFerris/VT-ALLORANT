using Discord;
using Discord.WebSocket;
using VT_ALLORANT.Controller;
using VT_ALLORANT.Model.Valorant;

namespace VT_ALLORANT.Model.Discord
{
    public static class SlashCommands
    {
        public static string Register(SocketSlashCommand command)
        {
            List<SocketSlashCommandDataOption> options = [.. command.Data.Options];
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
                    NAME = valoname,
                    TAG = tag
                };
            }
            catch
            {
                return $"Riot Account {valoname}#{tag} nicht gefunden. Wenn du glaubst, dass es ein Fehler ist, kontaktiere bitte einen Admin";
            }
            DiscordUser discordUser = new()
            {
                DiscordId = command.User.Id,
                Username = command.User.Username
            };
            string name = options[2]?.Value?.ToString()?.Trim() ?? throw new Exception("Kein Name angegeben");
            Player player = new(name.Trim(), discordUser, valorantUser);
            player.Insert();
            return $"Regestrierung für VTuber {name.Trim()} mit dem Valorant Account {valorantUser.NAME}#{valorantUser.TAG} erfolgreich abgeschlossen";
        }

        public static string Unregister(SocketSlashCommand command)
        {
            List<SocketSlashCommandDataOption> options = [.. command.Data.Options];
            Player player;
            try
            {
                player = Player.Load(command.User.Id);
                player.Delete();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Regestrierung für VTuber {options[0].Value.ToString()?.Trim()} erfolgreich gelöscht";
        }

        public static string CreateTeam(SocketSlashCommand command)
        {
            string teamName = command.Data.Options.First().Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Kein Teamname angegeben");
            Team.Create(teamName, Player.Load(command.User.Id));
            return $"Team {teamName} erstellt";
        }

        public static string DeleteTeam(SocketSlashCommand command)
        {
            Team team;
            try
            {
                Player leader = Player.Load(command.User.Id);
                team = Team.Load(Player.Load(command.User.Id));
                if (leader.PlayerId != team.Leader.PlayerId)
                {
                    throw new Exception($"Du bist nicht der Anführer des Teams {team.Name}");
                }
                team.Delete();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Team {team.Name} gelöscht";
        }

        public static string AddPlayer(SocketSlashCommand command)
        {
            Team team;
            Player playerToAdd;
            try
            {
                Player leader = Player.Load(command.User.Id);
                team = Team.Load(leader);
                if (leader.PlayerId != team.Leader.PlayerId)
                {
                    throw new Exception($"Du bist nicht der Anführer des Teams {team.Name}");
                }
                if (team.Players.Count >= team.MaxPlayers)
                {
                    throw new Exception($"Das Team {team.Name} ist bereits voll");
                }
                playerToAdd = Player.GetPlayerByDiscordUserName(command.Data.Options.First().Options.First().Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"));
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

        public static string RemovePlayer(SocketSlashCommand command)
        {
            Team team;
            Player playerToRemove;
            try
            {
                Player leader = Player.Load(command.User.Id);
                team = Team.Load(leader);
                if (leader.PlayerId != team.Leader.PlayerId)
                {
                    throw new Exception("Du bist nicht der Anführer dieses Teams");
                }
                playerToRemove = Player.GetPlayerByDiscordUserName(command.Data.Options.First().Options.First().Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"));
                team.RemovePlayer(playerToRemove);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"{playerToRemove.Name} wurde vom Team {team.Name} entfernt";
        }

        public static string ChangeLeader(SocketSlashCommand command)
        {
            Team team;
            Player newLeader;
            try
            {
                Player leader = Player.Load(command.User.Id);
                team = Team.Load(leader);
                if (leader.PlayerId != team.Leader.PlayerId)
                {
                    throw new Exception("Du bist nicht der Anführer dieses Teams");
                }
                newLeader = Player.GetPlayerByDiscordUserName(command.Data.Options.First().Options.First().Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"));
                if (newLeader.PlayerId == team.Leader.PlayerId)
                {
                    throw new Exception("Du bist bereits der Anführer dieses Teams");
                }
                team.SetLeader(newLeader);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"{newLeader.Name} ist jetzt der Anführer vom Team {team.Name}";
        }

        internal static string SendFriendRequest(SocketSlashCommand command)
        {
            string name = command.Data.Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Kein Name angegeben");
            try
            {
                Player player = Player.GetPlayerByDiscordUserName(name);
                ValorantConnection.SendFriendRequest(player.ValorantUser.NAME, player.ValorantUser.TAG);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Freundschaftsanfrage an {name} gesendet";
        }

        internal static string MatchGame(SocketSlashCommand command)
        {
            Team team1;
            Team team2;
            try
            {
                team1 = Team.LoadTeam(command.Data.Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Team 1 nicht angegeben"));
                team2 = Team.LoadTeam(command.Data.Options.ToList()[1].Value.ToString()?.Trim() ?? throw new Exception("Team 2 nicht angegeben"));
                if (team1.TeamId == team2.TeamId)
                {
                    throw new Exception("Zwei mal das selbe Team ausgewählt");
                }
                Game game = new(team1, team2, Player.Load(command.User.Id), new(), 0);
                game.Insert();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Spiel zwischen {team1.Name} und {team2.Name} erstellt";
        }

        internal static async Task UpdateRanking(SocketSlashCommand command)
        {
            Player player;
            try
            {
                if (command.Data.Options.Count == 0)
                {
                    player = Player.Load(command.User.Id);
                }
                else
                {
                    player = Player.GetPlayerByDiscordUserName(command.Data.Options.ToList()[0].Value.ToString()?.Trim()!);
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
                Tournament.Create(command.Data.Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Kein Turniername angegeben"));
                return $"Turnier {command.Data.Options.ToList()[0].Value.ToString()?.Trim()} erstellt";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        internal static Optional<string> DeleteTournament(SocketSlashCommand command)
        {
            throw new NotImplementedException();
        }
    }
}