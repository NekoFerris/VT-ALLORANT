using Microsoft.EntityFrameworkCore;
using VT_ALLORANT.Model;

namespace VT_ALLORANT.Controller
{
    public class DBPlayer : DbContext
    {
        public DbSet<Player> Items { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ModelBuilder modelBuilder = new ModelBuilder();
            modelBuilder.Entity<Player>()
                        .HasOne(t => t.DiscordUser);
            modelBuilder.Entity<Player>()
                        .HasOne(t => t.ValorantUser);
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

        public void DBAccess()
        {
            Database.EnsureCreated();
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
            return Items.Find(id) ?? throw new Exception("Item not found");
        }

        public List<Player> GetAll()
        {
            return Items.ToList();
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