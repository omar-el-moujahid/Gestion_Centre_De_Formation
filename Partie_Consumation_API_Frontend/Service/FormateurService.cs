using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Models;
using System.Configuration;
using System.Dynamic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Partie_Consumation_API_Frontend.Service
{
    public class FormateurService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FormateurService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            var baseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:62869/";
            _httpClient.BaseAddress = new Uri(baseUrl);
        }


        public async Task<Formateur?> GetFormateurById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Formateurs/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Formateur>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<Formation>> GetFormationsByFormateurId(int formateurId)
        {
            var response = await _httpClient.GetAsync($"api/Formations/ByFormateur/{formateurId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Formation>>(content,
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

                var response = await _httpClient.PutAsJsonAsync($"api/Formateurs/{id}", formateurToUpdate);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                return false;
            }
        }







        public async Task<bool> AddFormation(dynamic formation)
        {
            var response = await HttpClientExtensions.PostAsJsonAsync(_httpClient, "http://localhost:62869/api/Formations", formation);
            return response.IsSuccessStatusCode;
        }


        // Méthode pour récupérer une formation par ID
        public async Task<Formation> GetFormationByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formations/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Erreur lors de la récupération de la formation.");
            }

            var formation = await response.Content.ReadFromJsonAsync<Formation>();
            return formation;
        }

        public async Task<Formation> GetFormationById(int id)
        {
            // Logique pour récupérer une formation par ID depuis l'API ou la base de données
            var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formations/{id}");
            if (response.IsSuccessStatusCode)
            {
                var formation = await response.Content.ReadAsAsync<Formation>();
                return formation;
            }
            return null;
        }

        public async Task<bool> UpdateFormation(int id, Formation updatedFormation)
        {
            // Logique pour mettre à jour une formation via l'API ou la base de données
            var response = await _httpClient.PutAsJsonAsync($"http://localhost:62869/api/Formations/{id}", updatedFormation);
            return response.IsSuccessStatusCode;
        }

        // Récupérer la liste des catégories
        public async Task<List<dynamic>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:62869/api/Categories");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Erreur lors de la récupération des catégories.");
            }

            var categories = await response.Content.ReadFromJsonAsync<List<dynamic>>();
            return categories ?? new List<dynamic>();
        }


        public async Task<IEnumerable<Formation>> GetAllFormations()
        {
            // Logique pour récupérer toutes les formations
            var response = await _httpClient.GetAsync("http://localhost:62869/api/Formations");
            if (response.IsSuccessStatusCode)
            {
                var formations = await response.Content.ReadAsAsync<IEnumerable<Formation>>();
                return formations;
            }
            return new List<Formation>();
        }


        public async Task<bool> AddFormation(Dictionary<string, object> formationData)
        {
            var json = JsonSerializer.Serialize(formationData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:62869/api/Formations", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Category>> GetCategories()
        {
            var response = await _httpClient.GetAsync("http://localhost:62869/api/Categories");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response: {content}");
            return JsonSerializer.Deserialize<List<Category>>(content);
        }


        public async Task<string> UploadImage(IFormFile file)
        {
            using var content = new MultipartFormDataContent();
            using var stream = file.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, "file", file.FileName);

            var response = await _httpClient.PostAsync("http://localhost:62869/api/upload/image", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }





        public async Task<bool> DeleteFormation(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:62869/api/Formations/{id}");
            return response.IsSuccessStatusCode;
        }


       



    }

}

