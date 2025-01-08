using Partie_Api_Amd_Logique_Metier.Models;
using System.Text;
using System.Text.Json;

namespace Partie_Consumation_API_Frontend.Service
{
    public class CertificateService
    {
        private readonly HttpClient _httpClient;
        public CertificateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
       
        public async Task<bool> CreateCertificate(int formationId, int participantId, string formationTitre)
        {
            try
            {
                Console.WriteLine($"Creating certificate with FormationId: {formationId}, ParticipantId: {participantId}, Titre: {formationTitre}");

                var certificate = new Certificate
                {
                    FormationId = formationId,
                    ParticipantId = participantId,
                    DelivranceDate = DateTime.Now,
                    Titre = formationTitre?.Trim()
                };

                // Validate certificate object
                if (string.IsNullOrEmpty(certificate.Titre))
                {
                    Console.WriteLine("Certificate title is required");
                    return false;
                }

                var certificateJson = JsonSerializer.Serialize(certificate);
                Console.WriteLine($"Sending request to API: {certificateJson}");

                using var content = new StringContent(certificateJson, Encoding.UTF8, "application/json");
                using var response = await _httpClient.PostAsync("http://localhost:62869/api/Certificates", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {response.StatusCode}, Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Certificate Service Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
