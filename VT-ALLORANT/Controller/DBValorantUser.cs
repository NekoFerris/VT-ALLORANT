using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VT_ALLORANT.Model.Valorant;

namespace VT_ALLORANT.Controller
{
    public class DBValorantUser : DbContext
    {
        public DbSet<ValorantUser> Items { get; set; }

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

        public void DBAccess()
        {
            Database.EnsureCreated();
            Items = Set<ValorantUser>();
        }

        // Create operation
        public async void Add(ValorantUser item)
        {
            Items.Add(item);
            await SaveChangesAsync();
        }

        // Read operation
        public ValorantUser GetById(int id)
        {
            return Items.Find(id) ?? throw new Exception("Item not found");
        }

        public List<ValorantUser> GetAll()
        {
            return [.. Items];
        }

        // Update operation
        public async void Update(ValorantUser item)
        {
            Items.Update(item);
            await SaveChangesAsync();
        }

        // Delete operation
        public async void Delete(ValorantUser item)
        {
            Items.Remove(item);
            await SaveChangesAsync();
        }
    }
}