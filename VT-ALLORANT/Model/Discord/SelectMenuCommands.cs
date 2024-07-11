using Discord.WebSocket;
using VT_ALLORANT.Controller;
using VT_ALLORANT.Model;

public class SelectMenuCommands()
{
    public static async Task TeamSelectMenu(SocketMessageComponent component)
    {
        string[] data = component.Data.CustomId.Split(":");
        string action = data[1];
        string teamName = data[2];
        Player player = Player.Load(component.User.Id);
        Team team = Team.Load(player);
    }

    public static async Task PlayerSelectMenu(SocketMessageComponent component)
    {
        string[] data = component.Data.CustomId.Split(":");
        string action = data[1];
        string playerName = data[2];
        Player player = Player.GetPlayerByDiscordUserName(playerName);
        Team team = Team.Load(player);
    }

    public static async Task RankSelectMenu(SocketMessageComponent component)
    {
        string[] data = component.Data.CustomId.Split(":");
        Player player = Player.Load(Int32.Parse(data[1]));
        using DBAccess dBAccess = new();
        string rank = string.Join(", ", component.Data.Values);
        Player playerToModify = dBAccess.Players.Find(player.PlayerId);
        playerToModify!.Rank = Enum.Parse<PlayerRanks>(rank);
        dBAccess.SaveChanges();
        await component.FollowupAsync($"Rang {playerToModify.Rank} erfolgreich für {playerToModify.Name} gesetzt");
    }

    public static async Task JoinTournamentSelectMenu(SocketMessageComponent component)
    {
        string[] data = component.Data.CustomId.Split(":");
        Team joiningTeam = Team.Load(Player.Load(component.User.Id));
        Tournament tournament = Tournament.Load(Int32.Parse(data[1]));
        if (tournament.Teams.Count < tournament.MaxTeams)
        {
            tournament.AddTeam(joiningTeam);
            await component.FollowupAsync($"Team {joiningTeam.Name} erfolgreich für {tournament.Name} angemeldet");
        }
        else
        {
            await component.FollowupAsync($"Das Turnier {tournament.Name} ist bereits voll");
        }
    }

    public static async Task LeaveTournamentSelectMenu(SocketMessageComponent component)
    {
        string[] data = component.Data.CustomId.Split(":");
        Team leavingTeam = Team.Load(Player.Load(component.User.Id));
        Tournament tournament = Tournament.Load(Int32.Parse(data[1]));
        tournament.RemoveTeam(leavingTeam);
        await component.FollowupAsync($"Team {leavingTeam.Name} erfolgreich von {tournament.Name} abgemeldet");
    }
}