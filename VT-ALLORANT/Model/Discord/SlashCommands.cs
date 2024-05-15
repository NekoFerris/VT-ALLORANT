using Discord.WebSocket;
using VT_ALLORANT.Model.Valorant;
using MingweiSamuel.Camille.Enums;
using Discord;
using System.Reflection.Metadata.Ecma335;

namespace VT_ALLORANT.Model.Discord
{
    public static class SlashCommands
    {
        public static string Register(SocketSlashCommand command)
        {
            var options = command.Data.Options.ToList(); // Convert IReadOnlyCollection to List
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
            return "Regestrierung erfolgreich";
        }

        public static string Unregister(SocketSlashCommand command)
        {
            return "Unregister command executed";
        }

        public static string CreateTeam(SocketSlashCommand command)
        { 
            Team.CreateTeam(command.Data.Options.ToList()[0].Value.ToString().Trim(), Player.LoadPlayer(command.User.Id)).InsertTeam();
            return $"Team {command.Data.Options.ToList()[0].Value.ToString().Trim()} erstellt";
        }

        public static string DeleteTeam(SocketSlashCommand command)
        {
            return "Delete team command executed";
        }

        public static string AddPlayer(SocketSlashCommand command)
        {
            return "Add player command executed";
        }

        public static string RemovePlayer(SocketSlashCommand command)
        {
            return "Remove player command executed";
        }

        internal static string ChangeLeader(SocketSlashCommand command)
        {
            return "Change leader command executed";
        }
    }
}