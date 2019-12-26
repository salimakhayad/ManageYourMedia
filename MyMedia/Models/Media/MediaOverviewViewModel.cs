using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MyMedia.Models.Media
{
    public class MediaOverviewViewModel
    {
        [BindProperty]
        public ICollection<MediaListViewModel>? NietPubliekeMediaLijst { get; set; }
    }
}
