using Microsoft.EntityFrameworkCore;
using VT_ALLORANT.Model;

namespace VT_ALLORANT.Controller
{
    public class DBGame : DbContext
    {
        public DbSet<Game> Items { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

        public void DBAccess()
        {
            Items = Set<Game>();
        }

        // Create operation
        public async void Add(Game item)
        {
            Items.Add(item);
            await SaveChangesAsync();
        }

        // Read operation
        public Game GetById(int id)
        {
            return Items.Find(id) ?? throw new Exception("Item not found");
        }

        public List<Game> GetAll()
        {
            return [.. Items];
        }

        // Update operation
        public async void Update(Game item)
        {
            Items.Update(item);
            await SaveChangesAsync();
        }

        // Delete operation
        public async void Delete(Game item)
        {
            Items.Remove(item);
            await SaveChangesAsync();
        }
    }
}