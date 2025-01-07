using Partie_Api_Amd_Logique_Metier.Models;

namespace Partie_Consumation_API_Frontend.Models
{
    public class infoDashbordCategory
    {
        public Category categrys { get; set; }
        public List<Formation> formationsCategory { get; set; }
        public int countFormation {  get; set; }   

    }
}
