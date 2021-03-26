namespace MyMedia.Models.Music
{
    public class MusicEditViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ZangersName { get; set; }
        public string? Lied { get; set; }
        public string? Titel { get; set; }
        public byte[]? Photo { get; set; }
    }
}
