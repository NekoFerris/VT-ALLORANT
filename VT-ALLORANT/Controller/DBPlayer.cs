using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VT_ALLORANT.Model;
using VT_ALLORANT.Model.Discord;
using VT_ALLORANT.Model.Valorant;

namespace VT_ALLORANT.Controller
{
    public class DBPlayer : DbContext
    {
        public DbSet<Player> Items { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=database.db").LogTo(Console.WriteLine, LogLevel.Information);
            #else
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=database.db");
            #endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasOne(p => p.DiscordUser)
                .WithOne()
                .HasForeignKey<Player>(p => p.DiscordUserId)
                .IsRequired(true);
                
            modelBuilder.Entity<Player>()
                .HasOne(p => p.ValorantUser)
                .WithOne()
                .HasForeignKey<Player>(p => p.ValorantUserId)
                .IsRequired(true);
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