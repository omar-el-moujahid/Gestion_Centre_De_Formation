using Newtonsoft.Json;
using Partie_Api_Amd_Logique_Metier.Models;
using System.Text;



namespace Partie_Consumation_API_Frontend.Service
{
    public class AdmineService
    {
        private readonly HttpClient _httpClient;
        public AdmineService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task CreateAdminAsync(Admin newAdmin)
        {
            try
            {
                // Serialize the Admin object to JSON
                string jsonAdmin = JsonConvert.SerializeObject(newAdmin);
                var jsonContent = new StringContent(jsonAdmin, Encoding.UTF8, "application/json");

                // Send a POST request to the API
                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:62869/api/Admins", jsonContent);

                // Check if the response indicates success
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorMessage}");
                }

                Console.WriteLine("Admin created successfully!");
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Re-throw the exception for higher-level handling
            }
        }


        public async Task<Admin> adminEmail(string mail)
        {
            try
            {
                // Make the HTTP GET request to the API
                var response = await _httpClient.GetAsync($"http://localhost:62869/api/Admins?mail={mail}");

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


        public async Task<List<Admin>> GetAllAdminsAsync()
        {
            try
            {
                // Envoi d'une requête GET à l'API
                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:62869//api/Admins");

                // Vérification du succès de la réponse
                if (response.IsSuccessStatusCode)
                {
                    // Lecture et désérialisation du contenu JSON
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var admins = JsonConvert.DeserializeObject<List<Admin>>(jsonResponse);

                    return admins;
                }
                else
                {
                    // Gestion des erreurs HTTP
                    throw new HttpRequestException($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Gestion des exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }



        public async Task<bool> DeleteAdminAsync(int id)
        {
           
                try
                {
                    
                    HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:62869/api/Admins/{id}");


                    if (response.IsSuccessStatusCode) {
                        return true; // Suppression réussie
                    }
                    else
                    {
                        return false;
                    }

                    
                }
                catch (Exception ex)
                {
                    // Gérer les exceptions
                    Console.WriteLine($"Exception: {ex.Message}");
                    return false;
                }
            
        }


    }
}
