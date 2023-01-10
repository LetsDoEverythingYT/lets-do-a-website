﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using lets_do_a_website.Data;

#nullable disable

namespace lets_do_a_website.Migrations
{
    [DbContext(typeof(WTDContext))]
    [Migration("20230107030120_newstuff")]
    partial class newstuff
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

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

                    b.Property<string>("Deaths")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Streamer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Submitted")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("RunStats");
                });

            modelBuilder.Entity("lets_do_a_website.Data.Entities.Tracker", b =>
                {
                    b.Property<int>("key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Id")
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

                    b.Property<bool>("DeleteOnDeath")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("OverlayDeleteOnDeath")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ProfileImage")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("UserSettings");
                });
#pragma warning restore 612, 618
        }
    }
}