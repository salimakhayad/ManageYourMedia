using System;
using MyMedia.Core.Enums;

namespace MyMedia.Core.MediaClasses
{
    public class Movie: Media
    {
        public TimeSpan? Duration { get; set; }
        public string IMDBLink { get; set; }
        public DateTime? ReleaseDate { get; set; }
        // public virtual ICollection<Rating> Ratings { get; set; }

        public StatusMovie Status { get; set; }
    }
}
