using Microsoft.AspNetCore.Mvc;
using Partie_Consumation_API_Frontend.Service;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class FormateurController : Controller
    {
        private readonly FormateurService _formateurService;

        public FormateurController(FormateurService formateurService)
        {
            _formateurService = formateurService;
        }

        public async Task<IActionResult> Profile()
        {

            int formateurId = 1; // Hardcoded ID for now

            try
            {

                var formateur = await _formateurService.GetFormateurById(formateurId);

                if (formateur == null)
                {
                    return NotFound($"Formateur with ID {formateurId} not found.");
                }

                // Stocker les formations comme dynamic dans ViewBag
                ViewBag.Formations = await _formateurService.GetFormationsByFormateurId(formateurId);

                return View(formateur);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred while fetching the profile: {ex.Message}";
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(IFormCollection form)
        {
            try
            {
                int formateurId = 1; // Votre ID fixe

                // Créer un objet dynamique avec les données du formulaire
                dynamic updatedFormateur = new
                {
                    Name = form["Name"].ToString(),
                    Prenom = form["Prenom"].ToString(),
                    Email = form["Email"].ToString(),
                    Specialite = form["Specialite"].ToString(),
                    Biographie = form["Biographie"].ToString(),
                    LienLinkedIn = form["LienLinkedIn"].ToString(),
                };

                var result = await _formateurService.UpdateFormateurProfile(formateurId, updatedFormateur);

                if (result)
                {
                    // Mettre à jour le message de succès
                    ViewBag.SuccessMessage = "Profil mis à jour avec succès !";

                    // Recharger les données du formateur
                    var updatedProfile = await _formateurService.GetFormateurById(formateurId);
                    ViewBag.Formations = await _formateurService.GetFormationsByFormateurId(formateurId);

                    return View("Profile", updatedProfile);
                }
                else
                {
                    ViewBag.ErrorMessage = "Une erreur est survenue lors de la mise à jour du profil.";
                    var currentProfile = await _formateurService.GetFormateurById(formateurId);
                    ViewBag.Formations = await _formateurService.GetFormationsByFormateurId(formateurId);
                    return View("Profile", currentProfile);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue : {ex.Message}";
                var currentProfile = await _formateurService.GetFormateurById(1);
                ViewBag.Formations = await _formateurService.GetFormationsByFormateurId(1);
                return View("Profile", currentProfile);
            }
        }



        [HttpGet]
        public IActionResult AddFormation()
        {
            return PartialView("_AddFormationPartial");
        }

        [HttpPost]
        public async Task<IActionResult> AddFormation([FromForm] dynamic formation)
        {
            try
            {
                var result = await _formateurService.AddFormation(formation);
                if (result)
                {
                    return RedirectToAction(nameof(Profile));
                }
                return BadRequest("Échec de l'ajout de la formation");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditFormation(int id)
        {
            var formation = await _formateurService.GetFormationById(id);
            return PartialView("_EditFormationPartial", formation);
        }

        [HttpPost]
        public async Task<IActionResult> EditFormation(int id, [FromForm] dynamic formation)
        {
            try
            {
                var result = await _formateurService.UpdateFormation(id, formation);
                if (result)
                {
                    return RedirectToAction(nameof(Profile));
                }
                return BadRequest("Échec de la modification de la formation");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFormation(int id)
        {
            try
            {
                var result = await _formateurService.DeleteFormation(id);
                if (result)
                {
                    return RedirectToAction(nameof(Profile));
                }
                return BadRequest("Échec de la suppression de la formation");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }


}
