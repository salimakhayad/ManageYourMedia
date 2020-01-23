using System.Collections.Generic;
using MyMedia.Core.MediaClasses;

namespace MyMedia.Models.Series
{
    public class SerieDetailViewModel : AuthenticatedViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public byte[]? Photo { get; set; }
        public virtual List<Season>? Seasons { get; set; }

    }
}
