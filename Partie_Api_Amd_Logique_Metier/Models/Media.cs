using System.ComponentModel.DataAnnotations;

namespace Partie_Api_Amd_Logique_Metier.Models
{
    public class Media
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Le titre ne doit pas dépasser 200 caractères.")]
        public string Title { get; set; }

        // Type du média (e.g., "Vidéo", "PDF")
        [Required]
        [StringLength(50, ErrorMessage = "Le type ne doit pas dépasser 50 caractères.")]
        public string Type { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public int FormationId { get; set; }

        public Formation Formation { get; set; }
    }
}
