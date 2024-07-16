using Discord.WebSocket;
using VT_ALLORANT.Controller;
using VT_ALLORANT.Model;

public class SelectMenuCommands()
{
    public static async Task RankSelectMenu(SocketMessageComponent component)
    {
        string[] data = component.Data.CustomId.Split(":");
        Player player = Player.Load(player => player.PlayerId == Int32.Parse(data[1])) ?? throw new Exception("Spieler nicht gefunden");
        if(player.DiscordUser.DiscordId != component.User.Id)
        {
            await component.DeferAsync();
            return;
        }
        using DBAccess dBAccess = new();
        string rank = string.Join(", ", component.Data.Values);
        Player playerToModify = dBAccess.Players.Find(player.PlayerId)!;
        playerToModify!.Rank = Enum.Parse<PlayerRanks>(rank);
        dBAccess.SaveChanges();
        await component.FollowupAsync($"Rang {playerToModify.Rank} erfolgreich für {playerToModify.Name} gesetzt");
        await component.DeleteOriginalResponseAsync();
    }

    public static async Task JoinTournamentSelectMenu(SocketMessageComponent component)
    {
        string[] data = component.Data.CustomId.Split(":");
        Team joiningTeam = Team.Load(team => team.TeamId == Int32.Parse(data[1])) ?? throw new Exception("Team nicht gefunden");
        if(joiningTeam.Leader.DiscordUser.DiscordId != component.User.Id)
        {
            await component.DeferAsync();
            return;
        }
        Tournament tournament = Tournament.Load(Int32.Parse(string.Join(", ", component.Data.Values)));
        if (tournament.Teams.Count < tournament.MaxTeams)
        {
            tournament.AddTeam(joiningTeam);
            await component.FollowupAsync($"Team {joiningTeam.Name} erfolgreich für {tournament.Name} angemeldet");
            await component.DeleteOriginalResponseAsync();
        }
        else
        {
            await component.FollowupAsync($"Das Turnier {tournament.Name} ist bereits voll");
            await component.DeleteOriginalResponseAsync();
        }
    }

    public static async Task LeaveTournamentSelectMenu(SocketMessageComponent component)
    {
        string[] data = component.Data.CustomId.Split(":");
        Team joiningTeam = Team.Load(team => team.TeamId == Int32.Parse(data[1])) ?? throw new Exception("Team nicht gefunden");
        if(joiningTeam.Leader.DiscordUser.DiscordId != component.User.Id)
        {
            await component.DeferAsync();
            return;
        }
        Tournament tournament = Tournament.Load(Int32.Parse(string.Join(", ", component.Data.Values)));
        if (tournament.Teams.Count < tournament.MaxTeams)
        {
            tournament.RemoveTeam(joiningTeam);
            await component.FollowupAsync($"Team {joiningTeam.Name} erfolgreich von {tournament.Name} abgemeldet");
            await component.DeleteOriginalResponseAsync();
        }
    }
}