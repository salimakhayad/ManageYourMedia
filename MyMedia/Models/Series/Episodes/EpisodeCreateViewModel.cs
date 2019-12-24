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
        public string? SerieNaam { get; set; }
        public int Id { get; set; }
        public TimeSpan Duration { get; set; }
        public string? IMDBLink { get; set; }
        public string? Beschrijving { get; set; }
        public DateTime ReleaseDate { get; set; }
        [BindProperty]
        public int SeizoenId { get; set; }
        public IFormFile? Foto { get; set; }
        public string? Titel { get; set; }
        public List<Seizoen>? MogelijkeSeizoenen { get; set; }

    }
}
