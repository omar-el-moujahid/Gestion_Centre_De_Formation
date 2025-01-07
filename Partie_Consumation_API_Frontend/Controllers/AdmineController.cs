
using Microsoft.AspNetCore.Mvc;

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


            // Récupération des données de session
            string userEmail = HttpContext.Session.GetString("UserEmail");
            string userNom = HttpContext.Session.GetString("UserNom");
            string userPrenom = HttpContext.Session.GetString("UserPrenom");
            string userRole = HttpContext.Session.GetString("UserRole");

            // Stockage dans le ViewBag pour utilisation dans la vue
            ViewBag.UserEmail = userEmail;
            ViewBag.UserNom = userNom;
            ViewBag.UserPrenom = userPrenom;
            ViewBag.UserRole = userRole;


            // Appel au service pour obtenir les admins
            var admins = await _admineService.GetAllAdminsAsync();
            var formateurs = await _admineService.GetAllFormateurAsync();
            if (admins != null)
            {
                ViewData["existAdmin"] = "exist";
            }
            else
            {
                ViewData["existAdmin"] = "notexist";
            }

            if (formateurs != null)
            {
                ViewData["existFormateur"] = "exist";
            }
            else
            {
                ViewData["existFormateur"] = "notexist";
            }




            var viewModel = new AdminDashboardViewModel
            {
                Admins = admins,
                countAdmins = admins?.Count ?? 0,
                Formateurs = formateurs,
                countFormateur = formateurs?.Count ?? 0,

            };

            return View(viewModel);








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





        public IActionResult RegisterTeachers()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AddFormateur()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFormateur(Formateur model, string ConfirmPassword)
        {
            try
            {
                // Vérification des mots de passe
                if (model.Password != ConfirmPassword)
                {
                    ViewData["Error"] = "Passwords do not match. Please try again.";
                    return View("RegisterTeachers");
                }

                // Validation de l'email
                if (!IsValidEmail(model.Email))
                {
                    ViewData["Error"] = "The email format is invalid.";
                    return View("RegisterTeachers");
                }

                // Vérification de l'existence de l'email
                var existingFormateur = await _admineService.formateurEmail(model.Email);
                if (existingFormateur != null)
                {
                    ViewData["Error"] = "This email is already in use.";
                    return View("RegisterTeachers");
                }


                // Création de l'admin
                model.RoleId = 2; // Role par défaut
                await _admineService.CreateFormateurAsync(model);

                ViewData["Success"] = "Formateur has been registered successfully.";
                return View("RegisterTeachers");
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                ViewData["Error"] = $"An error occurred: {ex.Message}";
                return View("RegisterTeachers");
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
            try
            {
                // Suppression de l'admin
                await _admineService.DeleteAdminAsync(idAdminDelete);

                // Redirection vers l'action Index
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                ViewData["Error"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index");
            }
        }



        [HttpGet]
        public IActionResult DeleteFormateur()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFormateur(int idFormateurDelete)
        {
            try
            {
                // Vérification des formations associées
                var listFormation = await _admineService.GetFormationsByFormateurId(idFormateurDelete);

                // Si des formations sont associées au formateur, on le met à jour avant suppression
                if (listFormation != null && listFormation.Any())
                {
                    Formateur newFormateur = new Formateur
                    {
                        Name = "vide",
                        Prenom = "vide",
                        Email = "vide",
                        Biographie = "vide",
                        Specialite = "vide",
                        LienLinkedIn = "http://localhost:62869/Help/Api/PUT-api-Formateurs-id",
                        Password = "vide"
                    };

                    // Mise à jour du formateur avant suppression
                    bool isUpdated = await _admineService.UpdateFormateur(idFormateurDelete, newFormateur);
                    if (!isUpdated)
                    {
                        ViewData["Error"] = "Failed to update the formateur before deletion.";
                        return RedirectToAction("Index");
                    }
                }

                // Suppression du formateur
                bool isDeleted = await _admineService.DeleteFormateurAsync(idFormateurDelete);
                if (isDeleted)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = "Failed to delete the formateur.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                ViewData["Error"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index");
            }
        }












    }
}


