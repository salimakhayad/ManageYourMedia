namespace MyMedia.Models.Playlist
{
    public class PlayListIndexViewModel : AuthenticatedViewModel
    {
        public int Id { get; set; }
        public string? Naam { get; set; }
        public string? UserNaam { get; set; }
    }
}
