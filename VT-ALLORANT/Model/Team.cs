using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VT_ALLORANT.Controller;
using Microsoft.EntityFrameworkCore;

namespace VT_ALLORANT.Model;

[Table("Team")]
public class Team
{

    // Properties
    [Key]
    [ForeignKey("TeamId")]
    public int TeamId { get; set; }  // Unique ID of the team
    public string Name { get; set; } = "unset"; // Default value "unset
    public int? LeaderId { get; set; } // Default value null
    public Player Leader { get; set; }  // Leader of the team
    public List<Player> Players { get; set; } = []; // Default value new List<Player>()

    // Constructor
    public Team()
    {

    }

    public static Team CreateTeam(string name, Player leader)
    {
        return new Team()
        {
            Name = name,
            Leader = leader,
            Players = new List<Player>() { leader }
        };
    }

    // Methods
    // Add a player to the team
    public void AddPlayer(Player player)
    {
        Players.Add(player);
        SaveChanges();
    }

    // Remove a player from the team
    public void RemovePlayer(Player player)
    {
        Players.Remove(player);
        SaveChanges();
    }
    // Methods
    // Add your custom methods here
    public void InsertTeam()
    {
        DBTeam dBAccess = new();
        dBAccess.Entry(this.Leader).State = EntityState.Unchanged;
        dBAccess.Add(this);
    }

    public static Team LoadTeam(int id)
    {
        DBTeam dBAccess = new();
        return dBAccess.GetById(id);
    }
    public void SaveChanges()
    {
        DBTeam dBAccess = new();
        dBAccess.Update(this);
    }
    public void Delete()
    {
       DBTeam dBAccess = new();
        dBAccess.Delete(this);
    }

    public static List<Team> GetAll()
    {
        DBTeam dBAccess = new();
        return dBAccess.GetAll();
    }
}