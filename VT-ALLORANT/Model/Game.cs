namespace VT_ALLORANT.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VT_ALLORANT.Controller;

[Table("Games")]
public class Game
{
    [Key]
    [ForeignKey("GameId")]
    public int GameId { get; set; }
    public string MatchId { get; set; } = "VALORANT-1234";
    public Team Team1 { get; set; }
    public int? Team1Id { get; set; }
    public Team Team2 { get; set; } 
    public int? Team2Id { get; set; }
    public Team? Winner { get; set; } = null;
    public int? WinnerId { get; set; }
    public Player Moderator { get; set; }
    public int? ModeratorId { get; set; }
    public List<Player> Observers { get; set; } = [];
    public int TournamentId { get; set; }
    public int Stage { get; set; }

    public Game()
    {
    }

    public Game(Team team1, Team team2, Player moderator, List<Player> observers, int tournamentId, int stage = 0)
    {
        Team1 = team1;
        Team1Id = team1.TeamId;
        Team2 = team2;
        Team2Id = team2.TeamId;
        Moderator = moderator;
        ModeratorId = moderator.PlayerId;
        Observers = observers;
        TournamentId = tournamentId;
        Stage = stage;
    }

    public void Insert()
    {
        using DBAccess dBAccess = new();
        dBAccess.Games.Add(this);
        dBAccess.SaveChanges();
    }

    public void EndGame(Team winner)
    {
        Winner = winner;
        WinnerId = winner.TeamId;
    }

    public void AddObserver(Player observer)
    {
        DBAccess dBAccess = new();
        dBAccess.GameObservers.Attach(new GameObserver()
        {
            Game = this,
            Observer = observer
        });
        dBAccess.SaveChanges();
    }

    public void RemoveObserver(Player observer)
    {
        DBAccess dBAccess = new();
        dBAccess.GameObservers.Remove(dBAccess.GameObservers.Find(this.GameId, observer.PlayerId) ?? throw new Exception($"Spieler {observer.Name} nicht im Match {this.MatchId} gefunden"));
        dBAccess.SaveChanges();
    }

    public void SetModerator(Player moderator)
    {
        using DBAccess dBAccess = new();
        dBAccess.Games.Find(this.GameId)!.Moderator = moderator;
        dBAccess.SaveChanges();
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

    public void OpenGame()
    {

    }
    public void InvitePlayers()
    {

    }
    public void InviteObservers()
    {

    }
    public void InviteModerator()
    {

    }
}