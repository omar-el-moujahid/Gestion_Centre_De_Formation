using Microsoft.AspNetCore.Mvc;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Models;
using Partie_Consumation_API_Frontend.Service;
using System.Diagnostics;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FormationService _formationService;

        public HomeController(ILogger<HomeController> logger , FormationService formationService)
        {
            _logger = logger;
            _formationService = formationService;

        }
        //public HomeController(FormationService formationService)
        //{
        //}

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Courses()
        {
            var formations = await _formationService.GetFormations();
            return View(formations);
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

        public IActionResult ShowSession()
        {
            // Récupération des données de session existantes
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var userId = HttpContext.Session.GetString("UserId");
            var userNom = HttpContext.Session.GetString("UserNom");
            var userPrenom = HttpContext.Session.GetString("UserPrenom");
            var userRole = HttpContext.Session.GetString("UserRolefromclass");

            // Si les données de session sont nulles, retournez un message d'erreur
            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userId))
            {
                return Content("Aucune session valide n'a été trouvée.");
            }

            // Affichage des données de session
            return Content($"User Email: {userEmail}, User ID: {userId}, Nom: {userNom}, Prénom: {userPrenom}, Rôle: {userRole}");
        }


    }
}
