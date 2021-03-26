using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MyMedia.Models.Music
{
    public class MusicCreateViewModel
    {
        [Required]
        [DisplayName("Titel")]
        public string? Titel { get; set; }
        [DisplayName("Photo")]
        [Required]
        public IFormFile? Photo { get; set; }
        [Required]
        [DisplayName("ZangersName")]
        public string? ZangersName { get; set; }
        [Required]
        [DisplayName("Lied")]
        public string? Lied { get; set; }
       
    }
}
