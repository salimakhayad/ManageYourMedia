using System.Collections.Generic;
using MyMedia.Core.MediaClasses;

namespace MyMedia.Models.Playlist
{
    public class PlayListEditViewModel
    {
        public int Id { get; set; }
        public List<Core.MediaClasses.Media>? PlayList { get; set; }
    }
}
