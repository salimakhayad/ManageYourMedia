﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyMedia.Data;

namespace MyMedia.Migrations
{
    [DbContext(typeof(MediaDbContext))]
    [Migration("20191228130645_ProfielEdit")]
    partial class ProfielEdit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.Media", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Foto")
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("IsPubliek")
                        .HasColumnType("bit");

                    b.Property<int?>("PlayListId")
                        .HasColumnType("int");

                    b.Property<string>("Titel")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PlayListId");

                    b.ToTable("Media");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Media");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.PlayList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfielId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProfielId");

                    b.ToTable("Playlist");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.ProfielMedia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MediaId")
                        .HasColumnType("int");

                    b.Property<string>("ProfielId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("MediaId");

                    b.HasIndex("ProfielId");

                    b.ToTable("Bekeken");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MediaId")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("ProfielId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("MediaId");

                    b.HasIndex("ProfielId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.Seizoen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("SeizoenNr")
                        .HasColumnType("int");

                    b.Property<int?>("SerieId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SerieId");

                    b.ToTable("Seizoenen");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.Serie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Foto")
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("IsPubliek")
                        .HasColumnType("bit");

                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("MyMedia.Core.User.Profiel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FavorieteKleur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.Episode", b =>
                {
                    b.HasBaseType("MyMedia.Core.MediaClasses.Media");

                    b.Property<string>("Beschrijving")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("IMDBLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SeizoenId")
                        .HasColumnType("int");

                    b.HasIndex("SeizoenId");

                    b.HasDiscriminator().HasValue("Episode");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.Movie", b =>
                {
                    b.HasBaseType("MyMedia.Core.MediaClasses.Media");

                    b.Property<TimeSpan?>("Duration")
                        .HasColumnName("Movie_Duration")
                        .HasColumnType("time");

                    b.Property<string>("IMDBLink")
                        .HasColumnName("Movie_IMDBLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReleaseDate")
                        .HasColumnName("Movie_ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Movie");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.Muziek", b =>
                {
                    b.HasBaseType("MyMedia.Core.MediaClasses.Media");

                    b.Property<string>("Lied")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZangersNaam")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Muziek");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.Podcast", b =>
                {
                    b.HasBaseType("MyMedia.Core.MediaClasses.Media");

                    b.Property<string>("ConversationMP3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Podcast");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MyMedia.Core.User.Profiel", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MyMedia.Core.User.Profiel", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyMedia.Core.User.Profiel", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MyMedia.Core.User.Profiel", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.Media", b =>
                {
                    b.HasOne("MyMedia.Core.MediaClasses.PlayList", null)
                        .WithMany("MediaList")
                        .HasForeignKey("PlayListId");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.PlayList", b =>
                {
                    b.HasOne("MyMedia.Core.User.Profiel", "Profiel")
                        .WithMany("Playlists")
                        .HasForeignKey("ProfielId");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.ProfielMedia", b =>
                {
                    b.HasOne("MyMedia.Core.MediaClasses.Media", "Media")
                        .WithMany("Bekeken")
                        .HasForeignKey("MediaId");

                    b.HasOne("MyMedia.Core.User.Profiel", "Profiel")
                        .WithMany("Bekeken")
                        .HasForeignKey("ProfielId");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.Rating", b =>
                {
                    b.HasOne("MyMedia.Core.MediaClasses.Media", "Media")
                        .WithMany("Ratings")
                        .HasForeignKey("MediaId");

                    b.HasOne("MyMedia.Core.User.Profiel", "Profiel")
                        .WithMany("Ratings")
                        .HasForeignKey("ProfielId");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.Seizoen", b =>
                {
                    b.HasOne("MyMedia.Core.MediaClasses.Serie", "Serie")
                        .WithMany("Seizoenen")
                        .HasForeignKey("SerieId");
                });

            modelBuilder.Entity("MyMedia.Core.MediaClasses.Episode", b =>
                {
                    b.HasOne("MyMedia.Core.MediaClasses.Seizoen", "Seizoen")
                        .WithMany("Episodes")
                        .HasForeignKey("SeizoenId");
                });
#pragma warning restore 612, 618
        }
    }
}
