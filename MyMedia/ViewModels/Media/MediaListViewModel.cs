using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMedia.Models.Media
{
    public class MediaListViewModel
    {
        public int Id { get; set; }
        public string? Titel { get; set; }
        public byte[]? Photo { get; set; }
        public bool IsChecked { get; set; }
        public string? AddedByMediaUserName { get; set; }

    }
}
