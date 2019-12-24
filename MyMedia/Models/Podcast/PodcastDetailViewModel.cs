using System.Collections.Generic;
using MyMedia.Core.MediaClasses;

namespace MyMedia.Models.Podcast
{
    public class PodcastDetailViewModel : AuthenticatedViewModel
    {
        public int Id { get; set; }
        public int MediaId { get; set; }
        public string? Naam { get; set; }
        public string? PodcastLink { get; set; } 
        public byte[]? Foto { get; set; }
        public double AveragePoints { get; set; }
        public bool IsRated { get; set; }
        public int Points { get; set; }
        public int PlayListId { get; set; }
        public List<PlayList>? PlayLists { get; set; }



    }
}
