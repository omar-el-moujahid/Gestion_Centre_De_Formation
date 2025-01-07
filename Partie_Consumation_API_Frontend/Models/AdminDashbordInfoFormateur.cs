using Partie_Api_Amd_Logique_Metier.Models;

namespace Partie_Consumation_API_Frontend.Models
{
    public class AdminDashbordInfoFormateur
    {

        public Formateur formateurs { get; set; }
        public List<Formation> formationsFormateur { get; set; }
    }
}
