using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
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
            bool? wasApproved = IsApproved;
            if (Tournament!.MaxTeamRank < Team!.TeamRank)
            {
                IsApproved = false;
                if (Team.Players.Any(p => (int)p.Rank < (int)Tournament.MinPlayerRank || (int)p.Rank > (int)Tournament.MaxPlayerRank))
                {
                    IsApproved = false;
                }
                else
                {
                    IsApproved = true;
                }
            }
            if (wasApproved != IsApproved)
            {
                dBAccess.Entry(this).State = EntityState.Modified;
                dBAccess.Update(this);
                dBAccess.SaveChanges();
            }
        }

        public static TournamentTeam? Load(int tournamentId, int teamId)
        {
            using DBAccess dBAccess = new();
            return dBAccess.TournamentTeams.Include(t => t.Tournament).Include(t => t.Team).ThenInclude(t => t.Players).FirstOrDefault(t => t.TournamentId == tournamentId && t.TeamId == teamId);
        }

        public static List<TournamentTeam>? Load(Player player)
        {
            using DBAccess dBAccess = new();
            return [.. dBAccess.TournamentTeams.Include(t => t.Tournament).Include(t => t.Team).ThenInclude(t => t.Players).Where(t => t.Team!.Players.Contains(player))];
        }

        public static List<TournamentTeam>? Load(Tournament tournament)
        {
            using DBAccess dBAccess = new();
            return [.. dBAccess.TournamentTeams.Include(t => t.Tournament).Include(t => t.Team).ThenInclude(t => t.Players).Where(t => t.Tournament!.TournamentId == tournament.TournamentId)];
        }
    }
}