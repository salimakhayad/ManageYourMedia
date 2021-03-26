using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MyMedia.Models.Series
{
    public class SerieCreateViewModel
    {
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Name { get; set; }
        [Required]
        public IFormFile? Photo { get; set; }
    }
}
