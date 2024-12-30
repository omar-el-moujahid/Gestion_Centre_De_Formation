using System.ComponentModel.DataAnnotations;
using System;

namespace APIFORDATA.Models
{
    public class Inscription
    {
        [Key]
        public int Id { get; set; }

        // Relation avec Etudiant
        [Required]
        public int ParticipaantId { get; set; }
        public Participant Participant { get; set; }

        // Relation avec Formation
        [Required]
        public int FormationId { get; set; }
        public Formation Formation { get; set; }

        // Date d'inscription
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateInscription { get; set; }

        // Statut d'inscription (par exemple : "En cours", "Terminé", "Annulé")
        [Required]
        [StringLength(50)]
        public string Statut { get; set; }

    }
}