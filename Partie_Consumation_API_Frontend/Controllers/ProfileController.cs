using Microsoft.AspNetCore.Mvc;
using Partie_Api_Amd_Logique_Metier.Models;
using Microsoft.AspNetCore.Authorization;
using Partie_Consumation_API_Frontend.Service;
using System.Security.Claims;
using System.Net.Http;
using System.Text.Json;
using Partie_Consumation_API_Frontend.Models;

namespace Partie_Consumation_API_Frontend.Controllers
{
    [Authorize(Roles = "Formateur")]
    public class ProfileController : Controller
    {
        private readonly FormateurService _formateurService;
        private readonly ProfileService _profileService; // Remplacer FormateurService par ProfileService si nécessaire
        private readonly HttpClient _httpClient;

        public ProfileController(FormateurService formateurService, ProfileService profileService, HttpClient httpClient)
        {
            _formateurService = formateurService;
            _profileService = profileService; // Injecter ProfileService
            _httpClient = httpClient;
        }

        private int GetCurrentFormateurId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("Utilisateur non authentifié");
            }
            return int.Parse(userIdClaim.Value);
        }

        // Récupérer le profil du formateur
        public async Task<IActionResult> Index()
        {

            try
            {
                int formateurId = GetCurrentFormateurId();
                var formateur = await _formateurService.GetFormateurById(formateurId);
                var categories = await _profileService.GetCategories();
                var formations = await _formateurService.GetFormationsByFormateurId(formateurId);
                ViewBag.Formations = formations;


                if (formateur == null)
                {
                    return NotFound($"Formateur avec ID {formateurId} introuvable.");
                }

                // Récupérer les formations associées au formateur

                // Créer un modèle pour la vue
                var profileViewModel = new ProfileViewModel
                {
                    Formateur = formateur,
                    Formations = formations,
                    Categories = categories,
                    Inscriptions=null

                };

                return View(profileViewModel);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue : {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(IFormCollection form)
        {
            try
            {
                int formateurId = GetCurrentFormateurId();

                // Récupérer les données mises à jour du formulaire
                var updatedFormateur = new
                {
                    Name = form["Name"].ToString(),
                    Prenom = form["Prenom"].ToString(),
                    Email = form["Email"].ToString(),
                    Specialite = form["Specialite"].ToString(),
                    Biographie = form["Biographie"].ToString(),
                    LienLinkedIn = form["LienLinkedIn"].ToString(),
                };

                // Appeler le service pour mettre à jour le profil
                var result = await _formateurService.UpdateFormateurProfile(formateurId, updatedFormateur);

                if (result)
                {
                    TempData["SuccessMessage"] = "Profil mis à jour avec succès !";
                }
                else
                {
                    TempData["ErrorMessage"] = "Une erreur est survenue lors de la mise à jour du profil.";
                }

                // Rediriger vers Index après la mise à jour
                return RedirectToAction("Index");
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Une erreur est survenue : {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        // Ajouter une formation
        [HttpPost]
        public async Task<IActionResult> AddFormation(IFormCollection form, List<IFormFile> UrlImage)
        {
            try
            {
                var formationData = new Dictionary<string, object>
                {
                    { "Titre", form["Titre"] },
                    { "CategoryId", int.Parse(form["CategoryId"]) },
                    { "Description", form["Description"] },
                    { "Prix", decimal.Parse(form["Prix"]) },
                    { "EstimationDeDuree", int.Parse(form["EstimationDeDuree"]) },
                };

                if (UrlImage.Count > 0)
                {
                    var imageUrl = await _formateurService.UploadImage(UrlImage[0]);
                    if (imageUrl != null)
                    {
                        formationData.Add("UrlImage", imageUrl);
                    }
                }

                var result = await _formateurService.AddFormation(formationData);

                if (result)
                {
                    TempData["Success"] = "Formation ajoutée avec succès.";
                    return RedirectToAction("Profile");
                }

                TempData["Error"] = "Une erreur est survenue lors de l'ajout de la formation.";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Une erreur est survenue : {ex.Message}";
                return RedirectToAction("Profile");
            }
        }

        // Supprimer une formation
        [HttpPost]
        public async Task<IActionResult> DeleteFormation(int id)
        {
            try
            {
                int formateurId = GetCurrentFormateurId();
                var formation = await _formateurService.GetFormationById(id);

                if (formation == null || formation.FormateurId != formateurId)
                {
                    return Unauthorized();
                }

                var result = await _formateurService.DeleteFormation(id);
                if (result)
                {
                    TempData["Success"] = "Formation supprimée avec succès.";
                }
                else
                {
                    TempData["Error"] = "Erreur lors de la suppression de la formation.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Une erreur est survenue : {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
