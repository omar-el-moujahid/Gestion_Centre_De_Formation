using Partie_Api_Amd_Logique_Metier.Models;

namespace Partie_Consumation_API_Frontend.Models
{
    public class AdminDashboardViewModel
    {
        public List<Admin> Admins { get; set; }
        public int countAdmins { get; set; }
        public List<Formateur> Formateurs { get; set; }

        public int countFormateur { get; set; }

        public List<Category> Categorys { get; set; }

        public int countCategory { get; set; }

        public List<Formation> formations { get; set; }
        public int countFormatons { get; set; }
    }
}
