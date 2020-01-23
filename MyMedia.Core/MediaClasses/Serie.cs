using MyMedia.Core.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMedia.Core.MediaClasses
{
    public class Serie
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }
        public bool IsPublic { get; set; }
        
    }
}
