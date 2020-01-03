using MyMedia.Core.User;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyMedia.Core.MediaClasses
{
    public class Rating
    {
        public int Id { get; set; }
       
        public virtual Media Media { get; set; }
      
        public int Points { get; set; }
     
        public DateTime CreationDate { get; set; }
     
        public virtual Profiel Profiel { get; set; }
    }
}
