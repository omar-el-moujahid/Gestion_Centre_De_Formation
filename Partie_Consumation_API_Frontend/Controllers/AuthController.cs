using Microsoft.AspNetCore.Mvc;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
