
using Microsoft.AspNetCore.Mvc;

using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Models;
using Partie_Consumation_API_Frontend.Service;
using System.Data;
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
            var category = await _admineService.GetAllCategoryAsync();
            var formation = await _admineService.GetAllFormationAsync();
            var students = await _admineService.GetAllStudentsAsync();
            var inscriptions = await _admineService.GetAllInscriptionsAsync();
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
            if (category != null) 
            {
                ViewData["existCategory"] = "exist";

            }
            else
            {
                ViewData["existCategory"] = "notexist";

            }
            if (formation != null)
            {
                ViewData["existFormatio"] = "exist";

            }
            else
            {
                ViewData["existFormatio"] = "notexist";

            }
            if (students != null)
            {
                ViewData["existStudents"] = "exist";

            }
            else
            {
                ViewData["existStudents"] = "notexist";

            }
            if (inscriptions != null)
            {
                ViewData["existInscription"] = "exist";

            }
            else
            {
                ViewData["existInscription"] = "notexist";

            }




            var viewModel = new AdminDashboardViewModel
            {
                Admins = admins,
                countAdmins = admins?.Count ?? 0,
                Formateurs = formateurs,
                countFormateur = formateurs?.Count ?? 0,
                Categorys = category,
                countCategory = category?.Count ?? 0,
                formations= formation,
                countFormatons= formation?.Count ?? 0,
                participant=students,
                countStudents= students?.Count ?? 0,
                countInscriptions= inscriptions?.Count ?? 0

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
        public async Task<IActionResult> DeleteFormateur(string idFormateurDelete)
        {
            int idFormateur=int.Parse(idFormateurDelete);
            // Récupérer le formateur par son ID
            Formateur formateur = await _admineService.GetFormateurByIdAsync(idFormateur);

            if (formateur == null)
            {
                ViewData["Error"] = "Formateur introuvable.";
                return RedirectToAction("Index");
            }

            // Vérifier si le formateur a des formations associées
            var formations = await _admineService.GetFormationsByFormateurAsync(idFormateur);

            if (formations != null && formations.Any())
            {

                    return RedirectToAction("Index");
            }
            else
            {
                // Si aucune formation n'est associée, supprimer le formateur
                bool isDeleted = await _admineService.DeleteFormateurAsync(idFormateur);

                if (!isDeleted)
                {
                    ViewData["Error"] = "Échec de la suppression du formateur.";
                    return RedirectToAction("Index");
                }
            }

            // Rediriger vers l'index après une opération réussie
            return RedirectToAction("Index");
        }




        


        public async Task<IActionResult> infoFormateur(string idFormateur)
        {
            if (string.IsNullOrEmpty(idFormateur))
            {
                // Redirection vers la page précédente ou la page d'accueil si aucun rôle n'est défini
                return Redirect("Index");
            }
            int idFormatour=int.Parse(idFormateur);

            // Récupérer le formateur par son ID
            Formateur formateur = await _admineService.GetFormateurByIdAsync(idFormatour);
            if(formateur == null)
            {
                // Redirection vers la page précédente ou la page d'accueil si aucun rôle n'est défini
                return Redirect("Index");
            }


            var formations = await _admineService.GetFormationsByFormateurAsync(idFormatour);
            if(formations == null)
            {
                ViewData["listFormationsexist"] = "not";
            }

            ViewData["listFormationsexist"] = "exist";


            var viewTotal = new AdminDashbordInfoFormateur {
                formateurs = formateur ,
                formationsFormateur = formations
            };

            return View(viewTotal);
        }





        public IActionResult addCategories()
        {
            return View();
        }




        

        [HttpGet]
        public IActionResult RegisterCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCategory(Category model)
        {
            try
            {
               

               

                var existingCategory = await _admineService.categoryName(model.Name);
                if (existingCategory != null)
                {
                    ViewData["Error"] = "This category already exists.";
                    return View("addCategories");
                }


                // Création de l'admin
                
                await _admineService.CreateCategoryAsync(model);

                ViewData["Success"] = "The category has been successfully saved.";
                return View("addCategories");
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                ViewData["Error"] = $"An error occurred: {ex.Message}";
                return View("addCategories");
            }
        }






        public async Task<IActionResult> infoCategory(string idCategory)
        {
            
            if (string.IsNullOrEmpty(idCategory))
            {
                // Redirection vers la page précédente ou la page d'accueil si aucun rôle n'est défini
                return Redirect("Index");
            }
            int idcat = int.Parse(idCategory);







            // Récupérer le formateur par son ID
            Category category = await _admineService.GetCategoryByIdAsync(idcat);
            if (category == null)
            {
                // Redirection vers la page précédente ou la page d'accueil si aucun rôle n'est défini
                return Redirect("Index");
            }


            var formations = await _admineService.GetFormationsByCategoryAsync(idcat);
            if (formations == null)
            {
                ViewData["listFormationsexist"] = "not";
            }

            ViewData["listFormationsexist"] = "exist";


            var viewTotalcategory = new infoDashbordCategory
            {
                categrys  = category ,
                formationsCategory = formations,
                countFormation = formations?.Count ?? 0
            };
            

            return View(viewTotalcategory);
        }




        [HttpGet]
        public IActionResult DeleteCategory()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(string idCategoryDelete)
        {
            int idcat=int.Parse(idCategoryDelete);
            // Récupérer le formateur par son ID
            Category category  = await _admineService.GetCategoryByIdAsync(idcat);

            if (category == null)
            {
                ViewData["Error_category"] = "category not found.";
                return RedirectToAction("Index");
            }

            // Vérifier si le formateur a des formations associées
            var formations = await _admineService.GetFormationsByCategoryAsync(idcat);

            if (formations != null && formations.Any())
            {

                return RedirectToAction("Index");
            }
            else
            {
                // Si aucune formation n'est associée, supprimer le formateur
                bool isDeleted = await _admineService.DeleteCategoryAsync(idcat);

                if (!isDeleted)
                {
                    ViewData["Error_category"] = "Échec de la suppression du categorie.";
                    return RedirectToAction("Index");
                }
            }

            // Rediriger vers l'index après une opération réussie
            return RedirectToAction("Index");
        }






        [HttpGet]
        public IActionResult DeleteFormation()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFormation(string idFormationDelete)
        {
            int idFormation = int.Parse(idFormationDelete);
            // Récupérer le formateur par son ID
            Formation formation = await _admineService.GetFormationByIdAsync(idFormation);




            if (formation == null)
            {

                ViewData["Error_formation"] = "training not found.";
                return RedirectToAction("Index");
            }
            


            var inscriptin = await _admineService.GetInscriptionByFormationAsync(idFormation);

            if (inscriptin != null && inscriptin.Any())
            {

                return RedirectToAction("Index");
            }
            else
            {
                // Si aucune formation n'est associée, supprimer le formateur
                bool isDeleted = await _admineService.DeleteFormationAsync(idFormation);

                if (!isDeleted)
                {
                    ViewData["Error_formation"] = "Échec de la suppression du Formation.";
                    return RedirectToAction("Index");
                }
            }
            

            // Rediriger vers l'index après une opération réussie
            return RedirectToAction("Index");
        }







    }
}


