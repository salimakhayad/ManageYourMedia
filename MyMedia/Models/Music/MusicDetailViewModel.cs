using MyMedia.Core.MediaClasses;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyMedia.Models.Music
{
    public class MusicDetailViewModel : AuthenticatedViewModel
    {
        public int MediaId { get; set; }
        public string? Titel { get; set; }
        [DisplayName("Foto")]
        public byte[]? Foto { get; set; }
        public string? ZangersNaam { get; set; }
        public List<PlayList>? PlayLists { get; set; }
        public int PlayListId { get; set; }
        public string? Lied { get; set; }
        public double AveragePoints { get; set; }
        public int Points { get; set; }
        public bool IsRated { get; set; }
    }
}

