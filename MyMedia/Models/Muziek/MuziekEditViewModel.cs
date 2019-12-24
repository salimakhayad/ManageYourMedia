namespace MyMedia.Models.Muziek
{
    public class MuziekEditViewModel
    {
        public int Id { get; set; }
        public string? Naam { get; set; }
        public string? ZangersNaam { get; set; }
        public string? Lied { get; set; }
        public string? Titel { get; set; }
        public byte[]? Foto { get; set; }
    }
}
