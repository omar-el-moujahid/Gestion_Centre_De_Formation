using Partie_Api_Amd_Logique_Metier.Models;

namespace Partie_Consumation_API_Frontend.Models
{
    public class ProfileViewModel
    {
        public Formateur Formateur { get; set; }  // Détails du formateur
        public List<Formation> Formations { get; set; }  // Liste des formations
        public List<Category> Categories { get; set; }  // Liste des catégories
        public List<Inscription> Inscriptions { get; set; }
    }
}
