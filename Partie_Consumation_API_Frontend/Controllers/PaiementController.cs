using Microsoft.AspNetCore.Mvc;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Service;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class PaiementController : Controller
    {
        private readonly PaiementService _paiementService;
        private readonly FormationService _formationService;

        public PaiementController(PaiementService paiementService, FormationService formationService)
        {
            _paiementService = paiementService;
            _formationService = formationService;
        }
        public IActionResult Index(int formation_id, int participant_id)
        {
            // Vérifiez si les IDs sont correctement reçus
            Console.WriteLine($"Formation ID : {formation_id}, Participant ID : {participant_id}");

            ViewData["formation_id"] = formation_id;
            ViewData["participant_id"] = participant_id;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(int formation_id = 1, int participant_id = 1)
        {
            Console.WriteLine($"Début de la création du paiement : formation_id={formation_id}, participant_id={participant_id}");
            Payment payment = new Payment();
            payment.FormationId = formation_id;
            payment.ParticipantId = participant_id;
            payment.Date = DateTime.Now;
            try
            {
                Console.WriteLine("Vérification de l'existence de la formation...");
                Formation formation = await _formationService.GetFormationsbyid(formation_id);
                if (formation == null)
                {
                    Console.WriteLine("Formation introuvable.");
                    TempData["ErrorMessage"] = "Formation introuvable.";
                    return RedirectToAction("Index", "Paiement", new { formation_id, participant_id });
                }

                payment.Amount = formation.Prix;

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Données invalides.");
                    TempData["ErrorMessage"] = "Données invalides.";
                    return RedirectToAction("Index", "Paiement", new { formation_id, participant_id });
                }

                Console.WriteLine("Création du paiement...");
                await _paiementService.CreatePayment(payment);
                Console.WriteLine("Paiement créé avec succès.");

                TempData["SuccessMessage"] = "Paiement créé avec succès.";
                return RedirectToAction("Index", "FormationController1", new { id = formation_id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                TempData["ErrorMessage"] = "Une erreur inattendue est survenue : " + ex.Message;
                return RedirectToAction("Index", "Paiement", new { formation_id, participant_id });
            }
        }

        //public async Task<IActionResult> Create(int formation_id = 1, int participant_id = 1)
        //{
        //    Payment payment = new Payment
        //    {
        //        FormationId = formation_id,
        //        ParticipantId = participant_id
        //    };

        //    try
        //    {
        //        // Vérification si la formation existe
        //        Formation formation = await _formationService.GetFormationsbyid(formation_id);
        //        if (formation == null)
        //        {
        //            TempData["ErrorMessage"] = "Formation introuvable.";
        //            return RedirectToAction("Index", "Paiement", new { formation_id, participant_id });
        //        }

        //        payment.Amount = formation.Prix;

        //        if (!ModelState.IsValid)
        //        {
        //            TempData["ErrorMessage"] = "Données invalides.";
        //            return RedirectToAction("Index", "Paiement", new { formation_id, participant_id });
        //        }

        //        // Création du paiement
        //        await _paiementService.CreatePayment(payment);

        //        TempData["SuccessMessage"] = "Paiement créé avec succès.";
        //        // Redirection vers la page de formation en cas de succès
        //        return RedirectToAction("Index", "Paiement", new { formation_id, participant_id });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Gestion des erreurs et redirection vers la page de paiement
        //        TempData["ErrorMessage"] = "Une erreur inattendue est survenue : " + ex.Message;
        //        return RedirectToAction("Index", "Paiement", new { formation_id, participant_id });
        //    }
        //}

        //public async Task<IActionResult> Create(int formation_id = 1, int participant_id = 1)
        //{
        //    Payment payment = new Payment();
        //    payment.FormationId = formation_id;
        //    payment.ParticipantId = participant_id;
        //    Formation formation = await _formateurService.GetFormateurById(formation_id);
        //    if (formation == null)
        //    {
        //        ViewBag.ErrorMessage = "Formation introuvable.";
        //        return View(payment);
        //    }
        //    payment.Amount = formation.Prix;
        //    if (!ModelState.IsValid)
        //    {
        //        return View(payment);
        //    }

        //    try
        //    {
        //        var createdPayment = await _paiementService.CreatePayment(payment);
        //        return RedirectToAction("Index", "Paiement"); // Rediriger après la création
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        ViewBag.ErrorMessage = "Erreur lors de la création du paiement.";
        //        Console.WriteLine($"Error: {ex.Message}");
        //        return View(payment);
        //    }
        //}
        //public async Task<IActionResult> AddPayment(int id, int participant_id = 1)
        //{
        //    try
        //    {
        //        Payment payment = await _paiementService.GetPayementBy2Ids(id, participant_id);

        //        if (payment == null)
        //        {
        //            // Return a view for no payment found
        //            ViewBag.Message = "No payment details found for the specified IDs.";
        //            return View("NoPayment");
        //        }

        //        // Pass the payment details to the view
        //        return View(payment);
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        // Log the error and return an error view
        //        Console.WriteLine($"Error fetching payment: {ex.Message}");
        //        ViewBag.Message = "An error occurred while fetching payment details.";
        //        return View("Error");
        //    }
        //}
    }
}
