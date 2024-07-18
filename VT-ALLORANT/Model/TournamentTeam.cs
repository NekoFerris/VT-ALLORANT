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
            if (dBAccess.Tournaments.Any(t => t.MaxTeamRank < Team!.TeamRank))
            {
                IsApproved = false;
                dBAccess.Entry(this).State = EntityState.Modified;
                dBAccess.Update(this);
                dBAccess.SaveChanges();
                return;
            }
            if (!dBAccess.Teams.FirstOrDefault(t => t.TeamId == TeamId)!.Players.Any(p => p.Rank < Tournament!.MinPlayerRank || p.Rank > Tournament!.MaxPlayerRank))
            {
                IsApproved = false;
                dBAccess.Entry(this).State = EntityState.Modified;
                dBAccess.Update(this);
                dBAccess.SaveChanges();
                return;
            }
            IsApproved = true;
            dBAccess.Entry(this).State = EntityState.Modified;
            dBAccess.Update(this);
            dBAccess.SaveChanges();
        }

        public static TournamentTeam? Load(int tournamentId, int teamId)
        {
            using DBAccess dBAccess = new();
            return dBAccess.TournamentTeams.Include(t => t.Tournament).Include(t => t.Team).FirstOrDefault(t => t.TournamentId == tournamentId && t.TeamId == teamId);
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