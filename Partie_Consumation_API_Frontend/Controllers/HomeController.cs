using Microsoft.AspNetCore.Mvc;
using Partie_Consumation_API_Frontend.Models;
using System.Diagnostics;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult Price()
        {
            return View();
        }
        public IActionResult features()
        {
            return View();
        }
        public IActionResult team()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult testimonial()
        {
            return View();
        }
        public IActionResult quote()
        {
            return View();
        }
        public IActionResult Courses()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult RegisterFormateur()
        {
            return View();
        }
        public IActionResult RegisterParticipant()
        {
            return View();
        }
        public IActionResult SignupParticipant()
        {
            return View();
        }
        public IActionResult RegisterAdministrateur()
        {
            return View();
        }
        public IActionResult Notfounpage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
