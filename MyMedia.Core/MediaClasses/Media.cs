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
        public string Titel { get; set; }
        public byte[] Foto { get; set; }
        public bool IsPubliek { get; set; }
        public virtual ICollection<ProfielMedia> Bekeken { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }


    }
}
