
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Models;
using Partie_Consumation_API_Frontend.Service;


public class CertificateController1 : Controller
{
    private readonly CertificateService certificateService;
    private readonly ParticipantService participantService;
    private readonly InscriptionService inscriptionService;
    private readonly IConfiguration configuration;
    private readonly FormationService formationService;
    public CertificateController1(
        CertificateService certificateService,
        InscriptionService inscriptionService,
        IConfiguration configuration,
        ParticipantService participantService, FormationService formationService)
    {
        this.certificateService = certificateService;
        this.inscriptionService = inscriptionService;
        this.configuration = configuration;
        this.participantService = participantService;
        this.formationService = formationService;
    }


    public async Task<IActionResult> CreateCertificate(int formationId, int participantId, string formationTitre)
    {
        try
        {
            // Validate input parameters
            if (formationId <= 0 || participantId <= 0 || string.IsNullOrEmpty(formationTitre))
            {
                TempData["ErrorMessage"] = "Invalid parameters provided.";
                return RedirectToAction("CertificateError");
            }

            // Log input parameters
            Console.WriteLine($"Creating certificate for Formation: {formationId}, Participant: {participantId}, Title: {formationTitre}");

            // Create certificate
            bool isSuccess = await certificateService.CreateCertificate(formationId, participantId, formationTitre);

            if (!isSuccess)
            {
                TempData["ErrorMessage"] = "Certificate creation failed in the service layer.";
                return RedirectToAction("CertificateError");
            }

            try
            {
                // Update inscription status
                bool isSuccessUpdate = await inscriptionService.UpdateInscriptionStatus(formationId, participantId);
                if (!isSuccessUpdate)
                {
                    throw new Exception("Failed to update inscription status");
                }

                // Get participant details
                var participant = await participantService.GetParticipantById(participantId);
                if (participant == null)
                {
                    throw new Exception("Participant not found");
                }

            
                TempData["SuccessMessage"] = "Certificate created and sent successfully!";

                //return RedirectToAction("CertificateSuccess");
                return RedirectToAction("ShowCertificate", new
                {
                    formationId = formationId,
                    participantId = participantId
                });
            }
            catch (Exception innerEx)
            {
                Console.WriteLine($"Process error: {innerEx.Message}");
                Console.WriteLine($"Stack trace: {innerEx.StackTrace}");
                TempData["ErrorMessage"] = $"Process error: {innerEx.Message}";
                return RedirectToAction("CertificateError");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"System error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            TempData["ErrorMessage"] = $"System error: {ex.Message}";
            return RedirectToAction("CertificateError");
        }
    }
    public IActionResult CertificateSuccess()
    {
        return View();
    }

    public IActionResult CertificateError()
    {
        return View();
    }
    //public async Task<IActionResult> ShowCertificate(int formationId, int participantId)
    //{
    //    try
    //    {
    //        var participant = await participantService.GetParticipantById(participantId);
    //        var formation = await formateurService.GetFormationById(formationId);

    //        if (participant == null)
    //        {
    //            TempData["ErrorMessage"] = "Participant non trouvé.";
    //            return RedirectToAction("CertificateError");
    //        }

    //        if (formation == null)
    //        {
    //            TempData["ErrorMessage"] = "Formation non trouvée.";
    //            return RedirectToAction("CertificateError");
    //        }

    //        var viewModel = new CertificateViewModel
    //        {
    //            ParticipantName = $"{participant.Name} {participant.Prenom}",
    //            FormationTitle = formation.Titre,
    //            DateDelivrance = DateTime.Now,
    //            Url = formation.Url,
    //            CompanyName = "CODEM"
    //        };

    //        // Spécifier explicitement le chemin complet de la vue
    //        return View("~/Views/CertificateController1/ShowCertificate.cshtml", viewModel);
    //    }
    //    catch (Exception ex)
    //    {
    //        TempData["ErrorMessage"] = "Une erreur s'est produite lors de la génération du certificat.";
    //        return RedirectToAction("CertificateError");
    //    }
    //}
    //public async Task<IActionResult> ShowCertificate(int formationId, int participantId)
    //{
    //    try
    //    {
    //        // Log des paramètres d'entrée
    //        Console.WriteLine($"ShowCertificate called with formationId: {formationId}, participantId: {participantId}");

    //        // Récupération du participant
    //        Participant participant = await participantService.GetParticipantById(participantId);
    //        Console.WriteLine($"Participant retrieved: {participant?.Name ?? "null"}");

    //        // Récupération de la formation
    //        Formation formation = await formateurService.GetFormationById(formationId);
    //        Console.WriteLine($"Formation retrieved: {formation?.Titre ?? "null"}");

    //        // Vérifications détaillées
    //        if (participant == null)
    //        {
    //            Console.WriteLine("Error: Participant is null");
    //            TempData["ErrorMessage"] = "Participant non trouvé.";
    //            return RedirectToAction("CertificateError");
    //        }

    //        if (formation == null)
    //        {
    //            Console.WriteLine("Error: Formation is null");
    //            TempData["ErrorMessage"] = "Formation non trouvée.";
    //            return RedirectToAction("CertificateError");
    //        }

    //        // Construction du ViewModel avec vérification des valeurs
    //        var viewModel = new CertificateViewModel
    //        {
    //            ParticipantName = !string.IsNullOrEmpty(participant.Prenom) ?
    //                $"{participant.Name} {participant.Prenom}" :
    //                participant.Name,
    //            FormationTitle = formation.Titre ?? "Formation sans titre",
    //            DateDelivrance = DateTime.Now,
    //            Url = formation.url_image ?? "", // Gestion du cas où l'URL est null
    //            CompanyName = "CODEM"
    //        };

    //        Console.WriteLine("ViewModel created successfully");
    //        Console.WriteLine($"ParticipantName: {viewModel.ParticipantName}");
    //        Console.WriteLine($"FormationTitle: {viewModel.FormationTitle}");
    //        Console.WriteLine($"URL: {viewModel.Url}");



    //        return View("ShowCertificate", viewModel);
    //    }
    //    catch (Exception ex)
    //    {
    //        // Log détaillé de l'erreur
    //        Console.WriteLine($"Error in ShowCertificate: {ex.Message}");
    //        Console.WriteLine($"Stack trace: {ex.StackTrace}");

    //        // Message d'erreur plus spécifique
    //        TempData["ErrorMessage"] = $"Une erreur s'est produite : {ex.Message}";
    //        return RedirectToAction("CertificateError");
    //    }
    //}

    public async Task<IActionResult> ShowCertificate(int formationId, int participantId)
    {
        try
        {
            Console.WriteLine($"ShowCertificate called with formationId: {formationId}, participantId: {participantId}");

            var participant = await participantService.GetParticipantById(participantId);
            var formation = await formationService.GetFormationsbyid(formationId);

            if (participant == null)
            {
                TempData["ErrorMessage"] = "Participant non trouvé.";
                return RedirectToAction("CertificateError");
            }

            if (formation == null)
            {
                TempData["ErrorMessage"] = "Formation non trouvée.";
                return RedirectToAction("CertificateError");
            }

            var viewModel = new CertificateViewModel
            {
                ParticipantName = $"{participant.Name} {participant.Prenom}",
                FormationTitle = formation.Titre,
                DateDelivrance = DateTime.Now,
                Url = formation.url_image,
                CompanyName = "CODEM"
            };

            return View("ShowCertificate", viewModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ShowCertificate: {ex.Message}");
            TempData["ErrorMessage"] = $"Une erreur s'est produite : {ex.Message}";
            return RedirectToAction("CertificateError");
        }
    }

}