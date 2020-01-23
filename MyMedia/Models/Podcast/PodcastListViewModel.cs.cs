namespace MyMedia.Models.Podcast
{
    public class PodcastListViewModel : AuthenticatedViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public byte[]? Photo { get; set; }
        public string? PodcastLink { get; set; }
    }
}
