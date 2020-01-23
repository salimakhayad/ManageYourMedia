using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MyMedia.Models.Media
{
    public class MediaOverviewViewModel
    {
        public List<MediaListViewModel> UnApprovedMediaList { get; set; }
    }
}
