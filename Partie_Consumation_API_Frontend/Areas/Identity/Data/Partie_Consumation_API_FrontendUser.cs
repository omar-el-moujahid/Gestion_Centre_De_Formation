using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Partie_Consumation_API_Frontend.Areas.Identity.Data;

// Add profile data for application users by adding properties to the Partie_Consumation_API_FrontendUser class
public class Partie_Consumation_API_FrontendUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

