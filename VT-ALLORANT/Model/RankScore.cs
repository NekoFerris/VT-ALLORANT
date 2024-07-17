using System.ComponentModel.DataAnnotations;
using VT_ALLORANT.Controller;
using VT_ALLORANT.Model;

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

    public static List<RankScore> DefaultRankScores()
    {
        List<RankScore> rankScores = [];
        foreach (PlayerRanks rank in Enum.GetValues(typeof(PlayerRanks)))
        {
            rankScores.Add(new RankScore()
            {
                RankId = (int)rank,
                Score = (int)rank
            });
        }
        return rankScores;
    }
}