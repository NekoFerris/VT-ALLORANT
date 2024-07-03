using Discord.WebSocket;
using VT_ALLORANT.Model.Valorant;

namespace VT_ALLORANT.Model.Discord
{
    public static class SlashCommands
    {
        public static string Register(SocketSlashCommand command)
        {
            List<SocketSlashCommandDataOption> options = [.. command.Data.Options];
            ValorantUser valorantUser;
            try
            {
                string valoname = options[0]?.Value?.ToString()?.Trim() ?? throw new Exception("Kein Name angegeben");
                string tag = options[1]?.Value?.ToString()?.Trim() ?? throw new Exception("Kein Tag angegeben");
                valorantUser = new()
                {
                    PUUID = ValorantConnection.GetUserUUIDByNameAndTag(valoname, tag),
                    NAME = valoname,
                    TAG = tag
                };
            }
            catch
            {
                return "Riot Account nicht gefunden. Wenn du glaubst das es ein Fehler ist, kontaktiere bitte einen Admin";
            }
            DiscordUser discordUser = new()
            {
                DiscordId = command.User.Id,
                Username = command.User.Username
            };
            string name = options[2]?.Value?.ToString()?.Trim() ?? throw new Exception("Kein Name angegeben");
            Player player = new(name.Trim(), discordUser, valorantUser);
            player.InsertPlayer();
            return $"Regestrierung für VTuber {name.Trim()} mit dem Valorant Account {valorantUser.NAME}#{valorantUser.TAG} erfolgreich abgeschlossen";
        }

        public static string Unregister(SocketSlashCommand command)
        {
            List<SocketSlashCommandDataOption> options = [.. command.Data.Options];
            Player player;
            try
            {
                player = Player.LoadPlayer(command.User.Id);
                player.DeletePlayer();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Regestrierung für VTuber {options[0].Value.ToString()?.Trim()} erfolgreich gelöscht";
        }

        public static string CreateTeam(SocketSlashCommand command)
        {
            string teamName = command.Data.Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Kein Teamname angegeben");
            Team.CreateTeam(teamName, Player.LoadPlayer(command.User.Id));
            return $"Team {teamName} erstellt";
        }

        public static string DeleteTeam(SocketSlashCommand command)
        {
            Team team;
            try
            {
                Player leader = Player.LoadPlayer(command.User.Id);
                team = Team.LoadTeam(Player.LoadPlayer(command.User.Id));
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
                Player leader = Player.LoadPlayer(command.User.Id);
                team = Team.LoadTeam(leader);
                if (leader.PlayerId != team.Leader.PlayerId)
                {
                    throw new Exception($"Du bist nicht der Anführer des Teams {team.Name}");
                }
                if (team.Players.Count >= team.MaxPlayers)
                {
                    throw new Exception("Das Team ist bereits voll");
                }
                playerToAdd = Player.GetPlayerByDiscordUserName(command.Data.Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"));
                if (team.Players.Contains(playerToAdd))
                {
                    throw new Exception($"{playerToAdd.Name} ist bereits im Team");
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
                Player leader = Player.LoadPlayer(command.User.Id);
                team = Team.LoadTeam(leader);
                if (leader.PlayerId != team.Leader.PlayerId)
                {
                    throw new Exception("Du bist nicht der Anführer dieses Teams");
                }
                playerToRemove = Player.GetPlayerByDiscordUserName(command.Data.Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"));
                team.RemovePlayer(playerToRemove);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"{playerToRemove.Name} wurde vom Team {team.Name} entfernt";
        }

        internal static string ChangeLeader(SocketSlashCommand command)
        {
            Team team;
            Player newLeader;
            try
            {
                Player leader = Player.LoadPlayer(command.User.Id);
                team = Team.LoadTeam(leader);
                if (leader.PlayerId != team.Leader.PlayerId)
                {
                    throw new Exception("Du bist nicht der Anführer dieses Teams");
                }
                newLeader = Player.GetPlayerByDiscordUserName(command.Data.Options.ToList()[0].Value.ToString()?.Trim() ?? throw new Exception("Kein Spieler angegeben"));
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
    }
}