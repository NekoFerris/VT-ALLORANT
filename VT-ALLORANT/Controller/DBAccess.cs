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
        public DbSet<TournamentObserver> TournamentObservers { get; set; }
        public DbSet<TournamentModerator> TournamentModerators { get; set; }
        public DbSet<TournamentTeam> TournamentTeams { get; set; }
        public DbSet<GameObserver> GameObservers { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }


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
                        .HasForeignKey(tp => tp.TeamId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne(tp => tp.Player)
                        .WithMany()
                        .HasForeignKey(tp => tp.PlayerId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
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
                        .HasForeignKey(tp => tp.PlayerId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne(tp => tp.Team)
                        .WithMany()
                        .HasForeignKey(tp => tp.TeamId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
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

            modelBuilder.Entity<Game>()
                .HasOne(g => g.Team1)
                .WithOne()
                .HasForeignKey<Game>(g => g.Team1Id)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.Team2)
                .WithOne()
                .HasForeignKey<Game>(g => g.Team2Id)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.Winner)
                .WithOne()
                .HasForeignKey<Game>(g => g.WinnerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.Moderator)
                .WithOne()
                .HasForeignKey<Game>(g => g.ModeratorId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.Games)
                .WithOne()
                .HasForeignKey(g => g.TournamentId);

            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.Observers)
                .WithMany(p => p.Tournaments)
                .UsingEntity<TournamentObserver>(
                    j => j.HasOne(tp => tp.Observer)
                        .WithMany()
                        .HasForeignKey(tp => tp.ObserverId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne(tp => tp.Tournament)
                        .WithMany()
                        .HasForeignKey(tp => tp.TournamentId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey(tp => new { tp.ObserverId, tp.TournamentId });
                        j.ToTable("TournamentObserver");
                    }
                );

            modelBuilder.Entity<Player>()
                .HasMany(t => t.Tournaments)
                .WithMany(p => p.Observers)
                .UsingEntity<TournamentObserver>(
                    j => j.HasOne(tp => tp.Tournament)
                        .WithMany()
                        .HasForeignKey(tp => tp.TournamentId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne(tp => tp.Observer)
                        .WithMany()
                        .HasForeignKey(tp => tp.ObserverId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey(tp => new { tp.ObserverId, tp.TournamentId });
                        j.ToTable("TournamentObserver");
                    }
                );
            
            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.Moderators)
                .WithMany(p => p.Tournaments)
                .UsingEntity<TournamentModerator>(
                    j => j.HasOne(tp => tp.Moderator)
                        .WithMany()
                        .HasForeignKey(tp => tp.ModeratorId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne(tp => tp.Tournament)
                        .WithMany()
                        .HasForeignKey(tp => tp.TournamentId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey(tp => new { tp.ModeratorId, tp.TournamentId });
                        j.ToTable("TournamentModerator");
                    }
                );

            modelBuilder.Entity<Player>()
                .HasMany(t => t.Tournaments)
                .WithMany(p => p.Observers)
                .UsingEntity<TournamentModerator>(
                    j => j.HasOne(tp => tp.Tournament)
                        .WithMany()
                        .HasForeignKey(tp => tp.TournamentId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne(tp => tp.Moderator)
                        .WithMany()
                        .HasForeignKey(tp => tp.ModeratorId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey(tp => new { tp.ModeratorId, tp.TournamentId });
                        j.ToTable("TournamentModerator");
                    }
                );

            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.Teams)
                .WithMany(p => p.Tournaments)
                .UsingEntity<TournamentTeam>(
                    j => j.HasOne(tp => tp.Team)
                        .WithMany()
                        .HasForeignKey(tp => tp.TeamId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne(tp => tp.Tournament)
                        .WithMany()
                        .HasForeignKey(tp => tp.TournamentId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey(tp => new { tp.TeamId, tp.TournamentId });
                        j.ToTable("TournamentTeam");
                    }
                );

            modelBuilder.Entity<Team>()
                .HasMany(t => t.Tournaments)
                .WithMany(p => p.Teams)
                .UsingEntity<TournamentTeam>(
                    j => j.HasOne(tp => tp.Tournament)
                        .WithMany()
                        .HasForeignKey(tp => tp.TournamentId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne(tp => tp.Team)
                        .WithMany()
                        .HasForeignKey(tp => tp.TeamId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey(tp => new { tp.TeamId, tp.TournamentId });
                        j.ToTable("TournamentTeam");
                    }
                );

            modelBuilder.Entity<Game>()
            .HasMany(t => t.Observers)
            .WithMany(p => p.ObserverInGames)
            .UsingEntity<GameObserver>(
                j => j.HasOne(tp => tp.Observer)
                    .WithMany()
                    .HasForeignKey(tp => tp.ObserverId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne(tp => tp.Game)
                    .WithMany()
                    .HasForeignKey(tp => tp.GameId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.HasKey(tp => new { tp.ObserverId, tp.GameId });
                    j.ToTable("TournamentTeam");
                }
            );

            modelBuilder.Entity<Player>()
            .HasMany(t => t.ObserverInGames)
            .WithMany(p => p.Observers)
            .UsingEntity<GameObserver>(
                j => j.HasOne(tp => tp.Game)
                    .WithMany()
                    .HasForeignKey(tp => tp.GameId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne(tp => tp.Observer)
                    .WithMany()
                    .HasForeignKey(tp => tp.ObserverId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.HasKey(tp => new { tp.ObserverId, tp.GameId });
                    j.ToTable("GameObserver");
                }
            );
        }
    }
}