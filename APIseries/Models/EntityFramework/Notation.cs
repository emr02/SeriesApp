using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIseries.Models.EntityFramework
{
    [Table("t_j_notation_not")]
    public class Notation
    {
        [Key, Column("utl_id", Order = 0)]
        public int UtilisateurId { get; set; }

        [Key, Column("ser_id", Order = 1)]
        public int SerieId { get; set; }

        [Range(0, 5)]
        [Column("not_note")]
        public int Note { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UtilisateurId))]
        [JsonIgnore]
        public virtual Utilisateur UtilisateurNotant { get; set; }

        [ForeignKey(nameof(SerieId))]
        [JsonIgnore]
        public virtual Serie SerieNotee { get; set; }
    }
}
