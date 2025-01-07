using Microsoft.AspNetCore.Mvc;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Service;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class EvaluationController : Controller
    {
        private readonly EvaluationService _evaluationService;

        public EvaluationController(EvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        // For non-AJAX requests
        [HttpPost]
        public async Task<IActionResult> Evaluate(int participantId, int formationId, int rating, string review)
        {
            try
            {
                bool success = await _evaluationService.CreateOrUpdateEvaluationAsync(participantId, formationId, rating, review);

                if (success)
                {
                    TempData["Success"] = "Votre évaluation a été enregistrée avec succès.";
                }
                else
                {
                    TempData["Error"] = "Impossible d'enregistrer votre évaluation.";
                }

                return RedirectToAction("Details", "Formations", new { id = formationId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Une erreur s'est produite : {ex.Message}";
                return RedirectToAction("Details", "Formations", new { id = formationId });
            }
        }

        //// For AJAX requests
        //[HttpPost]
        //public async Task<IActionResult> EvaluateAjax(int participantId, int formationId, int rating, string review)
        //{
        //    try
        //    {
        //        bool success = await _evaluationService.CreateOrUpdateEvaluationAsync(participantId, formationId, rating, review);

        //        if (success)
        //        {
        //            return Json(new { success = true, message = "Votre évaluation a été enregistrée avec succès." });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "Impossible d'enregistrer votre évaluation." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = $"Une erreur s'est produite : {ex.Message}" });
        //    }
        //}
        [Route("Evaluation/EvaluateAjax")]
        //[HttpPost]
        //public async Task<IActionResult> EvaluateAjax(int participantId, int formationId, int rating, string review)
        //{
        //    try
        //    {
        //        bool success = await _evaluationService.CreateOrUpdateEvaluationAsync(participantId, formationId, rating, review);

        //        if (success)
        //        {
        //            return Json(new { success = true, message = "Votre évaluation a été enregistrée avec succès." });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "Impossible d'enregistrer votre évaluation." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Ensure errors are returned as JSON
        //        return Json(new { success = false, message = $"Une erreur s'est produite : {ex.Message}" });
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> EvaluateAjax([FromBody] Evaluation evaluation)
        {
            try
            {
                // Validate inputs
                if (evaluation.ParticipantId <= 0 || evaluation.FormationId <= 0 || evaluation.Rating < 1 || evaluation.Rating > 5)
                {
                    return Json(new { success = false, message = "Invalid input parameters." });
                }

                // Call your service
                bool success = await _evaluationService.CreateOrUpdateEvaluationAsync(
                    evaluation.ParticipantId,
                    evaluation.FormationId,
                    evaluation.Rating,
                    evaluation.Feedback
                );

                if (success)
                {
                    return Json(new { success = true, message = "Votre évaluation a été enregistrée avec succès." });
                }
                else
                {
                    return Json(new { success = false, message = "Impossible d'enregistrer votre évaluation." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Une erreur s'est produite : {ex.Message}" });
            }
        }


    }
}
