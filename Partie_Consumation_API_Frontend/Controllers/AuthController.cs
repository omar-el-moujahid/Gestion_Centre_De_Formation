using Microsoft.AspNetCore.Mvc;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                // Redirection vers la page précédente ou la page d'accueil si aucun rôle n'est défini
                return Redirect(Request.Headers["Referer"].ToString() ?? "/");
            }

            // Stocker le rôle dans ViewData pour affichage dans la vue
            ViewData["Role"] = role;

            return View();
        }

    }
}
