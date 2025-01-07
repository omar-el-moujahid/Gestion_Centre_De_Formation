using Microsoft.Extensions.Configuration;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace Partie_Consumation_API_Frontend.Service
{
    public class ProfileService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProfileService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            var baseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:62869/";
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        // Récupérer le profil du formateur par ID
        public async Task<Formateur?> GetProfileById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Formateurs/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Formateur>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        public async Task<Formateur?> GetFormateurById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Formateurs/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Formateur>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        // Mettre à jour le profil du formateur
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

        public async Task<List<Formation>> GetFormationsByFormateurId(int formateurId)
        {
            var response = await _httpClient.GetAsync($"api/Formations/ByFormateur/{formateurId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Formation>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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


        public async Task<List<Category>> GetCategories()
        {
            var response = await _httpClient.GetAsync("http://localhost:62869/api/Categories");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response: {content}");
            return JsonSerializer.Deserialize<List<Category>>(content);
        }


        // Téléverser une image de profil
        public async Task<string> UploadProfileImage(IFormFile file)
        {
            using var content = new MultipartFormDataContent();
            using var stream = file.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, "file", file.FileName);

            var response = await _httpClient.PostAsync("http://localhost:62869/api/upload/image", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
}
