using Partie_Api_Amd_Logique_Metier;
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

        public async Task<bool> UpdateInscriptionStatus(int formationId, int participantId)
        {
            try
            {
                // Get the current inscription
                var inscription = await GetInscriptiony2Ids(formationId, participantId);
                if (inscription == null)
                {
                    Console.WriteLine($"No inscription found for Formation {formationId} and Participant {participantId}");
                    return false;
                }

                // Update status
                inscription.Statut = Statut.Completed;

                // Serialize and send update request
                var inscriptionJson = JsonSerializer.Serialize(inscription);
                var content = new StringContent(inscriptionJson, Encoding.UTF8, "application/json");

                // Send PUT request
                var response = await _httpClient.PutAsync(
                    $"http://localhost:62869/api/Inscriptions/{inscription.Id}",
                    content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to update inscription status: {errorMessage}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating inscription status: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }
    }
}
