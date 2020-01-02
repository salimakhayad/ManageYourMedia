using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;

namespace MyMedia.Data
{
    public class MediaDbContext : IdentityDbContext<Profiel>
    {
        public MediaDbContext(DbContextOptions<MediaDbContext> options)
            : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Podcast> Podcast { get; set; }
        public DbSet<Muziek> Muziek { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Seizoen> Seizoenen { get; set; }
        public DbSet<PlayList> Playlist { get; set; }
        public DbSet<Profiel> Profiel { get; set; }
        public DbSet<ProfielMedia> Bekeken { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Profiel>(prof => prof.HasIndex(x => x.FavorieteKleur).IsUnique(false));


        }

    }
}