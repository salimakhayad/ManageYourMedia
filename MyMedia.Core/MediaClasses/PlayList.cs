using MyMedia.Core.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMedia.Core.MediaClasses
{
    public class PlayList
    {
        public int Id { get; set; }
        public virtual Profiel Profiel { get; set; }
        public virtual ICollection<Media> MediaList { get; set; }
        public string Name { get; set; }
        

    }
}
