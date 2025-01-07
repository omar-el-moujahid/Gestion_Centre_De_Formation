using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Service;
using System.Data;
using System.Reflection.Metadata;
using System.Security.Claims;

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


      

        public async Task<IActionResult> login(string email, string password, string rolee)
        {
            try
            {
                if (rolee == "formateur")
                {
                    var formateur = await _authService.authformateur(email, password);
                    if (formateur != null)
                    {
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, formateur.Id.ToString()),
                    new Claim(ClaimTypes.Name, formateur.Email),
                    new Claim(ClaimTypes.Role, "Formateur")
                };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties { IsPersistent = true }
                        );

                        return RedirectToAction("Index", "Profile");
                    }
                }
                // Reste du code inchangé pour participant et administrateur
                return RedirectToAction("Index", new { role = rolee });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { role = rolee });
            }
        }
    }

}

