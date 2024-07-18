﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VT_ALLORANT.Controller;

#nullable disable

namespace VT_ALLORANT.Migrations
{
    [DbContext(typeof(DBAccess))]
    partial class DBAccessModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("DiscordRole", b =>
                {
                    b.Property<long>("RoleType")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RoleType");

                    b.ToTable("DiscordRoles");

                    b.HasData(
                        new
                        {
                            RoleType = 1L,
                            RoleId = 1ul
                        },
                        new
                        {
                            RoleType = 2L,
                            RoleId = 2ul
                        },
                        new
                        {
                            RoleType = 3L,
                            RoleId = 3ul
                        },
                        new
                        {
                            RoleType = 4L,
                            RoleId = 4ul
                        },
                        new
                        {
                            RoleType = 5L,
                            RoleId = 5ul
                        });
                });

            modelBuilder.Entity("GameObserver", b =>
                {
                    b.Property<int>("ObserverId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ObserverId", "GameId");

                    b.HasIndex("GameId");

                    b.ToTable("GameObserver", (string)null);
                });

            modelBuilder.Entity("RankScore", b =>
                {
                    b.Property<int>("RankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Score")
                        .HasColumnType("INTEGER");

                    b.HasKey("RankId");

                    b.ToTable("RankScores");

                    b.HasData(
                        new
                        {
                            RankId = 1,
                            Score = 1
                        },
                        new
                        {
                            RankId = 2,
                            Score = 2
                        },
                        new
                        {
                            RankId = 3,
                            Score = 3
                        },
                        new
                        {
                            RankId = 4,
                            Score = 4
                        },
                        new
                        {
                            RankId = 5,
                            Score = 5
                        },
                        new
                        {
                            RankId = 6,
                            Score = 6
                        },
                        new
                        {
                            RankId = 7,
                            Score = 7
                        },
                        new
                        {
                            RankId = 8,
                            Score = 8
                        },
                        new
                        {
                            RankId = 9,
                            Score = 9
                        },
                        new
                        {
                            RankId = 10,
                            Score = 10
                        },
                        new
                        {
                            RankId = 11,
                            Score = 11
                        },
                        new
                        {
                            RankId = 12,
                            Score = 12
                        },
                        new
                        {
                            RankId = 13,
                            Score = 13
                        },
                        new
                        {
                            RankId = 14,
                            Score = 14
                        },
                        new
                        {
                            RankId = 15,
                            Score = 15
                        },
                        new
                        {
                            RankId = 16,
                            Score = 16
                        },
                        new
                        {
                            RankId = 17,
                            Score = 17
                        },
                        new
                        {
                            RankId = 18,
                            Score = 18
                        },
                        new
                        {
                            RankId = 19,
                            Score = 19
                        },
                        new
                        {
                            RankId = 20,
                            Score = 20
                        },
                        new
                        {
                            RankId = 21,
                            Score = 21
                        },
                        new
                        {
                            RankId = 22,
                            Score = 22
                        },
                        new
                        {
                            RankId = 23,
                            Score = 23
                        },
                        new
                        {
                            RankId = 24,
                            Score = 24
                        },
                        new
                        {
                            RankId = 25,
                            Score = 25
                        },
                        new
                        {
                            RankId = 26,
                            Score = 26
                        });
                });

            modelBuilder.Entity("TeamPlayer", b =>
                {
                    b.Property<int>("PlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PlayerId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamPlayer", (string)null);
                });

            modelBuilder.Entity("VT_ALLORANT.Model.Discord.DiscordUser", b =>
                {
                    b.Property<int>("DiscordUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("DiscordId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DiscordUserId");

                    b.ToTable("DiscordUser");
                });

            modelBuilder.Entity("VT_ALLORANT.Model.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("MatchId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ModeratorId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Stage")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Team1Id")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Team2Id")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TournamentId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("WinnerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GameId");

                    b.HasIndex("ModeratorId")
                        .IsUnique();

                    b.HasIndex("Team1Id")
                        .IsUnique();

                    b.HasIndex("Team2Id")
                        .IsUnique();

                    b.HasIndex("TournamentId");

                    b.HasIndex("WinnerId")
                        .IsUnique();

                    b.ToTable("Games");
                });

            modelBuilder.Entity("VT_ALLORANT.Model.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanChangeRank")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DiscordUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Rank")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ValorantUserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PlayerId");

                    b.HasIndex("DiscordUserId")
                        .IsUnique();

                    b.HasIndex("ValorantUserId")
                        .IsUnique();

                    b.ToTable("Players");
                });

            modelBuilder.Entity("VT_ALLORANT.Model.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LeaderId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<byte>("MaxPlayers")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TeamId");

                    b.HasIndex("LeaderId")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("VT_ALLORANT.Model.Tournament", b =>
                {
                    b.Property<int>("TournamentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentStage")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxPlayerRank")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxTeamRank")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxTeams")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinPlayerRank")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("OpenForRegistration")
                        .HasColumnType("INTEGER");

                    b.HasKey("TournamentId");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("VT_ALLORANT.Model.TournamentModerator", b =>
                {
                    b.Property<int>("ModeratorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TournamentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ModeratorId", "TournamentId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentModerator", (string)null);
                });

            modelBuilder.Entity("VT_ALLORANT.Model.TournamentObserver", b =>
                {
                    b.Property<int>("ObserverId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TournamentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ObserverId", "TournamentId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentObserver", (string)null);
                });

            modelBuilder.Entity("VT_ALLORANT.Model.TournamentTeam", b =>
                {
                    b.Property<int>("TeamId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TournamentId")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("IsApproved")
                        .HasColumnType("INTEGER");

                    b.HasKey("TeamId", "TournamentId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentTeam", (string)null);
                });

            modelBuilder.Entity("VT_ALLORANT.Model.Valorant.ValorantUser", b =>
                {
                    b.Property<int>("ValorantUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("PUUID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ValorantUserId");

                    b.ToTable("ValorantUser");
                });

            modelBuilder.Entity("GameObserver", b =>
                {
                    b.HasOne("VT_ALLORANT.Model.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VT_ALLORANT.Model.Player", "Observer")
                        .WithMany()
                        .HasForeignKey("ObserverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Observer");
                });

            modelBuilder.Entity("TeamPlayer", b =>
                {
                    b.HasOne("VT_ALLORANT.Model.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VT_ALLORANT.Model.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("VT_ALLORANT.Model.Game", b =>
                {
                    b.HasOne("VT_ALLORANT.Model.Player", "Moderator")
                        .WithOne()
                        .HasForeignKey("VT_ALLORANT.Model.Game", "ModeratorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VT_ALLORANT.Model.Team", "Team1")
                        .WithOne()
                        .HasForeignKey("VT_ALLORANT.Model.Game", "Team1Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VT_ALLORANT.Model.Team", "Team2")
                        .WithOne()
                        .HasForeignKey("VT_ALLORANT.Model.Game", "Team2Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VT_ALLORANT.Model.Tournament", null)
                        .WithMany("Games")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VT_ALLORANT.Model.Team", "Winner")
                        .WithOne()
                        .HasForeignKey("VT_ALLORANT.Model.Game", "WinnerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Moderator");

                    b.Navigation("Team1");

                    b.Navigation("Team2");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("VT_ALLORANT.Model.Player", b =>
                {
                    b.HasOne("VT_ALLORANT.Model.Discord.DiscordUser", "DiscordUser")
                        .WithOne()
                        .HasForeignKey("VT_ALLORANT.Model.Player", "DiscordUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VT_ALLORANT.Model.Valorant.ValorantUser", "ValorantUser")
                        .WithOne()
                        .HasForeignKey("VT_ALLORANT.Model.Player", "ValorantUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiscordUser");

                    b.Navigation("ValorantUser");
                });

            modelBuilder.Entity("VT_ALLORANT.Model.Team", b =>
                {
                    b.HasOne("VT_ALLORANT.Model.Player", "Leader")
                        .WithOne()
                        .HasForeignKey("VT_ALLORANT.Model.Team", "LeaderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Leader");
                });

            modelBuilder.Entity("VT_ALLORANT.Model.TournamentModerator", b =>
                {
                    b.HasOne("VT_ALLORANT.Model.Player", "Moderator")
                        .WithMany()
                        .HasForeignKey("ModeratorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VT_ALLORANT.Model.Tournament", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Moderator");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("VT_ALLORANT.Model.TournamentObserver", b =>
                {
                    b.HasOne("VT_ALLORANT.Model.Player", "Observer")
                        .WithMany()
                        .HasForeignKey("ObserverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VT_ALLORANT.Model.Tournament", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Observer");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("VT_ALLORANT.Model.TournamentTeam", b =>
                {
                    b.HasOne("VT_ALLORANT.Model.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VT_ALLORANT.Model.Tournament", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Team");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("VT_ALLORANT.Model.Tournament", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
