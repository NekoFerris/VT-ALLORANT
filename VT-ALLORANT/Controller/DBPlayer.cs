using Microsoft.EntityFrameworkCore;
using VT_ALLORANT.Model;

namespace VT_ALLORANT.Controller
{
    public class DBPlayer : DbContext
    {
        public DbSet<Player> Items { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

        public void DBAccess()
        {
            Items = Set<Player>();
        }

        // Create operation
        public async void Add(Player item)
        {
            Items.Add(item);
            await SaveChangesAsync();
        }

        // Read operation
        public Player GetById(int id)
        {
            return Items.Find(id) ?? throw new Exception("Player not found");
        }
        public Player GetByDiscordID(ulong id)
        {
            return Items.FirstOrDefault(p => p.DiscordUser.DiscordId == id) ?? throw new Exception("Player not found");
        }

        public List<Player> GetAll()
        {
            return [.. Items];
        }

        // Update operation
        public async void Update(Player item)
        {
            Items.Update(item);
            await SaveChangesAsync();
        }

        // Delete operation
        public async void Delete(Player item)
        {
            Items.Remove(item);
            await SaveChangesAsync();
        }
    }
}