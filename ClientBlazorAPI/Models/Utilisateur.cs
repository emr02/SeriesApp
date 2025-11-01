using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClientBlazorAPI.Models;

public class Utilisateur
{
    [JsonPropertyName("utilisateurId")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est requis.")]
    [MaxLength(50, ErrorMessage = "Le nom doit avoir 50 caractères au maximum")]
    public string Nom { get; set; }

    [Required(ErrorMessage = "Le prénom est requis.")]
    [MaxLength(50, ErrorMessage = "Le prénom doit avoir 50 caractères au maximum")]
    public string Prenom { get; set; }

    [MaxLength(10, ErrorMessage = "Le mobile doit avoir 10 caractères au maximum")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Le mobile doit contenir 10 chiffres")]
    public string Mobile { get; set; }

    [Required(ErrorMessage = "Le mail est requis.")]
    [EmailAddress(ErrorMessage = "Le format du mail n'est pas valide")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La longueur d’un email doit être comprise entre 6 et 100 caractères.")]
    public string Mail { get; set; }

    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [MaxLength(64, ErrorMessage = "Le mot de passe doit avoir 64 caractères au maximum")]
    [StringLength(20, MinimumLength = 12, ErrorMessage = "Le mot de passe doit contenir entre 12 et 20 caractères.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$", ErrorMessage = "Le mot de passe doit contenir entre 12 et 20 caractères avec au moins 1 lettre majuscule, 1 chiffre et 1 caractère spécial")]
    public string Pwd { get; set; }

    [MaxLength(200, ErrorMessage = "La rue doit avoir 200 caractères au maximum")]
    public string Rue { get; set; }

    [RegularExpression(@"^\d{5}$", ErrorMessage = "Le code postal doit être constitué 5 chiffres")]
    [MaxLength(5, ErrorMessage = "Le code postal doit avoir 5 chiffres au maximum")]
    public string CodePostal { get; set; }

    [MaxLength(50, ErrorMessage = "La ville doit avoir 50 caractères au maximum")]
    public string Ville { get; set; }

    [MaxLength(50, ErrorMessage = "Le pays doit avoir 50 caractères au maximum")]
    public string Pays { get; set; }
}