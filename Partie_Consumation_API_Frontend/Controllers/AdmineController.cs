using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Models;
using Partie_Consumation_API_Frontend.Service;
using System.Text.RegularExpressions;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class AdmineController : Controller
    {
        private readonly AdmineService _admineService;



        [ActivatorUtilitiesConstructor]
        public AdmineController(AdmineService adminService)
        {
            _admineService = adminService;
        }
        public async Task<IActionResult> Index()
        {
            
                // Appel au service pour obtenir les admins
                var admins = await _admineService.GetAllAdminsAsync();
                if (admins != null)
                {
                    ViewData["existAdmin"] = "exist";


                    var viewModel = new AdminDashboardViewModel
                    {
                        Admins = admins
                    };
                    return View(viewModel);
                }

                else
                {
                    ViewData["existAdmin"] = "notexist";
                    return View();
                }

                

                

               
            
        }







        public IActionResult RegisterAdmin()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin(Admin model, string confirmPassword)
        {
            try
            {
                // Vérification des mots de passe
                if (model.Password != confirmPassword)
                {
                    ViewData["Error"] = "Passwords do not match. Please try again.";
                    return View("RegisterAdmin");
                }

                // Validation de l'email
                if (!IsValidEmail(model.Email))
                {
                    ViewData["Error"] = "The email format is invalid.";
                    return View("RegisterAdmin");
                }

                // Vérification de l'existence de l'email
                var existingAdmin = await _admineService.adminEmail(model.Email);
                if (existingAdmin != null)
                {
                    ViewData["Error"] = "This email is already in use.";
                    return View("RegisterAdmin");
                }


                // Création de l'admin
                model.RoleId = 1; // Role par défaut
                await _admineService.CreateAdminAsync(model);

                ViewData["Success"] = "Admin has been registered successfully.";
                return View("RegisterAdmin");
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                ViewData["Error"] = $"An error occurred: {ex.Message}";
                return View("RegisterAdmin");
            }
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            // Expression régulière pour valider le format d'un email
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Validation stricte de l'extension du domaine
            return Regex.IsMatch(email, emailPattern) && email.Contains(".");
        }



        [HttpGet]
        public IActionResult DeleteAdmin()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAdmin(int idAdminDelete)
        {           
                // Recherche de l'admin par ID
                var deletadmin = await _admineService.DeleteAdminAsync(idAdminDelete);
                return View("Index");            
        }



    }
}
