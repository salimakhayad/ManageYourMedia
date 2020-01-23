using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MyMedia.Models.Podcast
{
    public class PodcastCreateViewModel
    {
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Titel { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public IFormFile? Photo { get; set; }
        [Required]
        public string? PodcastLink { get; set; }
    }
}
