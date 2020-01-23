using MyMedia.Core.Enums;
using MyMedia.Core.MediaClasses;
using System;
using System.Collections.Generic;

namespace MyMedia.Models.Movie
{
    public class MovieListViewModel : AuthenticatedViewModel
    {
        public MovieListViewModel()
        {

        }
        public int Id { get; set; }
        public string? Titel { get; set; }
        public byte[]? Photo { get; set; }
        public TimeSpan? Duration { get; set; }
        public int? Rating { get; set; }
        public string? IMDBLink { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public StatusMovie? Status { get; set; }
        public int? HuidigRating { get; set; }
        public virtual List<Rating>? Ratings { get; set; }

    }
}
