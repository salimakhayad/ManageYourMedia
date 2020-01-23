using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMedia.Models.Series.Episodes
{
    public class EpisodeRateViewModel
    {
        [Range(0.0, 10, ErrorMessage = "{0} must be a decimal/number between {1} and {10}.")]
        public int Points { get; set; }
        public int MediaId { get; set; }
    }
}
