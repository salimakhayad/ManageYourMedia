namespace MyMedia.Models.Playlist
{
    public class PlayListIndexViewModel : AuthenticatedViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
    }
}
