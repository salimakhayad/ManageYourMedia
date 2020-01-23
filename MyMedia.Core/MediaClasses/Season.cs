using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyMedia.Core.MediaClasses
{
    public class Season
    {
        public int Id { get; set; }
        public virtual Serie Serie { get; set; }
        public int SeasonNr { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
    }
}
