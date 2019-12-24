using Microsoft.AspNetCore.Http;

namespace MyMedia.Models.Podcast
{
    public class PodcastCreateViewModel
    {
        public string? Titel { get; set; }
        public string? Naam { get; set; }
        public IFormFile? Foto { get; set; }
        public string? PodcastLink { get; set; }
    }
}
