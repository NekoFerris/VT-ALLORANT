using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VT_ALLORANT.Controller;

namespace VT_ALLORANT.Model.Valorant;

[Table("ValorantUser")]
public class ValorantUser
{
    // Properties
    [Key]
    [ForeignKey("ValorantUserId")]
    public int ValorantUserId { get; set; }
    public string PUUID { get; set; }
    public string NAME { get; set; }
    public string TAG { get; set; }

    // Constructor
    public ValorantUser()
    {

    }

    // Methods
    public void SendMessage(string message)
    {
        DBAccess dBAccess = new();
        dBAccess.Update(this);
    }

    public void InsertUser()
    {
        DBAccess dBAccess = new();
        dBAccess.Add(this);
        dBAccess.SaveChanges();
    }

    public void SaveChanges()
    {
        DBAccess dBAccess = new();
        dBAccess.Update(this);
        dBAccess.SaveChanges();
    }

    public void DeleteUser()
    {
        DBAccess dBAccess = new();
        dBAccess.Remove(this);
        dBAccess.SaveChanges();
    }

    public static ValorantUser LoadUser(int Id)
    {
        DBAccess dBAccess = new();
        return dBAccess.ValorantUsers.Find(Id);
    }

    public static List<ValorantUser> GetAll()
    {
        DBAccess dBAccess = new();
        return dBAccess.ValorantUsers.ToList();
    }
}