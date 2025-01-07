using Microsoft.AspNetCore.Mvc;
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

        //[HttpPost]
        //public async Task<IActionResult> Evaluate(int participantId, int formationId, int rating, string review)
        //{
        //    try
        //    {
        //        bool success = await _evaluationService.CreateOrUpdateEvaluationAsync(participantId, formationId, rating, review);

        //        if (success)
        //        {
        //            TempData["Success"] = "Votre évaluation a été enregistrée avec succès.";
        //            return RedirectToAction("Details", "Formations", new { id = formationId });
        //        }
        //        else
        //        {
        //            TempData["Error"] = "Impossible d'enregistrer votre évaluation.";
        //            return RedirectToAction("Details", "Formations", new { id = formationId });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Une erreur s'est produite : {ex.Message}";
        //        return RedirectToAction("Details", "Formations", new { id = formationId });
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> Evaluate(int participantId,  int formationId, int rating, string review)
        {
            try
            {

                // Appeler le service pour créer ou mettre à jour l'évaluation
                bool success = await _evaluationService.CreateOrUpdateEvaluationAsync(participantId, formationId, rating, review);

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
