using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Partie_Api_Amd_Logique_Metier.Models
{
    public class CoursesViewModel
    {
        public IEnumerable<Formation> Formations { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}