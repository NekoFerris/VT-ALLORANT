using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VT_ALLORANT.Model
{
    public class TournamentObserver
    {
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
        public int ObserverId { get; set; }
        public Player Observer { get; set; }
    }
}