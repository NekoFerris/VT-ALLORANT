using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VT_ALLORANT.Model.Discord;

namespace VT_ALLORANT.Controller
{
    public class DBDiscordUser : DbContext
    {
        public DbSet<DiscordUser> Items { get; set; }

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
            return Items.Find(id) ?? throw new Exception("DiscordUser not found");
        }

        public DiscordUser GetByUUID(ulong uuid)
        {
            return Items.FirstOrDefault(u => u.DiscordId == uuid) ?? throw new Exception("DiscordUser not found");
        }

        public List<DiscordUser> GetAll()
        {
            return [.. Items];
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