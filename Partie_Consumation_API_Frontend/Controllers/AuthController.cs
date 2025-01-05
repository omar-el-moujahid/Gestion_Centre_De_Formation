using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Service;
using System.Data;
using System.Reflection.Metadata;

namespace Partie_Consumation_API_Frontend.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        [ActivatorUtilitiesConstructor]
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Index(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                // Redirection vers la page précédente ou la page d'accueil si aucun rôle n'est défini
                return Redirect(Request.Headers["Referer"].ToString() ?? "/");
            }

            // Stocker le rôle dans ViewData pour affichage dans la vue
            ViewData["Role"] = role;

            return View();
        }

        public IActionResult signUp() => View();






        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Participant model , string confirmPassword)
        {
            // Vérifier si les mots de passe correspondent
            if (model.Password != confirmPassword)
            {
                ViewData["test"] = "Passwords do not match. Please try again.";
                // Ajouter un message d'erreur à ModelState si les mots de passe ne correspondent pas
                return View("signUp");
            }
            else
            {
                model.RoleId = 3;
                _authService.CreateStudents(model);
                return RedirectToAction("Index", new { role = "participant" });
            }
                

        }


        [HttpGet]
        public IActionResult login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> login(string email , string password, string rolee)

        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rolee))
            {

                ModelState.AddModelError("", "Email, password, and role are required.");
                return RedirectToAction("Index", "Auth");

            }
            string returnUrl = TempData["ReturnUrl"] as string;
            switch (rolee.ToLower())
            {

                case "participant":
                    Participant participant = await _authService.authparticipant(email, password);
                    if (participant != null)
                    {
                        HttpContext.Session.SetString("UserEmail", participant.Email);
                        HttpContext.Session.SetString("UserId", participant.Id.ToString());
                        HttpContext.Session.SetString("UserNom", value: participant.Name);
                        HttpContext.Session.SetString("UserPrenom", value: participant.Prenom);
                        HttpContext.Session.SetString("UserRolefromclass", value: participant.Role.Name);
                        HttpContext.Session.SetString("UserRole", rolee);
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            TempData.Remove("ReturnUrl"); // Supprime l'URL après utilisation
                            return Redirect(returnUrl);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    break;

                case "administrateur":
                    Admin admin = await _authService.authadmin(email, password);
                    if (admin != null)
                    {
                        HttpContext.Session.SetString("UserEmail", admin.Email);
                        HttpContext.Session.SetString("UserId", admin.Id.ToString());
                        HttpContext.Session.SetString("UserNom", value: admin.Name);
                        HttpContext.Session.SetString("UserPrenom", value: admin.Prenom);
                        HttpContext.Session.SetString("UserRolefromclass", value: admin.Role.Name);
                        HttpContext.Session.SetString("UserRole", rolee);
                     
                         return RedirectToAction("Index", "Admine");
                    }
                    break;

                case "formateur":
                    Formateur formateur = await _authService.authformateur(email, password);
                    if (formateur != null)
                    {
                        HttpContext.Session.SetString("UserEmail", formateur.Email);
                        HttpContext.Session.SetString("UserId", formateur.Id.ToString());
                        HttpContext.Session.SetString("UserNom", value: formateur.Name);
                        HttpContext.Session.SetString("UserPrenom", value: formateur.Prenom);
                        HttpContext.Session.SetString("UserRolefromclass", value: formateur.Role.Name);
                        HttpContext.Session.SetString("UserRole", rolee);
                     
                        return RedirectToAction("Index", "Home");
                    }
                    break;

                default:
                    ModelState.AddModelError("", "Invalid role.");
                    return RedirectToAction("Index", "Auth");

            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return RedirectToAction("Index", "Auth", new { role = rolee });
        }
    }

}

