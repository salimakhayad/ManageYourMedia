using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;

namespace MyMedia.Data
{
    public class MediaDbContext : IdentityDbContext<MediaUser>
    {
        public MediaDbContext(DbContextOptions<MediaDbContext> options)
            : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<Music> Songs { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Season> Seasonen { get; set; }
        public DbSet<PlayList> Playlist { get; set; }
        public DbSet<MediaUser> MediaUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Media>()
            .HasOne(p => p.MediaUser)
            .WithMany(b => b.Media);

            base.OnModelCreating(modelBuilder);
            
    }

    }
}