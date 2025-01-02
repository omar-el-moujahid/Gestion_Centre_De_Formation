using Microsoft.AspNetCore.Mvc;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class AdmineController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
    }
}
