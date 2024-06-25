using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using VT_ALLORANT.Model;
using VT_ALLORANT.Model.Discord;
using VT_ALLORANT.Model.Valorant;

namespace VT_ALLORANT.Controller
{
    public class DBAccess : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<DiscordUser> DiscordUsers { get; set; }
        public DbSet<ValorantUser> ValorantUsers { get; set; }
        public DbSet<TeamPlayer> TeamPlayers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
            {
                builder.AddConsole().SetMinimumLevel(LogLevel.Debug);
            }));
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
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.ValorantUser)
                .WithOne()
                .HasForeignKey<Player>(p => p.ValorantUserId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Player>()
                .HasMany(p => p.Teams)
                .WithMany(t => t.Players)
                .UsingEntity<TeamPlayer>(
                    j => j.HasOne(tp => tp.Team)
                        .WithMany()
                        .HasForeignKey(tp => tp.TeamId),
                    j => j.HasOne(tp => tp.Player)
                        .WithMany()
                        .HasForeignKey(tp => tp.PlayerId),
                    j =>
                    {
                        j.HasKey(tp => new { tp.PlayerId, tp.TeamId });
                        j.ToTable("TeamPlayer");
                    }
                );

            modelBuilder.Entity<Team>()
                .HasMany(t => t.Players)
                .WithMany(p => p.Teams)
                .UsingEntity<TeamPlayer>(
                    j => j.HasOne(tp => tp.Player)
                        .WithMany()
                        .HasForeignKey(tp => tp.PlayerId),
                    j => j.HasOne(tp => tp.Team)
                        .WithMany()
                        .HasForeignKey(tp => tp.TeamId),
                    j =>
                    {
                        j.HasKey(tp => new { tp.PlayerId, tp.TeamId });
                        j.ToTable("TeamPlayer");
                    }
                );

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Leader)
                .WithOne()
                .HasForeignKey<Team>(t => t.LeaderId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasOne(e => e.Leader)
                .WithOne()
                .HasForeignKey<Team>(e => e.LeaderId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}