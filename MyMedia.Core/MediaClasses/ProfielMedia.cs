using MyMedia.Core.User;
using System.ComponentModel.DataAnnotations;

namespace MyMedia.Core.MediaClasses
{
    public class ProfielMedia
    {
        [Key]
        public int Id { get; set; }
        public virtual Media Media { get; set; }
        public virtual Profiel Profiel { get; set; }

    }
}
