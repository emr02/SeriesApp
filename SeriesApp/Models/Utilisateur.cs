namespace SeriesApp.Models
{
    public class Utilisateur
    {
        // Propriété utilisée par l'API : UtilisateurId
        public int UtilisateurId
        {
            get; set;
        }

        // Pour compatibilité tu peux aussi exposer Id qui mappe vers UtilisateurId
        public int Id
        {
            get => UtilisateurId;
            set => UtilisateurId = value;
        }

        public string? Nom
        {
            get; set;
        }
        public string? Prenom
        {
            get; set;
        }
        public string? Mobile
        {
            get; set;
        }
        public string? Mail
        {
            get; set;
        }

        // Champs API
        public string? Pwd
        {
            get; set;
        }
        public string? Rue
        {
            get; set;
        }
        public string? CodePostal
        {
            get; set;
        }
        public string? Ville
        {
            get; set;
        }
        public string? Pays
        {
            get; set;
        }
    }
}