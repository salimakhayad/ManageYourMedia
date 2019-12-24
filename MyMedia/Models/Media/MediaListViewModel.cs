using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMedia.Models.Media
{
    public class MediaListViewModel
    {
        public string? Titel { get; set; }
        public byte[]? Foto { get; set; }
        public bool IsChecked { get; set; }
      
    }
}
