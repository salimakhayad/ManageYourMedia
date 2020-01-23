using MyMedia.Core.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMedia.Core.MediaClasses
{
    public class Media
    {
        [Key]
        public int Id { get; set; }
        public string MediaUserId { get; set; }
        public virtual MediaUser MediaUser { get; set; }

        public string Titel { get; set; }
        public byte[] Photo { get; set; }
        public bool IsPublic { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }


    }
}
