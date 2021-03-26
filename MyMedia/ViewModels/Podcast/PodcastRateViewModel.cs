using System.ComponentModel.DataAnnotations;

namespace MyMedia.Controllers
{
    public class PodcastRateViewModel
    {
        [Range(0.0, 10, ErrorMessage = "{0} must be a decimal/number between {1} and {10}.")]
        public int Points { get; set; }
        public int MediaId { get; set; }
    }
}