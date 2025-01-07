
namespace Partie_Consumation_API_Frontend.Models
{
    public class FromationForMedia
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string url_image { get; set; }
        public string Description { get; set; }
        public decimal Prix { get; set; }
        public string EstimationDeDuree { get; set; }
        public List<Media> Media { get; set; }
    }

    public class Media
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int nombredesequence { get; set; }
        public string Url { get; set; }
    }
}
