using MyMedia.Core.User;
using System.ComponentModel.DataAnnotations;

namespace MyMedia.Core.MediaClasses
{
    public class ProfileMedia
    {
        [Key]
        public int Id { get; set; }
        public virtual Media Media { get; set; }
        public virtual MediaUser MediaUser { get; set; }

    }
}
