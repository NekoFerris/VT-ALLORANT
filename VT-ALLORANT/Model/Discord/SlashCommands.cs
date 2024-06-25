using Discord.WebSocket;
using VT_ALLORANT.Model.Valorant;

namespace VT_ALLORANT.Model.Discord
{
    public static class SlashCommands
    {
        public static string Register(SocketSlashCommand command)
        {
            List<SocketSlashCommandDataOption> options = command.Data.Options.ToList();
            ValorantUser valorantUser = null;
            try
            {
                valorantUser = new()
                {
                    PUUID = ValorantConnection.GetUserUUIDByNameAndTag(options[0].Value.ToString().Trim(), options[1].Value.ToString().Trim()),
                    NAME = options[0].Value.ToString().Trim(),
                    TAG = options[1].Value.ToString().Trim()
                };
            }
            catch (Exception e)
            {
                return "Riot Account nicht gefunden";
            }
            DiscordUser discordUser = new()
            {
                DiscordId = command.User.Id,
                Username = command.User.Username
            };
            Player player = Player.CreatePlayer(options[2].Value.ToString().Trim(), discordUser, valorantUser);
            player.InsertPlayer();
            return $"Regestrierung für VTuber {options[2].Value.ToString().Trim()} mit dem Valorant Account {valorantUser.NAME}#{valorantUser.TAG} erfolgreich abgeschlossen";
        }

        public static string Unregister(SocketSlashCommand command)
        {
            List<SocketSlashCommandDataOption> options = command.Data.Options.ToList();
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
            return $"Regestrierung für VTuber {options[2].Value.ToString().Trim()} erfolgreich gelöscht";
        }

        public static string CreateTeam(SocketSlashCommand command)
        { 
            Team.CreateTeam(command.Data.Options.ToList()[0].Value.ToString().Trim(), Player.LoadPlayer(command.User.Id));
            return $"Team {command.Data.Options.ToList()[0].Value.ToString().Trim()} erstellt";
        }

        public static string DeleteTeam(SocketSlashCommand command)
        {
            Team team;
            try
            {
                team = Team.LoadTeam(Player.LoadPlayer(command.User.Id));
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
            //try
            {
            Player leader = Player.LoadPlayer(command.User.Id);
            team = Team.LoadTeam(leader);
            if (leader.PlayerId != team.Leader.PlayerId)
            {
                throw new Exception("Du bist nicht der Anführer dieses Teams");
            }
            playerToAdd = Player.GetPlayerByDiscordUserName(command.Data.Options.ToList()[0].Value.ToString().Trim());
            team.AddPlayer(playerToAdd);
            }
            //catch (Exception e)
            {
                //return e.Message;
            }
            return $"Spieler {playerToAdd.Name} wurde zu Team {team.Name} hinzugefügt";
        }

        public static string RemovePlayer(SocketSlashCommand command)
        {
            Team team;
            Player playerToRemove;
            try
            {
            team = Team.LoadTeam(Player.LoadPlayer(command.User.Id));
            playerToRemove = Player.GetPlayerByDiscordUserName(command.Data.Options.ToList()[0].Value.ToString().Trim());
            team.RemovePlayer(playerToRemove);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Spieler {playerToRemove.Name} wurde vom Team {team.Name} entfernt";
        }

        internal static string ChangeLeader(SocketSlashCommand command)
        {
            Team team;
            Player newLeader;
            try
            {
            team = Team.LoadTeam(Player.LoadPlayer(command.User.Id));
            newLeader = Player.GetPlayerByDiscordUserName(command.Data.Options.ToList()[0].Value.ToString().Trim());
            team.Leader = newLeader;
            team.Update();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return $"Spieler {newLeader.Name} ist jetzt der Anführer vom Team {team.Name}";
        }
    }
}