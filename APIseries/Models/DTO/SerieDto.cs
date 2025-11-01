namespace APIseries.Models.DTO
{
    public class SerieDTO
    {
        public int Id { get; set; }
        public string? Titre { get; set; }
        public int? NbSaisons { get; set; }
        public int? NbEpisodes { get; set; }
        public double NoteMoyenne { get; set; }
    }
}