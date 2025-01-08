
using Microsoft.AspNetCore.Mvc;

using Partie_Consumation_API_Frontend.Service;


public class CertificateController1 : Controller
{
    private readonly CertificateService certificateService;
    private readonly ParticipantService participantService;
    private readonly InscriptionService inscriptionService;
    private readonly IConfiguration configuration;

    public CertificateController1(
        CertificateService certificateService,
        InscriptionService inscriptionService,
        IConfiguration configuration,
        ParticipantService participantService)
    {
        this.certificateService = certificateService;
        this.inscriptionService = inscriptionService;
        this.configuration = configuration;
        this.participantService = participantService;
    }

    //public async Task<IActionResult> CreateCertificate(int formationId, int participantId, string formationTitre)
    //{
    //    try
    //    {
    //        // 1. Create the certificate
    //        bool isSuccess = await certificateService.CreateCertificate(formationId, participantId, formationTitre);

    //        if (isSuccess)
    //        {
    //            // 2. Update inscription status
    //            bool isSuccessUpdate = await inscriptionService.UpdateInscriptionStatus(formationId, participantId);

    //            // 3. Generate PDF certificate
    //            var participant = await participantService.GetParticipantById(participantId);
    //            var certificatePdfPath = await GenerateCertificatePdf(participant, formationTitre);

    //            // 4. Send email with certificate
    //            await emailService.SendCertificateEmail(
    //                participant.Email,
    //                formationTitre,
    //                certificatePdfPath
    //            );

    //            // 5. Store certificate path for download
    //            TempData["CertificatePath"] = certificatePdfPath;

    //            return RedirectToAction("CertificateSuccess");
    //        }

    //        TempData["ErrorMessage"] = "Failed to create the certificate.";
    //        return RedirectToAction("CertificateError");
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"An error occurred: {ex.Message}");
    //        TempData["ErrorMessage"] = "An unexpected error occurred.";
    //        return RedirectToAction("CertificateError");
    //    }
    //}
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

                // Generate PDF certificate
           

                // Send email
               
                // Store certificate path
                TempData["SuccessMessage"] = "Certificate created and sent successfully!";

                return RedirectToAction("CertificateSuccess");
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
  
   
    
}