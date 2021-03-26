using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Core.MediaClasses;
using System;
using System.Collections.Generic;

namespace MyMedia.Models.Series.Episodes
{
    public class EpisodeCreateViewModel
    {
        public int SerieId { get; set; }
        public string? SerieName { get; set; }
        public int Id { get; set; }
        public string? MediaUserId { get; set; }
        public virtual Core.User.MediaUser? MediaUser { get; set; }
        public TimeSpan Duration { get; set; }
        public string? IMDBLink { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        [BindProperty]
        public int SeasonId { get; set; }
        public IFormFile? Photo { get; set; }
        public string? Titel { get; set; }
        public List<Season>? PossibleSeasons { get; set; }

    }
}
