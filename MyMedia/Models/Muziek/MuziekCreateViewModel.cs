using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MyMedia.Models.Muziek
{
    public class MuziekCreateViewModel
    {
        [Required]
        [DisplayName("Titel")]
        public string? Titel { get; set; }
        [DisplayName("Foto")]
        [Required]
        public IFormFile? Foto { get; set; }
        [Required]
        [DisplayName("ZangersNaam")]
        public string? ZangersNaam { get; set; }
        [Required]
        [DisplayName("Lied")]
        public string? Lied { get; set; }
       
    }
}
