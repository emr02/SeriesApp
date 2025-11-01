using System.Text.Json.Serialization;

namespace ClientBlazorAPI.Models
{
    public class NotationDTO
    {
        [JsonPropertyName("nomUtilisateur")]
        public string? NomUtilisateur { get; set; }

        [JsonPropertyName("titreSerie")]
        public string? TitreSerie { get; set; }

        [JsonPropertyName("note")]
        public int Note { get; set; }
    }
}