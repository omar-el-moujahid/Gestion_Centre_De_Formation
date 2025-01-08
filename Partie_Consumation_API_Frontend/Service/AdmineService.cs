
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Partie_Api_Amd_Logique_Metier.Models;
using System.Net;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;



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


                if (response.IsSuccessStatusCode)
                {
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



        public async Task<Formateur> formateurEmail(string email)
        {
            try
            {
                // Make the HTTP GET request to the API
                var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formateurs?mail={email}");

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and deserialize the response content into a Participant object
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var formateur = JsonConvert.DeserializeObject<Formateur>(jsonResponse);

                    // Return the participant object
                    return formateur;
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



        public async Task CreateFormateurAsync(Formateur newFormateur)
        {
            try
            {
                // Serialize the Admin object to JSON
                string jsonFormateur = JsonConvert.SerializeObject(newFormateur);
                var jsonContent = new StringContent(jsonFormateur, Encoding.UTF8, "application/json");

                // Send a POST request to the API
                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:62869/api/Formateurs", jsonContent);

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



        public async Task<List<Formateur>> GetAllFormateurAsync()
        {
            try
            {
                // Envoi d'une requête GET à l'API
                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:62869//api/Formateurs");

                // Vérification du succès de la réponse
                if (response.IsSuccessStatusCode)
                {
                    // Lecture et désérialisation du contenu JSON
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var formateur = JsonConvert.DeserializeObject<List<Formateur>>(jsonResponse);

                    return formateur;
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




        public async Task<List<Formation>> GetFormationsByFormateurAsync(int formateurId)
        {
           
                // Construire l'URL pour récupérer les formations d'un formateur
                string url = $"http://localhost:62869/api/Formations/ByIdFormateur/{formateurId}";

                // Envoyer une requête GET à l'API
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                // Vérifier si la réponse est un succès
                if (response.IsSuccessStatusCode)
                {
                    // Lire la réponse JSON
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Désérialiser la réponse en une liste de formations
                    List<Formation> formations = JsonConvert.DeserializeObject<List<Formation>>(jsonResponse);

                    return formations; // Retourner la liste des formations
                }
                else
                {
                     return null;
                }
                   
           
        }








        public async Task<bool> UpdateFormateurAsync(int id, Formateur formateur)
        {
            
                // Sérialiser l'objet Formateur en JSON
                string jsonFormateur = JsonConvert.SerializeObject(formateur);
                var jsonContent = new StringContent(jsonFormateur, Encoding.UTF8, "application/json");

                // Envoyer une requête PUT à l'API
                HttpResponseMessage response = await _httpClient.PutAsync($"http://localhost:62869/api/Formateurs/{id}", jsonContent);

                // Vérifier si la requête a réussi
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Formateur mis à jour avec succès !");
                    return true;
                }
                else
                {
                   
                    return false;
                }
            
        }









        public async Task<Formateur> GetFormateurByIdAsync(int id)
        {
           
                // Construire l'URL de l'API
                string url = $"http://localhost:62869/api/Formateurs/{id}";

                // Envoyer une requête GET à l'API
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                // Vérifier si la réponse est un succès
                if (response.IsSuccessStatusCode)
                {
                    // Lire et désérialiser la réponse JSON en un objet Formateur
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Formateur formateur = JsonConvert.DeserializeObject<Formateur>(jsonResponse);

                    return formateur; // Retourner le formateur récupéré
                }
                else
                {
                    return null;
                }
           
        }








        public async Task<bool> DeleteFormateurAsync(int id)
        {


            try
            {

                HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:62869/api/Formateurs/{id}");


                if (response.IsSuccessStatusCode)
                {
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





        public async Task<List<Category>> GetAllCategoryAsync()
        {
            try
            {
                // Envoi d'une requête GET à l'API
                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:62869/api/Categories");

                // Vérification du succès de la réponse
                if (response.IsSuccessStatusCode)
                {
                    // Lecture et désérialisation du contenu JSON
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var category = JsonConvert.DeserializeObject<List<Category>>(jsonResponse);

                    return category;
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



        public async Task<Category> categoryName(string name)
        {
            try
            {
                // Make the HTTP GET request to the API
                var response = await _httpClient.GetAsync($"http://localhost:62869/api/Categories?Name={name}");

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and deserialize the response content into a Participant object
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var category = JsonConvert.DeserializeObject<Category>(jsonResponse);

                    // Return the participant object
                    return category;
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




        public async Task CreateCategoryAsync(Category newCategory)
        {
            try
            {
                // Serialize the Admin object to JSON
                string jsonCategory = JsonConvert.SerializeObject(newCategory);
                var jsonContent = new StringContent(jsonCategory, Encoding.UTF8, "application/json");

                // Send a POST request to the API
                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:62869/api/Categories", jsonContent);

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




        public async Task<Category> GetCategoryByIdAsync(int id)
        {

            // Construire l'URL de l'API
            string url = $"http://localhost:62869/api/Categories/{id}";

            // Envoyer une requête GET à l'API
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // Vérifier si la réponse est un succès
            if (response.IsSuccessStatusCode)
            {
                // Lire et désérialiser la réponse JSON en un objet Formateur
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Category category = JsonConvert.DeserializeObject<Category>(jsonResponse);

                return category; // Retourner le formateur récupéré
            }
            else
            {
                return null;
            }

        }


        public async Task<List<Formation>> GetFormationsByCategoryAsync(int categoryId)
        {

            // Construire l'URL pour récupérer les formations d'un formateur
            string url = $"http://localhost:62869/api/Formations/ByIdCategory/{categoryId}";

            // Envoyer une requête GET à l'API
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // Vérifier si la réponse est un succès
            if (response.IsSuccessStatusCode)
            {
                // Lire la réponse JSON
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Désérialiser la réponse en une liste de formations
                List<Formation> formations = JsonConvert.DeserializeObject<List<Formation>>(jsonResponse);

                return formations; // Retourner la liste des formations
            }
            else
            {
                return null;
            }


        }




        public async Task<bool> DeleteCategoryAsync(int id)
        {


            try
            {

                HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:62869/api/Categories/{id}");


                if (response.IsSuccessStatusCode)
                {
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



        public async Task<List<Formation>> GetAllFormationAsync()
        {
            try
            {
                // Envoi d'une requête GET à l'API
                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:62869/api/Formations");

                // Vérification du succès de la réponse
                if (response.IsSuccessStatusCode)
                {
                    // Lecture et désérialisation du contenu JSON
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var formation = JsonConvert.DeserializeObject<List<Formation>>(jsonResponse);

                    return formation;
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





        public async Task<Formation> GetFormationByIdAsync(int id)
        {

            // Construire l'URL de l'API
            string url = $"http://localhost:62869/api/Formations{id}";

            // Envoyer une requête GET à l'API
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // Vérifier si la réponse est un succès
            if (response.IsSuccessStatusCode)
            {
                // Lire et désérialiser la réponse JSON en un objet Formateur
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Formation formation = JsonConvert.DeserializeObject<Formation>(jsonResponse);

                return formation; // Retourner le formateur récupéré
            }
            else
            {
                return null;
            }

        }


        public async Task<List<Inscription>> GetInscriptionByFormationAsync(int FormationId)
        {

            // Construire l'URL pour récupérer les formations d'un formateur
            string url = $"http://localhost:62869/api/Inscriptions?FormationId={FormationId}";

            // Envoyer une requête GET à l'API
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // Vérifier si la réponse est un succès
            if (response.IsSuccessStatusCode)
            {
                // Lire la réponse JSON
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Désérialiser la réponse en une liste de formations
                List<Inscription> inscriptio = JsonConvert.DeserializeObject<List<Inscription>>(jsonResponse);

                return inscriptio; // Retourner la liste des formations
            }
            else
            {
                return null;
            }


        }



        public async Task<bool> DeleteFormationAsync(int id)
        {


            try
            {

                HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:62869/api/Inscriptions/{id}");


                if (response.IsSuccessStatusCode)
                {
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


