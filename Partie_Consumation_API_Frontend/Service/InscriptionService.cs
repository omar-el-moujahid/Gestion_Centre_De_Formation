using Partie_Api_Amd_Logique_Metier.Models;
using Partie_Consumation_API_Frontend.Models;
using System.Text;
using System.Text.Json;

namespace Partie_Consumation_API_Frontend.Service
{
    public class InscriptionService
    {
        private readonly HttpClient _httpClient;
        public InscriptionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Inscription> GetInscriptiony2Ids(int id_Formation = 1, int id_participant = 1)
        {
            var response = await _httpClient.GetAsync($"http://localhost:62869/api/Inscriptions?id_formation={id_Formation}&participant_id={id_participant}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}, Message: {await response.Content.ReadAsStringAsync()}");
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Inscription>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

       public async Task CreateInscription(Inscription inscription)
        {
            try
            {
                // Serialize the inscription object to JSON
                var inscriptionJson = JsonSerializer.Serialize(inscription);
                var content = new StringContent(inscriptionJson, Encoding.UTF8, "application/json");

                // Send POST request
                var response = await _httpClient.PostAsync("http://localhost:62869/api/Inscriptions", content);

                // Check if the response is successful
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
                    throw new HttpRequestException($"Request failed with status code: {response.StatusCode}. Details: {errorMessage}");
                }

                Console.WriteLine("Inscription created successfully.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP error: {ex.Message}");
                throw; // Re-throw the exception to handle it higher in the call stack if needed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw; // Re-throw to avoid swallowing the exception
            }
        }

        public async Task<List<InscriptionViewModel>> GetFormationsByParticipantIdWithState(int participantId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:62869/api/Inscriptions?participantId={participantId}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<InscriptionViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                return new List<InscriptionViewModel>();
            }
        }
        
    }
}
