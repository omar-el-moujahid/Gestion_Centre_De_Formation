using Microsoft.Extensions.Configuration;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using Media = Partie_Consumation_API_Frontend.Models.Media;

namespace Partie_Consumation_API_Frontend.Service
{
    public class ProfileService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProfileService> _logger;


        public ProfileService(HttpClient httpClient, IConfiguration configuration, ILogger<ProfileService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            var baseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:62869/";
            _httpClient.BaseAddress = new Uri(baseUrl);
            _logger = logger;

        }

        public async Task<Formateur?> GetFormateurById(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formateurs/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<Formateur>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> UpdateFormateurProfile(int id, dynamic updatedFormateur)
        {
            try
            {
                var existingFormateur = await GetFormateurById(id);
                if (existingFormateur == null) return false;

                var formateurToUpdate = new
                {
                    Id = id,
                    Name = updatedFormateur.Name,
                    Prenom = updatedFormateur.Prenom,
                    Email = updatedFormateur.Email,
                    RoleId = existingFormateur.RoleId,
                    Specialite = updatedFormateur.Specialite,
                    Biographie = updatedFormateur.Biographie,
                    LienLinkedIn = updatedFormateur.LienLinkedIn,
                    Password = existingFormateur.Password
                };

                var response = await _httpClient.PutAsJsonAsync($"http://localhost:62869/api/Formateurs/{id}", formateurToUpdate);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                return false;
            }
        }

        public async Task<List<Formation>> GetFormationsByFormateurId(int formateurId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formations/ByFormateur/{formateurId}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<List<Formation>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des formations : {ex.Message}");
                return new List<Formation>();
            }
        }

        public async Task<Formation?> GetFormationById(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formations/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<Formation>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<Category>> GetCategories()
        {
            var response = await _httpClient.GetAsync("http://localhost:62869/api/Categories");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<List<Category>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }



        //public async Task<bool> AddFormation(Dictionary<string, object> formationData)
        //{
        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync("http://localhost:62869/api/Formations", formationData);
        //        return response.IsSuccessStatusCode;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}



        //public async Task<string> UploadImage(IFormFile file)
        //{
        //    if (file == null) return null;

        //    using var content = new MultipartFormDataContent();
        //    using var stream = file.OpenReadStream();
        //    var fileContent = new StreamContent(stream);
        //    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
        //    content.Add(fileContent, "file", file.FileName);

        //    var response = await _httpClient.PostAsync("http://localhost:62869/api/upload/image", content);
        //    return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        //}

        public async Task<bool> DeleteFormation(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"http://localhost:62869/api/Formations/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression de la formation : {ex.Message}");
                return false;
            }
        }


        public async Task<List<Inscription>> GetInscriptionsByFormateurId(int formateurId)
        {
            try
            {
                // Appel de l'API pour récupérer les inscriptions du formateur
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Formateurs/{formateurId}/inscriptions");

                if (response.IsSuccessStatusCode)
                {
                    // Désérialisation du contenu JSON de la réponse
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var inscriptions = JsonConvert.DeserializeObject<List<Inscription>>(jsonString);

                    return inscriptions;
                }
                else
                {
                    // Si l'API retourne un code d'erreur, on peut gérer l'erreur ici (par exemple, lancer une exception ou retourner une liste vide)
                    return new List<Inscription>();
                }
            }
            catch (Exception ex)
            {
                // Gérer les exceptions en cas d'erreur de communication
                Console.WriteLine($"Erreur lors de l'appel API : {ex.Message}");
                return new List<Inscription>(); // Retourner une liste vide en cas d'erreur
            }
        }




        public async Task<ProfileViewModel> GetProfileData(int formateurId)
        {
            var profileData = new ProfileViewModel
            {
                Formateur = await GetFormateurById(formateurId),
                Formations = await GetFormationsByFormateurId(formateurId),
                Categories = await GetCategories(),
                Inscriptions = await GetInscriptionsByFormateurId(formateurId),
                Media = await GetMedia(formateurId)
            };

            return profileData;
        }






        private async Task<List<Media>> GetMedia(int formationId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:62869/api/Media?FormationId={formationId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Media>>();
        }

        public string UploadImage(IFormFile file)
        {
            if (file == null)
                return null;

            // Retourne simplement le nom du fichier comme URL
            return $"/images/{file.FileName}";
        }

        //public async Task<bool> AddFormation(Dictionary<string, object> formationData)
        //{
        //    try
        //    {
        //        // Log des données reçues
        //        _logger.LogInformation($"Données reçues: {System.Text.Json.JsonSerializer.Serialize(formationData)}");

        //        var formationJson = new
        //        {
        //            Titre = formationData["Titre"].ToString(),
        //            CategoryId = int.Parse(formationData["CategoryId"].ToString()),
        //            Description = formationData["Description"].ToString(),
        //            FormateurId = int.Parse(formationData["FormateurId"].ToString()),
        //            Prix = int.Parse(formationData["Prix"].ToString()),
        //            EstimationDeDuree = int.Parse(formationData["EstimationDeDuree"].ToString()),
        //            url_image = formationData.ContainsKey("url_image") ? formationData["url_image"].ToString() : null
        //        };

        //        // Log de l'objet JSON à envoyer
        //        _logger.LogInformation($"JSON à envoyer: {System.Text.Json.JsonSerializer.Serialize(formationJson)}");

        //        var response = await _httpClient.PostAsJsonAsync($"http://localhost:62869/api/Formations", formationJson);

        //        // Log de la réponse
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        _logger.LogInformation($"Réponse de l'API: Status={response.StatusCode}, Content={responseContent}");

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            _logger.LogError($"Erreur API: {response.StatusCode} - {responseContent}");
        //            throw new Exception($"Erreur API: {responseContent}");
        //        }

        //        return response.IsSuccessStatusCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception dans AddFormation: {ex.Message}");
        //        throw; // Relancer l'exception pour la gérer dans le contrôleur
        //    }
        //}

        public async Task<FormationResult> AddFormationAndGetId(Dictionary<string, object> formationData)
        {
            try
            {
                var formationJson = new
                {
                    Titre = formationData["Titre"].ToString(),
                    CategoryId = int.Parse(formationData["CategoryId"].ToString()),
                    Description = formationData["Description"].ToString(),
                    FormateurId = int.Parse(formationData["FormateurId"].ToString()),
                    Prix = int.Parse(formationData["Prix"].ToString()),
                    EstimationDeDuree = int.Parse(formationData["EstimationDeDuree"].ToString()),
                    url_image = formationData.ContainsKey("url_image") ? formationData["url_image"].ToString() : null
                };

                _logger.LogInformation($"Envoi des données de formation: {System.Text.Json.JsonSerializer.Serialize(formationJson)}");

                var response = await _httpClient.PostAsJsonAsync($"http://localhost:62869/api/Formations", formationJson);
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Réponse de l'API: {content}");

                if (response.IsSuccessStatusCode)
                {
                    var createdFormation = await response.Content.ReadFromJsonAsync<Formation>();
                    if (createdFormation != null)
                    {
                        return new FormationResult { Success = true, FormationId = createdFormation.Id };
                    }
                }

                return new FormationResult { Success = false, FormationId = 0 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'ajout de la formation");
                return new FormationResult { Success = false, FormationId = 0 };
            }
        }

        public async Task<bool> AddMedia(Dictionary<string, object> mediaData)
        {
            try
            {
                var mediaJson = new
                {
                    Title = mediaData["Title"].ToString(),
                    Type = mediaData["Type"].ToString(),
                    Url = mediaData["Url"].ToString(),
                    FormationId = int.Parse(mediaData["FormationId"].ToString()),
                    nombredesequence = int.Parse(mediaData["nombredesequence"].ToString())
                };

                _logger.LogInformation($"Envoi des données média: {System.Text.Json.JsonSerializer.Serialize(mediaJson)}");

                var response = await _httpClient.PostAsJsonAsync($"http://localhost:62869/api/Media", mediaJson);

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erreur lors de l'ajout du média: {content}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'ajout du média");
                return false;
            }
        }




        public async Task<int> GetLastCreatedFormationId(int formateurId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:62869/Formations?formateurId={formateurId}");
                if (response.IsSuccessStatusCode)
                {
                    var formations = await response.Content.ReadFromJsonAsync<List<Formation>>();
                    return formations.OrderByDescending(f => f.Id).First().Id;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }




    }

}