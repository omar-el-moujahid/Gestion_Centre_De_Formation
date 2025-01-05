
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Partie_Api_Amd_Logique_Metier.Models;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Partie_Consumation_API_Frontend.Service
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async void CreateStudents(Participant newstudent)
        {
            // Serialize the Formation object to JSON
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(newstudent),
                Encoding.UTF8,
                "application/json"
            );

            // Send a POST request to the API
            var response = await _httpClient.PostAsync("http://localhost:62869/api/Participants", jsonContent);

            // Handle unsuccessful responses
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}, Message: {await response.Content.ReadAsStringAsync()}");
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }

            //// Deserialize the response content to a Formation object
            //var content = await response.Content.ReadAsStringAsync();
            //return JsonSerializer.Deserialize<Formation>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<Participant> authparticipant(string mail, string password)
        {
            try
            {
                // Make the HTTP GET request to the API
                var response = await _httpClient.GetAsync($"http://localhost:62869/api/Participants?mail={mail}&password={password}");

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and deserialize the response content into a Participant object
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Participant participant = JsonConvert.DeserializeObject<Participant>(jsonResponse);

                    // Return the participant object
                    return participant;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Handle the case where the participant is not found
                    return null; // or throw an exception if preferred
                }
                else
                {
                    // Handle other HTTP errors
                    throw new HttpRequestException($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., network errors, JSON deserialization errors)
                throw new Exception("An error occurred while authenticating the participant.", ex);
            }
        }



        public async Task<Admin> authadmin(string mail, string password)
        {
            try
            {
                // Make the HTTP GET request to the API
                var response = await _httpClient.GetAsync($"http://localhost:62869/api/Admins?mail={mail}&password={password}");

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and deserialize the response content into a Participant object
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var Admine = JsonConvert.DeserializeObject<Admin>(jsonResponse);

                    // Return the participant object
                    return Admine;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Handle the case where the participant is not found
                    return null; // or throw an exception if preferred
                }
                else
                {
                    // Handle other HTTP errors
                    throw new HttpRequestException($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., network errors, JSON deserialization errors)
                throw new Exception("An error occurred while authenticating the participant.", ex);
            }
        }





        public async Task<Formateur> authformateur(string mail, string password)
        {
            try
            {
                // Make the HTTP GET request to the API
                var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formateurs?mail={mail}&password={password}");

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and deserialize the response content into a Participant object
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var Formateu = JsonConvert.DeserializeObject<Formateur>(jsonResponse);

                    // Return the participant object
                    return Formateu;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Handle the case where the participant is not found
                    return null; // or throw an exception if preferred
                }
                else
                {
                    // Handle other HTTP errors
                    throw new HttpRequestException($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., network errors, JSON deserialization errors)
                throw new Exception("An error occurred while authenticating the participant.", ex);
            }
        }
    }

}
