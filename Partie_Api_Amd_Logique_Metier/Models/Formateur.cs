using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIFORDATA.Models
{
    public class Formateur:User
    {
        [Required]
        [StringLength(100)]
        public string Specialite { get; set; }

        [Required]
        [StringLength(1000)]
        public string Biographie { get; set; }

        [Required]
        [Url]
        public string LienLinkedIn { get; set; }
        public ICollection<Formation> Formations { get; set; }

        //[StringLength(50)]
        //public CompteBancaire CompteBancaire { get; set; }


    }
}