using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MyMedia.Core.MediaClasses;

namespace MyMedia.Models.Series
{
    public class SerieEditViewModel
    {
        public int Id { get; set; }
        public string? Naam { get; set; }
        public IFormFile? Foto { get; set; }
        public virtual List<Seizoen>? Seizoenen { get; set; }

    }
}
