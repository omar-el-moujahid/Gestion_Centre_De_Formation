using Microsoft.AspNetCore.Mvc;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Controllers;
using Partie_Consumation_API_Frontend.Service;
using System.Text.Json;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class ParticipantController1 : Controller
    {
        private readonly ParticipantService _participantService;
        private readonly InscriptionService _inscriptionService;
        public ParticipantController1(ParticipantService participantService , InscriptionService inscriptionService )
        {
            _participantService = participantService;
            _inscriptionService = inscriptionService;
        }
        public async Task<IActionResult> Profile(int participantId)
        {
            try
            {
                Participant participant = await _participantService.GetParticipantById(participantId);

                if (participant == null)
                {
                    return NotFound($"Formateur with ID {participantId} not found.");
                }

                var formations = await _inscriptionService.GetFormationsByParticipantIdWithState(participantId);

                // Debugging: Log the formations to verify they are fetched correctly
                Console.WriteLine($"Formations fetched: {JsonSerializer.Serialize(formations)}");

                ViewData["Formations"] = formations;
                return View(participant);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred while fetching the profile: {ex.Message}";
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(int participantId, IFormCollection form)
        {
            try
            {
                // Préparer les données mises à jour
                dynamic updatedFormateur = new
                {
                    Name = form["Name"].ToString(),
                    Prenom = form["Prenom"].ToString(),
                    Email = form["Email"].ToString()
                };

                // Appeler le service pour mettre à jour le profil
                var result = await _participantService.UpdateFormateurProfile(participantId, updatedFormateur);

                if (result)
                {
                    // Message de succès
                    ViewBag.SuccessMessage = "Profil mis à jour avec succès !";

                    // Recharger les données pour l'affichage
                    var updatedProfile = await _participantService.GetParticipantById(participantId);
                    return View("Profile", updatedProfile);
                }
                else
                {
                    ViewBag.ErrorMessage = "Une erreur est survenue lors de la mise à jour du profil.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur s'est produite : {ex.Message}";
            }

            // Recharger le profil en cas d'erreur
            var currentProfile = await _participantService.GetParticipantById(participantId);
            return View("Profile", currentProfile);
        }


    }
}




