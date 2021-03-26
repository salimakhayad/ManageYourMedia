using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MyMedia.Core.Enums;

namespace MyMedia.Models.Movie
{
    public class MovieCreateViewModel
    {
        [StringLength(60, MinimumLength = 3)]
        [Required]
        [DisplayName("Titel")]
        public string? Titel { get; set; }
        [DisplayName("Photo")]
        [Required]
        public IFormFile? Photo { get; set; }
        [DisplayName("Duration")]
        [Required]
        public TimeSpan Duration { get; set; }

        [DisplayName("ReleaseDate")]
        [Required]
        public DateTime? ReleaseDate { get; set; }
        [DisplayName("IMDBLink")]
        [Required]
        public string? IMDBLink { get; set; }
        [DisplayName("Status")]
        [Required]
        public StatusMovie Status { get; set; } 




    }
}
