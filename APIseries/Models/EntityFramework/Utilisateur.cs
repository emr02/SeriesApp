using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIseries.Models.EntityFramework
{
    [Table("t_e_utilisateur_utl")]
    public class Utilisateur
    {
        [Key]
        [Column("utl_id")]
        public int UtilisateurId { get; set; }

        [Required]
        [Column("utl_nom")]
        [StringLength(50, ErrorMessage = "Le nom doit avoir 50 caractères maximum.")]
        [Display(Name = "Nom de famille")]
        public string Nom { get; set; } = null!;

        [Required]
        [Column("utl_prenom")]
        [StringLength(50, ErrorMessage = "Le prénom doit avoir 50 caractères maximum.")]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; } = null!;

        [Column("utl_mobile")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Le numéro de mobile doit commencer par 0 et contenir 10 chiffres.")]
        public string? Mobile { get; set; }

        [Required]
        [Column("utl_mail")]
        [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La longueur d’un email doit être comprise entre 6 et 100 caractères.")]
        public string Mail { get; set; } = null!;

        [Required]
        [Column("utl_pwd")]
        [StringLength(80, MinimumLength = 6, ErrorMessage = "Le mot de passe doit avoir entre 6 et 80 caractères.")]
        [Display(Name = "Mot de passe")]
        public string Pwd { get; set; } = null!;

        [Column("utl_rue")]
        [StringLength(100)]
        public string? Rue { get; set; }

        [Column("utl_cp")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Le code postal doit comporter 5 chiffres.")]
        public string? CodePostal { get; set; }

        [Column("utl_ville")]
        [StringLength(50)]
        public string? Ville { get; set; }

        [Column("utl_pays")]
        [StringLength(50)]
        public string? Pays { get; set; }

        [Column("utl_latitude")]
        public float? Latitude { get; set; }

        [Column("utl_longitude")]
        public float? Longitude { get; set; }

        [Column("utl_datecreation")]
        public DateTime? DateCreation { get; set; }

        // Navigation properties (sans lazy loading, donc pas de 'virtual')
        public ICollection<Notation> NotesUtilisateur { get; set; } = new List<Notation>();
    }
}