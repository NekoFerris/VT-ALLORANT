using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VT_ALLORANT.Model;

namespace VT_ALLORANT.Controller
{
    public class DBTeam : DbContext
    {
        public DbSet<Team> Items { get; set; }


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
            modelBuilder.Entity<Team>()
                .HasMany(e => e.Players)
                .WithMany(e => e.Teams)
                .UsingEntity("PlayersInTeams");

            modelBuilder.Entity<Team>()
                .HasOne(e => e.Leader)
                .WithOne()
                .HasForeignKey<Team>(e => e.LeaderId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public void DBAccess()
        {
            Items = Set<Team>();
        }

        // Create operation
        public async void Add(Team item)
        {
            Items.Add(item);
            await SaveChangesAsync();
        }

        // Read operation
        public Team GetById(int id)
        {
            return Items.Find(id) ?? throw new Exception("Item not found");
        }

        public List<Team> GetAll()
        {
            return [.. Items];
        }

        // Update operation
        public async void Update(Team item)
        {
            Items.Update(item);
            await SaveChangesAsync();
        }

        // Delete operation
        public async void Delete(Team item)
        {
            Items.Remove(item);
            await SaveChangesAsync();
        }
    }
}