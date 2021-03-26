using System;
using Microsoft.AspNetCore.Http;
using MyMedia.Core.Enums;

namespace MyMedia.Models.Movie
{
    public class MovieEditViewModel
    {
        public int Id { get; set; }
        public string? Titel { get; set; }
        public IFormFile? Photo { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public StatusMovie Status { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? IMDBLink { get; set; }
    }
}
