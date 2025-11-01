namespace APIseries.Models.DTO
{
    public class SerieDetailDTO
    {
        public int Id { get; set; }
        public string? Titre { get; set; }
        public string? Resume { get; set; }
        public int? NbSaisons { get; set; }
        public int? NbEpisodes { get; set; }
        public int? AnneeCreation { get; set; }
        public string? Network { get; set; }
        public double NoteMoyenne { get; set; }
    }
}