using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MyMedia.Core.MediaClasses;

namespace MyMedia.Models.Series
{
    public class SerieEditViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IFormFile? Photo { get; set; }
        public virtual List<Season>? Seasons { get; set; }

    }
}
