using Discord.WebSocket;
using VT_ALLORANT.Model.Valorant;
using MingweiSamuel.Camille.Enums;

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
                return "User not found in Valorant API";
            }
            DiscordUser discordUser = new()
            {
                DiscordId = command.User.Id,
                Username = command.User.Username
            };
            Player player = Player.CreatePlayer(options[2].Value.ToString().Trim(), discordUser, valorantUser);
            player.InsertPlayer();
            return "Register command executed";
        }

        public static string Unregister(SocketSlashCommand command)
        {
            return "Unregister command executed";
        }

        public static string CreateTeam()
        {
            return "Create team command executed";
        }

        public static string DeleteTeam()
        {
            return "Delete team command executed";
        }

        public static string AddPlayer()
        {
            return "Add player command executed";
        }

        public static string RemovePlayer()
        {
            return "Remove player command executed";
        }
    }
}