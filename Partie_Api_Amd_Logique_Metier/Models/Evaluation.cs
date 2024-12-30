using System.ComponentModel.DataAnnotations;

namespace APIFORDATA.Models
{
    public class Evaluation
    {
        [Key]
        public int Id { get; set; }

        [StringLength(1000, ErrorMessage = "Le feedback ne doit pas dépasser 1000 caractères.")]
        public string Feedback { get; set; }

        [Required]
        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }

        [Required]
        public int FormationId { get; set; }
        public Formation Formation { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "La note doit être entre 1 et 5.")]
        public int Rating { get; set; }
    }
}
