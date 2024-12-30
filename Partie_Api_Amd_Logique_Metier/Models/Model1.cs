using System;
using System.Data.Entity;
using System.Linq;

namespace Partie_Api_Amd_Logique_Metier.Models
{
    public class Model1 : DbContext
    {
       
        public Model1()
            : base("name=Model1")
        {
        }

       
    }

   
}