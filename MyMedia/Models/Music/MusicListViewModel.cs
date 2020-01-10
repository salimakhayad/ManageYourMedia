namespace MyMedia.Models.Music
{
    public class MusicListViewModel : AuthenticatedViewModel
    {
        public int Id { get; set; }
        public string? Titel { get; set; }
        public string? ZangersNaam { get; set; }
        public byte[]? Foto { get; set; }

    }
}
