using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMedia.Core.MediaClasses
{
    public class PlayList
    {
        public int Id { get; set; }
        public virtual Profiel Profiel { get; set; }
        public virtual ICollection<Media> MediaList { get; set; }

        [NotMapped]
        public virtual List<Profiel> ProfielIds { get; set; }
        public string Name { get; set; }
        public bool IsPubliek { get; set; }

    }
}
