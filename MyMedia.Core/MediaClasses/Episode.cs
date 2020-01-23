using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace MyMedia.Core.MediaClasses
{
    public class Episode : Media
    {
        public TimeSpan Duration { get; set; }
        public string IMDBLink { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        
        public virtual Season Season { get; set; }

    }
}
