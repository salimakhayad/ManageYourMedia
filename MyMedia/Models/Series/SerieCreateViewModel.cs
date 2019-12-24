using Microsoft.AspNetCore.Http;

namespace MyMedia.Models.Series
{
    public class SerieCreateViewModel
    {
        public string? Naam { get; set; }
        public IFormFile? Foto { get; set; }
    }
}
