using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMedia.Models.Playlist
{
    public class PlayListDetailViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public List<Core.MediaClasses.Media>? MediaList { get; set; }


    }
}
