using System.Collections.Generic;
using MyMedia.Core.MediaClasses;
namespace MyMedia.Models.Home
{
    public class HomeOverviewViewModel : AuthenticatedViewModel
    {
        public IEnumerable<Core.MediaClasses.Movie>? Movies { get; set; }
        public IEnumerable<Core.MediaClasses.Muziek>? Musics { get; set; }
        public IEnumerable<Core.MediaClasses.Podcast>? Podcasts { get; set; }
        public IEnumerable<Core.MediaClasses.Serie>? Series { get; set; }
        public IEnumerable<Core.MediaClasses.PlayList>? PlayLists { get; set; }
        public Profiel? Profiel { get; set; }
    }
}
