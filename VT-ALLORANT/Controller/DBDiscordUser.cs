using Microsoft.EntityFrameworkCore;
using VT_ALLORANT.Model.Discord;

namespace VT_ALLORANT.Controller
{
    public class DBDiscordUser : DbContext
    {
        public DbSet<DiscordUser> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ModelBuilder modelBuilder = new ModelBuilder();
            modelBuilder.Entity<DiscordUser>()
                .HasKey(t => t.Id);
            base.OnModelCreating(modelBuilder);
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

        public void DBAccess()
        {
            Database.EnsureCreated();
            Items = Set<DiscordUser>();
        }

        // Create operation
        public async void Add(DiscordUser item)
        {
            Items.Add(item);
            await SaveChangesAsync();
        }

        // Read operation
        public DiscordUser GetById(int id)
        {
            return Items.Find(id) ?? throw new Exception("Item not found");
        }

        public List<DiscordUser> GetAll()
        {
            return Items.ToList();
        }

        // Update operation
        public async void Update(DiscordUser item)
        {
            Items.Update(item);
            await SaveChangesAsync();
        }

        // Delete operation
        public async void Delete(DiscordUser item)
        {
            Items.Remove(item);
            await SaveChangesAsync();
        }
    }
}