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
        [DisplayName("Foto")]
        public string? Titel { get; set; }
        [DisplayName("Foto")]
        public IFormFile? Foto { get; set; }
        [DisplayName("Duration")]
        public TimeSpan Duration { get; set; }
        [DisplayName("ReleaseDate")]
        public DateTime? ReleaseDate { get; set; }
        [DisplayName("IMDBLink")]
        public string? IMDBLink { get; set; }
        [DisplayName("Status")]
        public StatusMovie Status { get; set; } 




    }
}
