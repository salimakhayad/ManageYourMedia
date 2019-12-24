using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace MyMedia.Models.Muziek
{
    public class MuziekCreateViewModel
    {
        public string? Titel { get; set; }
        [DisplayName("Foto")]
        public IFormFile? Foto { get; set; }
        public string? ZangersNaam { get; set; }
        public string? Lied { get; set; }
       
    }
}
