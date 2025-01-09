namespace Partie_Consumation_API_Frontend.Models
{
    public class CertificateViewModel
    {
        public string ParticipantName { get; set; }
        public string FormationTitle { get; set; }
        public DateTime DateDelivrance { get; set; }
        public string Url { get; set; }

        public string CompanyName { get; set; } = "CODEM";
    }
}
