using System;
using System.ComponentModel.DataAnnotations;

namespace APIFORDATA.Models
{
    public class Certificate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Le titre ne doit pas dépasser 100 caractères.")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DelivranceDate { get; set; }

        [Required]
        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }

        [Required]
        public int FormationId { get; set; }
        public Formation Formation { get; set; }
    }
}
