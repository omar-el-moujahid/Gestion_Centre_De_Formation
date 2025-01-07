using Microsoft.AspNetCore.Mvc;
using Partie_Api_Amd_Logique_Metier.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Partie_Consumation_API_Frontend.Service;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Partie_Consumation_API_Frontend.Models;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class FormateurController : Controller
    {
        private readonly FormateurService _formateurService;
        private readonly HttpClient _httpClient;

        public FormateurController(FormateurService formateurService, HttpClient httpClient)
        {
            _formateurService = formateurService;
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

        [Authorize(Roles = "Formateur")]
        public async Task<IActionResult> Profile()
        {
            try
            {
                int formateurId = GetCurrentFormateurId();
                var formateur = await _formateurService.GetFormateurById(formateurId);

                if (formateur == null)
                {
                    return NotFound($"Formateur with ID {formateurId} not found.");
                }

                ViewBag.Formations = await _formateurService.GetFormationsByFormateurId(formateurId);
                return View(formateur);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View("Error");
            }
        }

        [Authorize(Roles = "Formateur")]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(IFormCollection form)
        {
            try
            {
                int formateurId = GetCurrentFormateurId();

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
                    ViewBag.SuccessMessage = "Profil mis à jour avec succès !";
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
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue : {ex.Message}";
                return RedirectToAction("Profile");
            }
        }



        [HttpPost]
        public async Task<IActionResult> AddFormation(IFormCollection form, List<IFormFile> UrlImage)
        {
            // Préparer les données pour l'API
            var formationData = new Dictionary<string, object>
    {
        { "Titre", form["Titre"] },
        { "CategoryId", int.Parse(form["CategoryId"]) },
        { "Description", form["Description"] },
        { "Prix", decimal.Parse(form["Prix"]) },
        { "EstimationDeDuree", int.Parse(form["EstimationDeDuree"]) },
    };

            // Gérer l'upload de l'image
            if (UrlImage.Count > 0)
            {
                var imageUrl = await _formateurService.UploadImage(UrlImage[0]);
                if (imageUrl != null)
                {
                    formationData.Add("UrlImage", imageUrl);
                }
            }

            // Ajouter les médias
            var mediaList = new List<Dictionary<string, object>>();
            for (int i = 0; i < form["MediaTitre[]"].Count(); i++)
            {
                mediaList.Add(new Dictionary<string, object>
        {
            { "Titre", form["MediaTitre[]"][i] },
            { "Type", form["MediaType[]"][i] },
            { "Url", form["MediaUrl[]"][i] },
            { "NombreDeSequence", int.Parse(form["NombreDeSequence[]"][i]) }
        });
            }

            formationData.Add("Medias", mediaList);

            // Envoyer les données à l'API
            var result = await _formateurService.AddFormation(formationData);

            if (result)
            {
                TempData["Success"] = "Formation ajoutée avec succès.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Une erreur est survenue lors de l'ajout de la formation.";
            return RedirectToAction("Index");
        }

        // Ajoutez ce log
        public async Task<ViewResult> AddFormation()
        {
            var categories = await _formateurService.GetCategories();
            Console.WriteLine($"Categories received: {JsonSerializer.Serialize(categories)}");
            ViewBag.Categories = categories;
            return View();
        }





        [Authorize(Roles = "Formateur")]
        [HttpPost]
        public async Task<IActionResult> DeleteFormation(int id)
        {
            try
            {
                int formateurId = GetCurrentFormateurId();

                // Vérifier que la formation appartient bien au formateur connecté
                var formation = await _formateurService.GetFormationById(id);
                if (formation == null || formation.FormateurId != formateurId)
                {
                    return Unauthorized();
                }

                var result = await _formateurService.DeleteFormation(id);
                if (result)
                {
                    return RedirectToAction(nameof(Profile));
                }
                return BadRequest("Échec de la suppression de la formation");
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        


    }
}
