using Microsoft.AspNetCore.Identity;
using MyMedia.Core.MediaClasses;
using System;
using System.Collections.Generic;

namespace MyMedia.Core.User
{
    public class Profiel :IdentityUser
    {
        
        public virtual ICollection<ProfielMedia> Bekeken { get; set; }
        public virtual ICollection<PlayList> Playlists { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public string FavorieteKleur { get; set; }


    }
}
