namespace VT_ALLORANT.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

[Table("Games")]
public class Game(Team team1, Team team2, Player moderator, List<Player> observers, int tournamentId, int stage = 0)
{
    public int GameId { get; set; }
    public string MatchId { get; set; } = "VALORANT-1234";
    public Team Team1 { get; set; } = team1;
    public int? Team1Id { get; set; }
    public Team Team2 { get; set; } = team2;
    public int? Team2Id { get; set; }
    public Team? Winner { get; set; } = null;
    public int? WinnerId { get; set; }
    public Player Moderator = moderator;
    public int? ModeratorId { get; set; }
    public List<Player> Observers = observers;
    public int TournamentId { get; set; } = tournamentId;
    public int Stage { get; set; } = stage;

    public void EndGame(Team winner)
    {
        Winner = winner;
        WinnerId = winner.TeamId;
    }

    public void AddObserver(Player observer)
    {
        Observers.Add(observer);
    }

    public void RemoveObserver(Player observer)
    {
        Observers.Remove(observer);
    }

    public void SetModerator(Player moderator)
    {
        Moderator = moderator;
        ModeratorId = moderator.PlayerId;
    }

    public void ChangeTeam(int teamNumber, Team team)
    {
        if (teamNumber == 1)
        {
            Team1 = team;
            Team1Id = team.TeamId;
        }
        else
        {
            Team2 = team;
            Team2Id = team.TeamId;
        }
    }

    public void ChangeTeam2(Team team)
    {
        Team2 = team;
        Team2Id = team.TeamId;
    }

    public void OpenGame()
    {
        //open a valorant match
    }
    public void InvitePlayers()
    {
        //invite players to valorant match
    }
    public void InviteObservers()
    {
        //invite observers to valorant match
    }
    public void InviteModerator()
    {
        //invite moderator to valorant match
    }
}