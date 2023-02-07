﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using lets_do_a_website.Data;

#nullable disable

namespace lets_do_a_website.Migrations
{
    [DbContext(typeof(WTDContext))]
    partial class WTDContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("lets_do_a_website.Data.Entities.Invite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("GuestStreamer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("HostStreamer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("MatchId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("lets_do_a_website.Data.Entities.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Streamer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("lets_do_a_website.Data.Entities.MatchEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("MatchId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProfileImage")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("MatchEntry");
                });

            modelBuilder.Entity("lets_do_a_website.Data.Entities.Permissions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Mod")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Streamer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("lets_do_a_website.Data.Entities.RunStats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DeathCount")
                        .HasColumnType("int");

                    b.Property<string>("Deaths")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Streamer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("RunStats");
                });

            modelBuilder.Entity("lets_do_a_website.Data.Entities.Tracker", b =>
                {
                    b.Property<int>("key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DataBits")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FirstUsed")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LastUsed")
                        .HasColumnType("datetime(6)");

                    b.HasKey("key");

                    b.ToTable("Trackers");
                });

            modelBuilder.Entity("lets_do_a_website.Data.Entities.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("DarkMode")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MatchId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("OverlayMatchScores")
                        .HasColumnType("int");

                    b.Property<int>("OverlayOnDeath")
                        .HasColumnType("int");

                    b.Property<string>("ProfileImage")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TrackerOnDeath")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("lets_do_a_website.Data.Entities.MatchEntry", b =>
                {
                    b.HasOne("lets_do_a_website.Data.Entities.Match", null)
                        .WithMany("Entries")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("lets_do_a_website.Data.Entities.Match", b =>
                {
                    b.Navigation("Entries");
                });
#pragma warning restore 612, 618
        }
    }
}
