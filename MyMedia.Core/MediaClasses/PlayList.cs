using System.Collections.Generic;

namespace MyMedia.Core.MediaClasses
{
    public class PlayList
    {
        public int Id { get; set; }
        public virtual Profiel Profiel { get; set; }
        public virtual ICollection<Media> MediaList { get; set; }
        public string Name { get; set; }
        public bool IsPubliek { get; set; }

    }
}
