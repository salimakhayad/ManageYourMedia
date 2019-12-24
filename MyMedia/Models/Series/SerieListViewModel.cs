using MyMedia.Core.MediaClasses;
using System.Collections.Generic;

namespace MyMedia.Models.Series
{
    public class SerieListViewModel : AuthenticatedViewModel
    {
        public SerieListViewModel()
        {

        }
        public int? Id;
        public string? Naam;
        public byte[]? Foto;
        public List<Seizoen>? Seizoenen;
        public int AantalLedenReedsGezien;
        public int AantalLedenWillenZien;
    }
}
