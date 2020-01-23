using Microsoft.AspNetCore.Identity;
using MyMedia.Core.MediaClasses;
using System;
using System.Collections.Generic;

namespace MyMedia.Core.User
{
    public class MediaUser : IdentityUser
    {
        
        public virtual ICollection<ProfileMedia> Bekeken { get; set; }
        public virtual ICollection<PlayList> Playlists { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public string Role { get; set; }
        public byte[] ProfilePicture { get; set; }
        public virtual ICollection<Media> Media { get; set; }


    }
}
