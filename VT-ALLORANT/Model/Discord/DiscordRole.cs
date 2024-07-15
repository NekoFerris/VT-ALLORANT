using System.ComponentModel.DataAnnotations;
using VT_ALLORANT.Controller;
using VT_ALLORANT.Model;

public enum RoleType
{
    Admin = 1,
    Moderator = 2,
    Player = 3,
    TeamLeader = 4,
    TeamMember = 5
}

public class DiscordRole()
{
    public ulong RoleId { get; set; }
    [Key]
    public RoleType RoleType { get; set; }

    public static void SetupRoles()
    {
        DBAccess dBAccess = new();
        if (!dBAccess.DiscordRoles.Any())
        {
            dBAccess.DiscordRoles.Add(new DiscordRole()
            {
                RoleId = 1,
                RoleType = RoleType.Admin
            });
            dBAccess.DiscordRoles.Add(new DiscordRole()
            {
                RoleId = 2,
                RoleType = RoleType.Moderator
            });
            dBAccess.DiscordRoles.Add(new DiscordRole()
            {
                RoleId = 3,
                RoleType = RoleType.Player
            });
            dBAccess.DiscordRoles.Add(new DiscordRole()
            {
                RoleId = 4,
                RoleType = RoleType.TeamLeader
            });
            dBAccess.DiscordRoles.Add(new DiscordRole()
            {
                RoleId = 5,
                RoleType = RoleType.TeamMember
            });
            dBAccess.SaveChanges();
        }
    }

    public static void SetRole(RoleType roleType, ulong roleId)
    {
        using DBAccess dBAccess = new();
        if (dBAccess.DiscordRoles.Any(r => r.RoleId == roleId))
        {
            DiscordRole discordRole = dBAccess.DiscordRoles.First(r => r.RoleId == roleId);
            discordRole.RoleId = roleId;
        }
        else
        {
            DiscordRole discordRole = new()
            {
                RoleId = roleId,
                RoleType = roleType
            };
            dBAccess.Add(discordRole);
        }
        dBAccess.SaveChanges();
    }

    public static ulong GetDiscordRoleIdByType(RoleType role)
    {
        DBAccess dBAccess = new();
        DiscordRole discordRole = dBAccess.DiscordRoles.FirstOrDefault(r => (int)r.RoleType == (int)role) ?? throw new Exception("Rolle nicht gefunden");
        return discordRole.RoleId;
    }
}