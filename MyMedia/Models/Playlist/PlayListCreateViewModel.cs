using System.ComponentModel.DataAnnotations;

namespace MyMedia.Models.Playlist
{
    public class PlayListCreateViewModel
    {
        public string UserName { get; set; }
        [Required]
        public string? Naam { get; set; }

    }

}
