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
        public string? Name;
        public byte[]? Photo;
        public List<Season>? Seasons;
        public int AantalLedenReedsGezien;
        public int AantalLedenWillenZien;
    }
}
