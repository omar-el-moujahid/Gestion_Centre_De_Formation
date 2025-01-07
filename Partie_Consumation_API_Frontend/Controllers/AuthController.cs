using Microsoft.AspNetCore.Http;
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
        public IActionResult login(string email , string password, string rolee)
        {
           
            
            if (rolee == "participant")
            {
                

               var participant = _authService.authparticipant(email, password);
                if(participant !=null)
                {

                    HttpContext.Session.SetString("Name",Newtonsoft.Json.JsonConvert.SerializeObject(participant));
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", new { role = "participant" });
            }
            if (rolee == "administrateur")
            {

                var admini = _authService.authadmin(email,password);
                if (admini != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", new { role = "administrateur" });
            }
            if (rolee == "formateur")
            {

                var formateu = _authService.authadmin(email, password);
                if (formateu != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", new { role = "formateur" });
            }
            return RedirectToAction("Index", new { role = "participant" });

        }
    }

}

