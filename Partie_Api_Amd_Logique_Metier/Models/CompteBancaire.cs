using System.ComponentModel.DataAnnotations;

namespace APIFORDATA.Models
{
    public class CompteBancaire
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string IBAN { get; set; }

        [Required]
        [StringLength(50)]
        public string Banque { get; set; } 

        [Required]
        [StringLength(100)]
        public string TitulaireCompte { get; set; } 

        //[Required]
        //public int ProfesseurId { get; set; }
        //public Formateur Formateur { get; set; }
    }
}