using Microsoft.AspNetCore.Http.Features;
using Microsoft.VisualBasic;
using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Models;
using System.Text.Json;
namespace Partie_Consumation_API_Frontend.Service
{
    public class FormationService
    {
        private readonly HttpClient _httpClient;

        public FormationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Formation> GetFormationsbyid(int id )
        {
            var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formations/{id}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}, Message: {await response.Content.ReadAsStringAsync()}");
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Formation>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        //public async Task<Formation> GetFormationsbyid(int id = 1)
        //{
        //    var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formations/{id}");
        //    response.EnsureSuccessStatusCode();

        //    var content = await response.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<Formation>(content, options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //}
        public async Task<List<Formation>> GetFormations()
        {
            var response = await _httpClient.GetAsync("http://localhost:62869/api/Formations");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Formation>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        //public async Task<List<FromationForMedia>> GetFormationMedias(int formationid)
        //{
        //    var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formations/{formationid}/Media");
        //    response.EnsureSuccessStatusCode();

        //    var content = await response.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<List<FromationForMedia>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //}
        public async Task<FromationForMedia> GetFormationWithMedia(int formationId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formations/{formationId}/Media");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FromationForMedia>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        public async Task<int> GetCFormationCount()
        {
            var response = await _httpClient.GetAsync("http://localhost:62869/api/Formations");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var formations = JsonSerializer.Deserialize<List<Formation>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return formations?.Count ?? 0; // Retourne 0 si la liste est nulle
        }



        public async Task<List<Category>> GetCategories()
        {
            var response = await _httpClient.GetAsync("http://localhost:62869/api/Categories");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Category>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        public async Task<List<Formation>> GetLastThreeFormationsByIdAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:62869/api/Formations");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var formations = JsonSerializer.Deserialize<List<Formation>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Trie par Id décroissant et retourne les 3 premiers
            return formations?
                .OrderByDescending(f => f.Id) // Trie les formations par Id décroissant
                .Take(5) // Prend les 3 premiers résultats
                .ToList() ?? new List<Formation>(); // Retourne une liste vide si null
        }

       


    }
}
