using MyMedia.Core.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMedia.Core.MediaClasses
{
    public class PlayList
    {
        public int Id { get; set; }
        public string MediaUserId { get; set; }
        public virtual MediaUser MediaUser { get; set; }
        public virtual ICollection<Media> MediaList { get; set; }
        public string Name { get; set; }
        

    }
}
