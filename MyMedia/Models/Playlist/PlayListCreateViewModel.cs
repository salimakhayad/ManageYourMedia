using System.ComponentModel.DataAnnotations;

namespace MyMedia.Models.Playlist
{
    public class PlayListCreateViewModel
    {
        [Required]
        public string? Naam { get; set; }

    }

}
