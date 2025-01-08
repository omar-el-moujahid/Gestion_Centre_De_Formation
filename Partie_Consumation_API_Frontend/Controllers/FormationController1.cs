using Microsoft.AspNetCore.Mvc;
using Partie_Consumation_API_Frontend.Service;
using Partie_Api_Amd_Logique_Metier.Models;
using Microsoft.IdentityModel.Tokens;
using Partie_Consumation_API_Frontend.Models;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class FormationController1 : Controller
    {
        private readonly FormationService _formationService;
        private readonly InscriptionService _inscriptionService;

        public FormationController1(FormationService formationService  , InscriptionService inscriptionService)
        {
            _formationService = formationService;
            _inscriptionService = inscriptionService;
        }
        public async Task<IActionResult> Index(int id)
        {
            // Vérifier si la session existe
            string participant_id_string = HttpContext.Session.GetString("UserId");

            // Récupérer la formation par ID
            var formation = await _formationService.GetFormationsbyid(id);
            if (formation == null)
            {
                TempData["ErrorMessage"] = "Formation introuvable.";
                return RedirectToAction("Index", "Home");
            }

            // Si la session n'existe pas
            if (string.IsNullOrEmpty(participant_id_string))
            {
                ViewData["ActionLabel"] = "Acheter"; // Si pas de session, afficher "Acheter"
                return View(formation);
            }

            // Si la session existe, vérifier si l'utilisateur est inscrit
            if (int.TryParse(participant_id_string, out int participant_id))
            {
                var inscription = await _inscriptionService.GetInscriptiony2Ids(id, participant_id);
                if (inscription != null)
                {
                    ViewData["ActionLabel"] = "Consulter"; // Déjà inscrit, afficher "Consulter"
                }
                else
                {
                    ViewData["ActionLabel"] = "Acheter"; // Pas inscrit, afficher "Acheter"
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Session invalide. Veuillez vous reconnecter.";
                return RedirectToAction("Index", "Auth", new { role = "participant" });
            }

            return View(formation);
        }

        //public async Task<IActionResult> ConsulterFormation(int id=1)
        //{
        //    var formation = await _formationService.GetFormationsbyid(id);
        //    if (formation == null)
        //    {
        //        TempData["ErrorMessage"] = "Formation introuvable.";
        //        return RedirectToAction("Index");
        //    }
        //    return View(formation);
        //}

        public async Task<IActionResult> ConsulterFormation(int id , int  id_p)
        {
            try
            {
                var formation = await _formationService.GetFormationWithMedia(id);
                if (formation == null)
                {
                    TempData["ErrorMessage"] = "Formation introuvable.";
                    return RedirectToAction("Index");
                }
                ViewData["id_p"] = id_p;
                return View(formation);
            }
            catch (HttpRequestException)
            {
                TempData["ErrorMessage"] = "Erreur lors de la récupération de la formation.";
                return RedirectToAction("Index");
            }
        }
        }
}
