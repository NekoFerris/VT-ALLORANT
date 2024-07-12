using System.ComponentModel.DataAnnotations;
using VT_ALLORANT.Controller;

public class RankScore
{
    [Key]
    public int RankId { get; set; }
    public int Score { get; set; }

    public static RankScore GetRank(int id)
    {
        using DBAccess dBAccess = new();
        return dBAccess.RankScores.Find(id) ?? throw new Exception("RankScore not found");
    }
}