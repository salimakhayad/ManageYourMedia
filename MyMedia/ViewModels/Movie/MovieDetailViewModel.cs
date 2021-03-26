using Microsoft.AspNetCore.Mvc;
using MyMedia.Core.Enums;
using MyMedia.Core.MediaClasses;
using System;
using System.Collections.Generic;

namespace MyMedia.Models.Movie
{
    public class MovieDetailViewModel: AuthenticatedViewModel
    {
        public int MediaId { get; set; }
        public string? Titel { get; set; }
        public byte[]? Photo { get; set; }
        public List<string>? Reviews { get; set; }
        public string? AddedByUserName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public StatusMovie Status { get; set; }
        public TimeSpan? Duration { get; set; }
        public List<Rating> Ratings { get; set; }
        public string? IMDBLink { get; set; }
        public List<PlayList>? PlayLists { get; set; }
        public double AveragePoints { get; set; }
        public bool IsRated { get; set; }
       
        public int? Points { get; set; }
        
        public string? Review { get; set; }
        public int PlayListId { get; set; }
    }
}
