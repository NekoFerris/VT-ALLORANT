using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VT_ALLORANT.Controller;

namespace VT_ALLORANT.Model
{
    public class TournamentTeam
    {
        public int TournamentId { get; set; }
        public Tournament? Tournament { get; set; }
        public int TeamId { get; set; }
        public Team? Team { get; set; }
        public bool? IsApproved { get; set; }

        public void CheckApproval()
        {
            DBAccess dBAccess = new();
            if (dBAccess.Tournaments.Any(t => t.MaxTeamRank < Team!.TeamRank))
            {
                IsApproved = false;
                return;
            }
            if (dBAccess.Teams.FirstOrDefault(t => t.TeamId == TeamId)!.Players.Any(p => p.Rank < Tournament!.MinPlayerRank || p.Rank > Tournament!.MaxPlayerRank))
            {
                IsApproved = false;
                return;
            }
            IsApproved = true;
        }
    }
}