using System.ComponentModel.DataAnnotations.Schema;

namespace MyMedia.Core.MediaClasses
{
    public class UserMovieStatus
    {
      public int Id { get; set; }
        //[ForeignKey("Movie")]
        //public int MediaId { get; set; }
        //public Movie Movie { get; set; }
        //[ForeignKey("User")]
        //public string UserId { get; set; }
        //public StatusUserMovie Status { get; set; }
    }
}
