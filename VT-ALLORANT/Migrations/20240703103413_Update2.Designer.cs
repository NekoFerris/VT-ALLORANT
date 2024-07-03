﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VT_ALLORANT.Controller;

#nullable disable

namespace VT_ALLORANT.Migrations
{
    [DbContext(typeof(DBAccess))]
    [Migration("20240703103413_Update2")]
    partial class Update2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

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

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

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

                    b.Property<int>("DiscordUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RankedScore")
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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TournamentId");

                    b.ToTable("Tournament");
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

                    b.HasKey("TeamId", "TournamentId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentTeam", (string)null);
                });

            modelBuilder.Entity("VT_ALLORANT.Model.Valorant.ValorantUser", b =>
                {
                    b.Property<int>("ValorantUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("NAME")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PUUID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TAG")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ValorantUserId");

                    b.ToTable("ValorantUser");
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
