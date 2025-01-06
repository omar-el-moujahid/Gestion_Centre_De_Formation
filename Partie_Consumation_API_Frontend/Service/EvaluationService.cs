using Partie_Api_Amd_Logique_Metier.Models;
using System.Text;
using System.Text.Json;

namespace Partie_Consumation_API_Frontend.Service
{
    public class EvaluationService
    {
        private readonly HttpClient _httpClient;

        public EvaluationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CreateOrUpdateEvaluationAsync(int participantId, int formationId, int rating, string review)
        {
            // Vérifier les paramètres
            if (participantId <= 0 || formationId <= 0 || rating <= 0 || rating > 5)
            {
                throw new ArgumentException("Invalid input parameters.");
            }

            try
            {
                // Vérifier si une évaluation existe déjà
                var response = await _httpClient.GetAsync($"http://localhost:62869/api/Evaluations/{formationId}/{participantId}");

                if (response.IsSuccessStatusCode)
                {
                    // Si une évaluation existe, la récupérer
                    var existingEvaluationJson = await response.Content.ReadAsStringAsync();
                    var existingEvaluation = JsonSerializer.Deserialize<Evaluation>(existingEvaluationJson);

                    // Mettre à jour l'évaluation existante
                    existingEvaluation.Rating = rating;
                    existingEvaluation.Feedback = review;

                    // Envoyer une requête PUT pour mettre à jour
                    var updateJson = JsonSerializer.Serialize(existingEvaluation);
                    var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");
                    var updateResponse = await _httpClient.PutAsync($"http://localhost:62869/api/Evaluations/{existingEvaluation.Id}", updateContent);

                    return updateResponse.IsSuccessStatusCode; // Retourne true si l'opération réussit
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Si aucune évaluation n'existe, créer une nouvelle
                    var newEvaluation = new Evaluation
                    {
                        ParticipantId = participantId,
                        FormationId = formationId,
                        Rating = rating,
                        Feedback = review
                    };

                    // Sérialiser l'objet en JSON
                    var newEvaluationJson = JsonSerializer.Serialize(newEvaluation);
                    var content = new StringContent(newEvaluationJson, Encoding.UTF8, "application/json");

                    // Envoyer une requête POST pour créer une nouvelle évaluation
                    var createResponse = await _httpClient.PostAsync("http://localhost:62869/api/Evaluations", content);

                    return createResponse.IsSuccessStatusCode; // Retourne true si la création réussit
                }
                else
                {
                    // Gérer les autres erreurs HTTP
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error checking evaluation existence: {errorMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Gestion des erreurs HTTP
                Console.WriteLine($"HTTP error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Gestion des autres erreurs
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }
    }
}

   