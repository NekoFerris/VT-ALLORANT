using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VT_ALLORANT.Model
{
    public class TournamentModerator
    {
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
        public int ModeratorId { get; set; }
        public Player Moderator { get; set; }
    }
}