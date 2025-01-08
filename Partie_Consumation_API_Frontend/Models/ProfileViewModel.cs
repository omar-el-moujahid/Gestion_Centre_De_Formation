using Partie_Api_Amd_Logique_Metier.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Partie_Consumation_API_Frontend.Models
{
    public class ProfileViewModel
    {
        public Formateur Formateur { get; set; }  // Détails du formateur
        public List<Formation> Formations { get; set; }  // Liste des formations
        public List<Category> Categories { get; set; }  // Liste des catégories
        public List<Inscription> Inscriptions { get; set; }

        public List<Media> Media { get; set; }
        [NotMapped]
        public IFormFile UrlImage { get; set; }
    }

    public class FormationResult
    {
        public bool Success { get; set; }
        public int FormationId { get; set; }
    }
}