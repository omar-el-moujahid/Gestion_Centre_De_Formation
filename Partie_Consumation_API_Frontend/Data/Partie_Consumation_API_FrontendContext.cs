using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Partie_Consumation_API_Frontend.Models;

namespace Partie_Consumation_API_Frontend.Model
{
    public class Partie_Consumation_API_FrontendContext : DbContext
    {
        public Partie_Consumation_API_FrontendContext (DbContextOptions<Partie_Consumation_API_FrontendContext> options)
            : base(options)
        {
        }

      
    }
}
