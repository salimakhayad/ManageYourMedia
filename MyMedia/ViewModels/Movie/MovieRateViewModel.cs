
using System.ComponentModel.DataAnnotations;

namespace MyMedia.Controllers
{
    public class MovieRateViewModel
    {
        public int Points { get; set; }
        public string? Review { get; set; }
        public int MediaId { get; set; }

    }
}