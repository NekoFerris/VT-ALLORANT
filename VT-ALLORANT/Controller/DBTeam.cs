using Microsoft.EntityFrameworkCore;
using VT_ALLORANT.Model;

namespace VT_ALLORANT.Controller
{
    public class DBTeam : DbContext
    {
        public DbSet<Team> Items { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=database.db");
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