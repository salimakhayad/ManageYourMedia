using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMedia.Core.MediaClasses
{
    public class Serie
    {
        
        public int Id { get; set; }
        public string Naam { get; set; }
        public byte[] Foto { get; set; }
        public virtual ICollection<Seizoen> Seizoenen { get; set; }
        public bool IsPubliek { get; set; }
        
    }
}
