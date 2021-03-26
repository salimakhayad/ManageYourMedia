namespace MyMedia.Models.Music
{
    public class MusicListViewModel : AuthenticatedViewModel
    {
        public int Id { get; set; }
        public string? Titel { get; set; }
        public string? ZangersName { get; set; }
        public byte[]? Photo { get; set; }

    }
}
