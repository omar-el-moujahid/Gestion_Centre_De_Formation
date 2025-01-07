using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Partie_Api_Amd_Logique_Metier.Models
{

    public class FormateurCategory
    {
        public int FormateurId { get; set; }
        public Formateur Formateur { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}