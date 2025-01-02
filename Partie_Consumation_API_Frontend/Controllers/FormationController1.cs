using Microsoft.AspNetCore.Mvc;
using Partie_Consumation_API_Frontend.Service;
using Partie_Api_Amd_Logique_Metier.Models;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class FormationController1 : Controller
    {
        private readonly FormationService _formationService;

        public FormationController1(FormationService formationService)
        {
            _formationService = formationService;
        }
        public async Task<IActionResult> Index()
        {
            var formations = await _formationService.GetFormationsbyid(1);
            return View(formations);
        }

    }
}
