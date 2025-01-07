using Microsoft.AspNetCore.Mvc;
using Partie_Api_Amd_Logique_Metier;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Service;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class PaiementController : Controller
    {
        private readonly PaiementService _paiementService;
        private readonly FormationService _formationService;
        private readonly InscriptionService _inscriptionService;

        public PaiementController(PaiementService paiementService, FormationService formationService, InscriptionService inscriptionService)
        {
            _paiementService = paiementService;
            _formationService = formationService;
            _inscriptionService = inscriptionService;
        }

        public IActionResult Index(int formation_id)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userRole))
            {
                // Stocker l'URL actuelle avant de rediriger vers l'authentification
                TempData["ReturnUrl"] = Url.Action("Index", "Paiement", new { formation_id });
                return RedirectToAction("Index", "Auth", new { role = "participant" });
            }

            if (userRole != "participant")
            {
                TempData["ErrorMessage"] = "Vous devez être authentifié en tant que participant pour effectuer cette action.";
                return RedirectToAction("Index", "Auth", new { role = "participant" });
            }

            // Récupérer l'ID du participant depuis la session
            var participantId = HttpContext.Session.GetString("UserId");

            ViewData["formation_id"] = formation_id;
            ViewData["participant_id"] = participantId; // Passer l'ID du participant à la vue

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int formation_id)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Index", "Auth", new { role = "participant" });
            }

            if (userRole != "participant")
            {
                TempData["ErrorMessage"] = "Vous devez être authentifié en tant que participant pour effectuer cette action.";
                return RedirectToAction("Index", "Paiement", new { formation_id });
            }

            try
            {
                // Récupérer l'ID du participant depuis la session
                var participantIdString = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(participantIdString))
                {
                    TempData["ErrorMessage"] = "ID du participant introuvable dans la session.";
                    return RedirectToAction("Index", "Paiement", new { formation_id });
                }

                int participantId = int.Parse(participantIdString);

                // Récupérer la formation par son ID
                Formation formation = await _formationService.GetFormationsbyid(formation_id);
                if (formation == null)
                {
                    TempData["ErrorMessage"] = "Formation introuvable.";
                    return RedirectToAction("Index", "Paiement", new { formation_id });
                }

                // Créer le paiement
                Payment payment = new Payment
                {
                    FormationId = formation_id,
                    ParticipantId = participantId, // Utiliser l'ID du participant depuis la session
                    Date = DateTime.Now,
                    Amount = formation.Prix
                };

                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Données invalides.";
                    return RedirectToAction("Index", "Paiement", new { formation_id });
                }

                await _paiementService.CreatePayment(payment);

                // Créer l'inscription
                Inscription inscription = new Inscription
                {
                    FormationId = formation_id,
                    ParticipaantId = participantId, // Utiliser l'ID du participant depuis la session
                    DateInscription = DateTime.Now,
                    Statut = Statut.InProgress
                };
                await _inscriptionService.CreateInscription(inscription);

                TempData["SuccessMessage"] = "Paiement créé avec succès.";
                return RedirectToAction("Index", "FormationController1", new { id = formation_id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                TempData["ErrorMessage"] = "Une erreur inattendue est survenue : " + ex.Message;
                return RedirectToAction("Index", "Paiement", new { formation_id });
            }
        }

    }
}