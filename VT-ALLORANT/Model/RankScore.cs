using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using VT_ALLORANT.Controller;
using VT_ALLORANT.Model;

public class RankScore
{
    [Key]
    public int RankId { get; set; }
    public float Score { get; set; }

    public static RankScore GetRank(int id)
    {
        using DBAccess dBAccess = new();
        return dBAccess.RankScores.Find(id) ?? throw new Exception("RankScore not found");
    }

    public void Update()
    {
        using DBAccess dBAccess = new();
        dBAccess.Entry(this).State = EntityState.Modified;
        dBAccess.RankScores.Update(this);
        dBAccess.SaveChanges();
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

    public static List<RankScore> LoadAll()
    {
        using DBAccess dBAccess = new();
        return [.. dBAccess.RankScores];
    }

    internal static RankScore Load(int RankId)
    {
        using DBAccess dBAccess = new();
        return dBAccess.RankScores.Find(RankId);
    }
}