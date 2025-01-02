
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Partie_Consumation_API_Frontend.Service;
using System.Security.Claims;

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

                return View(formateur);
            }
            catch (Exception ex)
            {
                // Log the error and return a custom error view or message
                ViewBag.ErrorMessage = $"An error occurred while fetching the profile: {ex.Message}";
                return View("Error");
            }
        }
    }


}
