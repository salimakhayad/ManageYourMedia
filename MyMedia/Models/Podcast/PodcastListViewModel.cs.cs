namespace MyMedia.Models.Podcast
{
    public class PodcastListViewModel : AuthenticatedViewModel
    {
        public int Id { get; set; }
        public string? Naam { get; set; }
        public byte[]? Foto { get; set; }
        public string? PodcastLink { get; set; }
    }
}
