using Microsoft.AspNetCore.Mvc;
using Partie_Api_Amd_Logique_Metier.Models;
using Microsoft.AspNetCore.Authorization;
using Partie_Consumation_API_Frontend.Service;
using System.Security.Claims;
using System.Net.Http;
using System.Text.Json;
using Partie_Consumation_API_Frontend.Models;

namespace Partie_Consumation_API_Frontend.Controllers
{
    [Authorize(Roles = "Formateur")]
    public class ProfileController : Controller
    {
        private readonly ProfileService _profileService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProfileController> _logger;


        public ProfileController(ProfileService profileService, HttpClient httpClient, ILogger<ProfileController> logger)
        {
            _profileService = profileService;
            _httpClient = httpClient;
            _logger = logger;

        }

        private int GetCurrentFormateurId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("Utilisateur non authentifié");
            }
            return int.Parse(userIdClaim.Value);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                int formateurId = GetCurrentFormateurId();
                var formateur = await _profileService.GetFormateurById(formateurId);
                var categories = await _profileService.GetCategories();
                var formations = await _profileService.GetFormationsByFormateurId(formateurId);
                var inscriptions = await _profileService.GetInscriptionsByFormateurId(formateurId);

                var profileViewModel = new ProfileViewModel
                {
                    Formateur = formateur,
                    Formations = formations,  // Assurez-vous que ceci est rempli
                    Categories = categories,
                    Inscriptions = inscriptions
                };
                ViewBag.Formations = formations;


                return View(profileViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Une erreur est survenue : {ex.Message}";
                return View(new ProfileViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(IFormCollection form)
        {
            try
            {
                int formateurId = GetCurrentFormateurId();

                // Récupérer les données mises à jour du formulaire
                var updatedFormateur = new
                {
                    Name = form["Name"].ToString(),
                    Prenom = form["Prenom"].ToString(),
                    Email = form["Email"].ToString(),
                    Specialite = form["Specialite"].ToString(),
                    Biographie = form["Biographie"].ToString(),
                    LienLinkedIn = form["LienLinkedIn"].ToString(),
                };

                // Appeler le service pour mettre à jour le profil
                var result = await _profileService.UpdateFormateurProfile(formateurId, updatedFormateur);

                if (result)
                {
                    // Message de succès
                    TempData["SuccessMessage"] = "Profil mis à jour avec succès !";
                }
                else
                {
                    // Message d'erreur
                    TempData["ErrorMessage"] = "Une erreur est survenue lors de la mise à jour du profil.";
                }

                // Important : utilisez le TempData.Keep() pour conserver les messages
                TempData.Keep("SuccessMessage");
                TempData.Keep("ErrorMessage");

                // Rediriger vers Index après la mise à jour
                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Une erreur est survenue : {ex.Message}";
                TempData.Keep("ErrorMessage");
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            if (file == null)
                return null;

            return $"/images/formations/{file.FileName}";
        }

        //[HttpPost]
        //public async Task<IActionResult> AddFormation(IFormCollection form, IFormFile UrlImage)
        //{
        //    try
        //    {
        //        int formateurId = GetCurrentFormateurId();

        //        // Préparer les données de la formation
        //        var formationData = new Dictionary<string, object>
        //{
        //    { "Titre", form["Titre"].ToString() },
        //    { "CategoryId", int.Parse(form["CategoryId"]) },
        //    { "Description", form["Description"].ToString() },
        //    { "Prix", decimal.Parse(form["Prix"]) },
        //    { "EstimationDeDuree", int.Parse(form["EstimationDeDuree"]) },
        //    { "FormateurId", formateurId }
        //};

        //        // Upload de l'image
        //        if (UrlImage != null)
        //        {
        //            var imageUrl = _profileService.UploadImage(UrlImage);
        //            if (!string.IsNullOrEmpty(imageUrl))
        //            {
        //                formationData.Add("url_image", imageUrl);  // Notez que j'ai changé "UrlImage" en "url_image"
        //            }
        //        }

        //        // Ajouter la formation
        //        var result = await _profileService.AddFormation(formationData);

        //        if (result)
        //        {
        //            TempData["SuccessMessage"] = "Formation ajoutée avec succès.";

        //            // Ajouter les médias
        //            var mediaTitres = form["MediaTitre[]"].ToArray();
        //            var mediaTypes = form["MediaType[]"].ToArray();
        //            var mediaUrls = form["MediaUrl[]"].ToArray();
        //            var nombreDeSequences = form["NombreDeSequence[]"].ToArray();

        //            for (int i = 0; i < mediaTitres.Length; i++)
        //            {
        //                var mediaData = new Dictionary<string, object>
        //        {
        //            { "Titre", mediaTitres[i] },
        //            { "Type", mediaTypes[i] },
        //            { "Url", mediaUrls[i] },
        //            { "NombreDeSequence", int.Parse(nombreDeSequences[i]) }
        //        };

        //                await _profileService.AddMedia(mediaData);
        //            }
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "Erreur lors de l'ajout de la formation.";
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = $"Une erreur est survenue : {ex.Message}";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        // [HttpPost]
        // public async Task<IActionResult> AddFormation([FromForm] Formation formation,
        //[FromForm] List<string> MediaTitre,
        //[FromForm] List<string> MediaType,
        //[FromForm] List<string> MediaUrl,
        //[FromForm] List<int> NombreDeSequence)
        // {
        //     try
        //     {
        //         formation.FormateurId = GetCurrentFormateurId();

        //         // Récupérer le nom du fichier uniquement pour l'URL de l'image
        //         if (!string.IsNullOrEmpty(formation.url_image))
        //         {
        //             // Si formation.url_image est une URL ou un chemin, extrait le nom du fichier
        //             formation.url_image = Path.GetFileName(formation.url_image);
        //         }

        //         var newFormation = await _profileService.AddFormation(formation);

        //         // Ajouter les médias associés
        //         if (MediaTitre != null)
        //         {
        //             for (int i = 0; i < MediaTitre.Count; i++)
        //             {
        //                 var media = new Media
        //                 {
        //                     Title = MediaTitre[i],
        //                     Type = MediaType[i],
        //                     Url = MediaUrl[i],
        //                     FormationId = newFormation.Id,
        //                     nombredesequence = NombreDeSequence[i]
        //                 };

        //                 await _profileService.AddMedia(media);
        //             }
        //         }

        //         // Ajouter un message de succès dans TempData
        //         TempData["SuccessMessage"] = "La formation a été ajoutée avec succès!";

        //         return RedirectToAction(nameof(Index));
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Erreur lors de l'ajout de la formation");
        //         ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de la création de la formation");
        //         var profileData = await _profileService.GetProfileData(GetCurrentFormateurId());
        //         return View("Index", profileData);
        //     }
        // }

        // [HttpGet]
        // public async Task<IActionResult> AddFormation()
        // {
        //     try
        //     {
        //         var profileData = await _profileService.GetProfileData(GetCurrentFormateurId());
        //         return View(profileData);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Erreur lors du chargement du formulaire d'ajout de formation");
        //         return View("Error");
        //     }
        // }


        //  [HttpPost]
        //  public async Task<IActionResult> AddFormation([FromForm] Formation formation,
        //[FromForm] string[] MediaTitre,
        //[FromForm] string[] MediaType,
        //[FromForm] string[] MediaUrl,
        //[FromForm] int[] NombreDeSequence)
        //  {
        //      try
        //      {
        //          formation.FormateurId = GetCurrentFormateurId();

        //          var newFormation = await _profileService.AddFormation(formation);

        //          // Ajouter les médias associés si présents
        //          if (MediaTitre != null && MediaTitre.Length > 0)
        //          {
        //              for (int i = 0; i < MediaTitre.Length; i++)
        //              {
        //                  if (!string.IsNullOrEmpty(MediaTitre[i]))
        //                  {
        //                      var media = new Media
        //                      {
        //                          Title = MediaTitre[i],
        //                          Type = MediaType[i],
        //                          Url = MediaUrl[i],
        //                          FormationId = newFormation.Id,
        //                          nombredesequence = NombreDeSequence[i]
        //                      };

        //                      await _profileService.AddMedia(media);
        //                  }
        //              }
        //          }

        //          TempData["SuccessMessage"] = "La formation a été ajoutée avec succès!";
        //          return RedirectToAction(nameof(Index));
        //      }
        //      catch (Exception ex)
        //      {
        //          _logger.LogError(ex, "Erreur lors de l'ajout de la formation");
        //          ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de la création de la formation");
        //          var profileData = await _profileService.GetProfileData(GetCurrentFormateurId());
        //          return View("Index", profileData);
        //      }
        //  }

        //[HttpPost]
        //public async Task<IActionResult> AddFormation(Formation formation)
        //{
        //    try
        //    {
        //        // On s'assure que les données essentielles sont présentes
        //        if (formation == null || string.IsNullOrEmpty(formation.Titre) ||
        //            string.IsNullOrEmpty(formation.Description))
        //        {
        //            TempData["ErrorMessage"] = "Veuillez remplir tous les champs obligatoires";
        //            return RedirectToAction(nameof(Index));
        //        }

        //        // Définir le FormateurId (à remplacer par la vraie logique d'authentification)
        //        formation.FormateurId = 1;

        //        // Préparation des données pour l'API
        //        var formationToAdd = new Formation
        //        {
        //            Titre = formation.Titre,
        //            CategoryId = formation.CategoryId,
        //            Description = formation.Description,
        //            FormateurId = formation.FormateurId,
        //            Prix = formation.Prix,
        //            EstimationDeDuree = formation.EstimationDeDuree,
        //            url_image = formation.url_image
        //        };

        //        // Appel au service pour ajouter la formation
        //        var result = await _profileService.AddFormation(formationToAdd);

        //        if (result != null)
        //        {
        //            TempData["SuccessMessage"] = "Formation ajoutée avec succès!";
        //            return RedirectToAction(nameof(Index));
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "Erreur lors de l'ajout de la formation";
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "Une erreur s'est produite lors de l'ajout de la formation";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}


        //[HttpPost]
        //public async Task<IActionResult> AddFormation(IFormCollection form, IFormFile UrlImage)
        //{
        //    try
        //    {
        //        int formateurId = GetCurrentFormateurId();

        //        // Préparer les données de la formation dans le format correct
        //        var formationData = new Dictionary<string, object>
        //{
        //    { "Titre", form["Titre"].ToString() },
        //    { "CategoryId", form["CategoryId"].ToString() },
        //    { "Description", form["Description"].ToString() },
        //    { "FormateurId", formateurId },
        //    { "Prix", form["Prix"].ToString() },
        //    { "EstimationDeDuree", form["EstimationDeDuree"].ToString() }
        //};

        //        // Ajout de l'URL de l'image si présente
        //        if (UrlImage != null)
        //        {
        //            formationData.Add("url_image", $"/images/{UrlImage.FileName}");
        //        }

        //        // Ajouter la formation
        //        var result = await _profileService.AddFormation(formationData);

        //        if (result)
        //        {
        //            TempData["SuccessMessage"] = "Formation ajoutée avec succès.";
        //            // Ici vous pouvez ajouter la logique pour les médias si nécessaire
        //            return RedirectToAction(nameof(Index));
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "Erreur lors de l'ajout de la formation.";
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = $"Une erreur est survenue : {ex.Message}";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddFormation(IFormCollection form, IFormFile UrlImage)
        //{
        //    try
        //    {
        //        int formateurId = GetCurrentFormateurId();

        //        // Préparer les données de la formation
        //        var formationData = new Dictionary<string, object>
        //{
        //    { "Titre", form["Titre"].ToString() },
        //    { "CategoryId", int.Parse(form["CategoryId"]) },
        //    { "Description", form["Description"].ToString() },
        //    { "Prix", decimal.Parse(form["Prix"]) },
        //    { "EstimationDeDuree", int.Parse(form["EstimationDeDuree"]) },
        //    { "FormateurId", formateurId }
        //};

        //        // Upload de l'image si présente
        //        if (UrlImage != null)
        //        {
        //            formationData.Add("url_image", UrlImage.FileName);
        //        }

        //        // Ajouter la formation
        //        var result = await _profileService.AddFormation(formationData);
        //        if (result)
        //        {
        //            TempData["SuccessMessage"] = "Formation ajoutée avec succès.";

        //            // Récupérer l'ID de la formation créée (vous devez implémenter cette méthode)
        //            var formationId = await _profileService.GetLastCreatedFormationId(formateurId);

        //            // Ajouter les médias
        //            var mediaTitres = form["MediaTitre[]"].ToArray();
        //            var mediaTypes = form["MediaType[]"].ToArray();
        //            var mediaUrls = form["MediaUrl[]"].ToArray();
        //            var nombreDeSequences = form["NombreDeSequence[]"].ToArray();

        //            for (int i = 0; i < mediaTitres.Length; i++)
        //            {
        //                var mediaData = new Dictionary<string, object>
        //        {
        //            { "Title", mediaTitres[i] },
        //            { "Type", mediaTypes[i] },
        //            { "Url", mediaUrls[i] },
        //            { "FormationId", formationId },
        //            { "NombreDeSequence", int.Parse(nombreDeSequences[i]) }
        //        };

        //                await _profileService.AddMedia(mediaData);
        //            }
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "Erreur lors de l'ajout de la formation.";
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = $"Une erreur est survenue : {ex.Message}";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}



        [HttpPost]
        public async Task<IActionResult> AddFormation(IFormCollection form, IFormFile UrlImage)
        {
            try
            {
                int formateurId = GetCurrentFormateurId();

                _logger.LogInformation($"Données du formulaire reçues: {JsonSerializer.Serialize(form)}");

                var formationData = new Dictionary<string, object>
        {
            { "Titre", form["Titre"].ToString() },
            { "CategoryId", form["CategoryId"].ToString() },
            { "Description", form["Description"].ToString() },
            { "FormateurId", formateurId },
            { "Prix", form["Prix"].ToString() },
            { "EstimationDeDuree", form["EstimationDeDuree"].ToString() }
        };

                if (UrlImage != null)
                {
                    _logger.LogInformation($"Image reçue: {UrlImage.FileName}");
                    formationData.Add("url_image", $"/images/{UrlImage.FileName}");
                }

                // Ajouter la formation et récupérer l'ID directement de la réponse
                var formationResult = await _profileService.AddFormationAndGetId(formationData);

                if (formationResult.Success && formationResult.FormationId > 0)
                {
                    // Traiter les médias
                    var mediaTitres = form["MediaTitre[]"].ToArray();
                    var mediaTypes = form["MediaType[]"].ToArray();
                    var mediaUrls = form["MediaUrl[]"].ToArray();
                    var nombreDeSequences = form["NombreDeSequence[]"].ToArray();

                    bool allMediaAdded = true;
                    for (int i = 0; i < mediaTitres.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(mediaTitres[i]))
                        {
                            var mediaData = new Dictionary<string, object>
                    {
                        { "Title", mediaTitres[i] },
                        { "Type", mediaTypes[i] },
                        { "Url", mediaUrls[i] },
                        { "FormationId", formationResult.FormationId },
                        { "nombredesequence", int.Parse(nombreDeSequences[i]) }
                    };

                            var mediaResult = await _profileService.AddMedia(mediaData);
                            if (!mediaResult)
                            {
                                allMediaAdded = false;
                            }
                        }
                    }

                    TempData["SuccessMessage"] = allMediaAdded
                        ? "Formation et médias ajoutés avec succès."
                        : "Formation ajoutée mais certains médias n'ont pas pu être ajoutés.";

                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Erreur lors de l'ajout de la formation.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'ajout de la formation et des médias");
                TempData["ErrorMessage"] = $"Erreur : {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }





        [HttpPost]
        public async Task<IActionResult> DeleteFormation(int id)
        {
            try
            {
                int formateurId = GetCurrentFormateurId();
                var formation = await _profileService.GetFormationById(id);

                if (formation == null || formation.FormateurId != formateurId)
                {
                    return Unauthorized();
                }

                var result = await _profileService.DeleteFormation(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Formation supprimée avec succès.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Erreur lors de la suppression de la formation.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Une erreur est survenue : {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}