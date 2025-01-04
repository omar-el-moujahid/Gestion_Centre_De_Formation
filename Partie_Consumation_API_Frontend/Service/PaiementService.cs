using Partie_Api_Amd_Logique_Metier.Models;
using System.Text;
using System.Text.Json;

namespace Partie_Consumation_API_Frontend.Service
{
    public class PaiementService
    {
        private readonly HttpClient _httpClient;

        public PaiementService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Payment> GetPayementBy2Ids(int id_Formation = 1 , int id_participant=1)
        {
            var response = await _httpClient.GetAsync($"api/Payments?formation_id={id_Formation}&participant_id={id_participant}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}, Message: {await response.Content.ReadAsStringAsync()}");
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Payment>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        //public async Task<Payment> CreatePayment(Payment payment)
        //{
        //    var paymentJson = JsonSerializer.Serialize(payment);
        //    var content = new StringContent(paymentJson, Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync("api/Payments", content);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        Console.WriteLine($"Error: {response.StatusCode}, Message: {await response.Content.ReadAsStringAsync()}");
        //        throw new HttpRequestException($"Error: {response.StatusCode}");
        //    }

        //    var responseData = await response.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<Payment>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //}
        public async Task CreatePayment(Payment payment)
        {
            var paymentJson = JsonSerializer.Serialize(payment);
            var content = new StringContent(paymentJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:62869/api/Payments", content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}, Message: {await response.Content.ReadAsStringAsync()}");
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }
        }

    }
}
