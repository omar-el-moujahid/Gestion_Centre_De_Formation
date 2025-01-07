using Partie_Api_Amd_Logique_Metier.Models;

namespace Partie_Consumation_API_Frontend.Models
{
    public class FormateurWithCategories
    {
        public Formateur Formateur { get; set; }
        public List<Category> Categories { get; set; }

    }
}
