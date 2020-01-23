using System;
using System.Collections.Generic;
using MyMedia.Core.MediaClasses;

namespace MyMedia.Models.Movie
{
    public class MovieReviewModel
    {
        public string? Titel { get; set; }
        public string? IMDBLink { get; set; }
        public byte[]? Photo { get; set; }
        public TimeSpan Duration { get; set; }
        public virtual List<Rating>? Reviews {get;set; }



    }
}
