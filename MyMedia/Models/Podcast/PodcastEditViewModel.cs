using Microsoft.AspNetCore.Http;

namespace MyMedia.Models.Podcast
{
    public class PodcastEditViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IFormFile? Photo { get; set; }
        public string? PodcastLink { get; set; }

    }
}
