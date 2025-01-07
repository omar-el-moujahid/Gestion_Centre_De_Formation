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

        //public async Task<bool> CreateOrUpdateEvaluationAsync(int participantId, int formationId, int rating, string review)
        //{
        //    Console.WriteLine($"participantId: {participantId}, formationId: {formationId}, rating: {rating}, review: {review}");

        //    if (participantId <= 0 || formationId <= 0 || rating <= 0 || rating > 5)
        //    {
        //        throw new ArgumentException("Invalid input parameters.");
        //    }

        //    try
        //    {
        //        // Check if an evaluation already exists
        //        var response = await _httpClient.GetAsync($"http://localhost:62869/api/Evaluations?formationId={formationId}&participantId={participantId}");

        //        if (response.IsSuccessStatusCode)
        //        {
        //            // If it exists, update it
        //            var existingEvaluationJson = await response.Content.ReadAsStringAsync();
        //            var existingEvaluation = JsonSerializer.Deserialize<Evaluation>(existingEvaluationJson);

        //            existingEvaluation.Rating = rating;
        //            existingEvaluation.Feedback = review;

        //            var updateJson = JsonSerializer.Serialize(existingEvaluation);
        //            var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");
        //            var updateResponse = await _httpClient.PutAsync($"http://localhost:62869/api/Evaluations/{existingEvaluation.Id}", updateContent);

        //            return updateResponse.IsSuccessStatusCode;
        //        }
        //        else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        //        {
        //            // Create a new evaluation if none exists
        //            var newEvaluation = new Evaluation
        //            {
        //                ParticipantId = participantId,
        //                FormationId = formationId,
        //                Rating = rating,
        //                Feedback = review
        //            };

        //            var newEvaluationJson = JsonSerializer.Serialize(newEvaluation);
        //            var content = new StringContent(newEvaluationJson, Encoding.UTF8, "application/json");
        //            var createResponse = await _httpClient.PostAsync("http://localhost:62869/api/Evaluations", content);

        //            return createResponse.IsSuccessStatusCode;
        //        }
        //        else
        //        {
        //            throw new HttpRequestException("Unexpected error when checking evaluation existence.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        throw;
        //    }
        //}
        public async Task<bool> CreateOrUpdateEvaluationAsync(int participantId, int formationId, int rating, string feedback)
        {
            // Validate parameters
            if (participantId <= 0 || formationId <= 0 || rating < 1 || rating > 5)
            {
                throw new ArgumentException("Invalid input parameters.");
            }

            try
            {
                // Check if an evaluation exists
                var response = await _httpClient.GetAsync($"http://localhost:62869/api/Evaluations?formationId={formationId}&participantId={participantId}");

                if (response.IsSuccessStatusCode)
                {
                    // Update existing evaluation
                    var existingEvaluationJson = await response.Content.ReadAsStringAsync();
                    var existingEvaluation = JsonSerializer.Deserialize<Evaluation>(existingEvaluationJson);

                    existingEvaluation.Rating = rating;
                    existingEvaluation.Feedback = feedback;

                    var updateJson = JsonSerializer.Serialize(existingEvaluation);
                    var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");
                    var updateResponse = await _httpClient.PutAsync($"http://localhost:62869/api/Evaluations/{existingEvaluation.Id}", updateContent);

                    return updateResponse.IsSuccessStatusCode;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Create a new evaluation
                    var newEvaluation = new Evaluation
                    {
                        ParticipantId = participantId,
                        FormationId = formationId,
                        Rating = rating,
                        Feedback = feedback
                    };

                    var newEvaluationJson = JsonSerializer.Serialize(newEvaluation);
                    var content = new StringContent(newEvaluationJson, Encoding.UTF8, "application/json");
                    var createResponse = await _httpClient.PostAsync("http://localhost:62869/api/Evaluations", content);

                    return createResponse.IsSuccessStatusCode;
                }
                else
                {
                    throw new HttpRequestException("Unexpected error while checking evaluation existence.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

    }
}
