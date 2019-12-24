using System.Collections.Generic;
using MyMedia.Core.MediaClasses;

namespace MyMedia.Models.Series
{
    public class SerieDetailViewModel : AuthenticatedViewModel
    {
        public int Id { get; set; }
        public string? Naam { get; set; }
        public byte[]? Foto { get; set; }
        public virtual List<Seizoen>? Seizoenen { get; set; }

    }
}
