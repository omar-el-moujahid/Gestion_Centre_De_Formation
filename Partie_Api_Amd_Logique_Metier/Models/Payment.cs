using System;
using System.ComponentModel.DataAnnotations;

namespace APIFORDATA.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        // Montant du paiement
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Le montant doit être une valeur positive.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }

        [Required]
        public int FormationId { get; set; }
        public Formation Formation { get; set; }
    }
}
