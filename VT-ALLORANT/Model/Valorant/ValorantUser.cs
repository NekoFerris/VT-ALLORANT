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
        public int Id { get; set; }
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
            DBValorantUser dBAccess = new();
            dBAccess.Update(this);
        }

        public void InsertUser()
        {
            DBValorantUser dBAccess = new();
            dBAccess.Add(this);
        }

        public void SaveChanges()
        {
            DBValorantUser dBAccess = new();
            dBAccess.Update(this);
        }

        public void DeleteUser()
        {
            DBValorantUser dBAccess = new();
            dBAccess.Delete(this);
        }

        public static void LoadUser(int Id)
        {
            DBValorantUser dBAccess = new();
            dBAccess.GetById(Id);
        }

        public List<ValorantUser> GetAll()
        {
            DBValorantUser dBAccess = new();
            return dBAccess.GetAll();
        }
    }