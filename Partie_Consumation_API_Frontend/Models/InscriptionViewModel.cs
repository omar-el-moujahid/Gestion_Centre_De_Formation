namespace Partie_Consumation_API_Frontend.Models
{
    public class InscriptionViewModel
    {
        public int FormationId { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public string EstimationDeDuree { get; set; } // Changé en string
        public decimal Prix { get; set; }
        public int Status { get; set; } // 0 = En cours, 1 = Complet (ajustez en fonction de vos besoins)
        public DateTime DateInscription { get; set; }
        public string url { get; set; }
    }
}