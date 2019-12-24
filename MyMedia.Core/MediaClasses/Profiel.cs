using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MyMedia.Core.MediaClasses
{
    public class Profiel:IdentityUser
    {
        //public int Id { get; set; }
        public virtual ICollection<ProfielMedia> Bekeken { get; set; }
        public virtual ICollection<PlayList> Playlists { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public string UserId { get; set; }
        public string Naam { get; set; }
        
    }
}
