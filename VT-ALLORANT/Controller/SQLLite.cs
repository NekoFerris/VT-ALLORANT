using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VT_ALLORANT.Controller
{
    public class DBGame<T> : DbContext where T : class
    {
        public DbSet<T> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=database.db");
            Database.EnsureCreated();
        }

        // Create operation
        public void Add(T item)
        {
            Items.Add(item);
            SaveChanges();
        }

        // Read operation
        public T GetById(int id)
        {
            return Items.Find(id);
        }

        public List<T> GetAll()
        {
            return Items.ToList();
        }

        // Update operation
        public void Update(T item)
        {
            Items.Update(item);
            SaveChanges();
        }

        // Delete operation
        public void Delete(T item)
        {
            Items.Remove(item);
            SaveChanges();
        }
    }
}