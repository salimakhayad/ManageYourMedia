using MyMedia.Core.MediaClasses;
using System;
using System.Collections.Generic;

namespace MyMedia.Models.Episodes
{
    public class EpisodeDetailViewModel : AuthenticatedViewModel
    {
        public int MediaId { get; set; }
        public string? Titel { get; set; }
        public byte[]? Photo { get; set; }
        public string? Description { get; set; }
        public TimeSpan Duration { get; set; }
        public string? IMDBLink { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int SeasonId { get; set; }
        public int SeasonNr { get; set; }
        public int SerieId { get; set; }
        public string? SerieName { get; set; }
        public List<PlayList>? PlayLists { get; set; }
        public double AveragePoints { get; set; }
        public bool IsRated { get; set; }
        public int Points { get; set; }
        public int PlayListId { get; set; }


    }
}
