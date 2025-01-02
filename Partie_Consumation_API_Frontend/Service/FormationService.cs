using Microsoft.VisualBasic;
using Partie_Api_Amd_Logique_Metier.Models;
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

        public async Task<Formation> GetFormationsbyid(int id =1)
        {
            var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formations/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Formation>(content, options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public async Task<List<Formation>> GetFormations()
        {
            var response = await _httpClient.GetAsync("http://localhost:62869/api/Formations");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Formation>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
