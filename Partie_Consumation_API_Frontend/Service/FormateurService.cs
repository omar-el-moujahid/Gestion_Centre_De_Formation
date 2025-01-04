
﻿using Newtonsoft.Json;
using Partie_Api_Amd_Logique_Metier.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Partie_Consumation_API_Frontend.Service
{
    public class FormateurService
    {
        private readonly HttpClient _httpClient;

        public FormateurService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<Formateur?> GetFormateurById(int id=1)

        {
            var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formateurs/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Formateur>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<Formation>> GetFormationsByFormateurId(int formateurId=3)
        {
            var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formations/ByFormateur/{formateurId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Formation>>(content, options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        public async Task<bool> UpdateFormateurProfile(int id, dynamic updatedFormateur)
        {
            try
            {
                // Récupérer les données actuelles pour le mot de passe
                var existingFormateur = await GetFormateurById(id);

                // Préparer les données à mettre à jour
                var formateurToUpdate = new
                {
                    Id = id,
                    Name = updatedFormateur.Name,
                    Prenom = updatedFormateur.Prenom,
                    Email = updatedFormateur.Email,
                    RoleId = 2, // Toujours fixé à 2
                    Specialite = updatedFormateur.Specialite,
                    Biographie = updatedFormateur.Biographie,
                    LienLinkedIn = updatedFormateur.LienLinkedIn,
                    Password = existingFormateur.Password // Garder le mot de passe actuel
                };

                // Envoyer la requête PUT
                var response = await _httpClient.PutAsJsonAsync($"http://localhost:62869/api/Formateurs/{id}", formateurToUpdate);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                return false;
            }
        }



        public async Task<bool> AddFormation(dynamic formation)
        {
            var response = await HttpClientExtensions.PostAsJsonAsync(_httpClient, "http://localhost:62869/api/Formations", formation);
            return response.IsSuccessStatusCode;
        }


        public async Task<bool> UpdateFormation(int id, dynamic formation)
        {
            var response = await HttpClientExtensions.PutAsJsonAsync(_httpClient, $"http://localhost:62869/api/Formations/{id}", formation);
            return response.IsSuccessStatusCode;
        }


        public async Task<bool> DeleteFormation(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:62869/api/Formations/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<dynamic> GetFormationById(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:62869/api/Formations/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<dynamic>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


    }

}

