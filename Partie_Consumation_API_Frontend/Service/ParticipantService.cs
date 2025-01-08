using Partie_Api_Amd_Logique_Metier.Models;
using System.Text.Json;

namespace Partie_Consumation_API_Frontend.Service
{
    public class ParticipantService
    {
        private readonly HttpClient _httpClient;
        public ParticipantService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Participant?> GetParticipantById(int id )
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:62869/api/Participants/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Participant>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                // Gestion d'erreur : log ou exception
                throw new Exception($"Erreur lors de la récupération du participant : {ex.Message}");
            }
        }

        //public async Task<bool> UpdateFormateurProfile(int id, dynamic updatedFormateur)
        //{
        //    try
        //    {
        //        // Récupérer les données actuelles pour le mot de passe
        //        var existingFormateur = await GetParticipantById(id);

        //        // Préparer les données à mettre à jour
        //        var formateurToUpdate = new
        //        {
        //            Id = id,
        //            Name = updatedFormateur.Name,
        //            Prenom = updatedFormateur.Prenom,
        //            Email = updatedFormateur.Email,
        //            RoleId = 3, 
        //            Password = existingFormateur.Password // Garder le mot de passe actuel
        //        };

        //        // Envoyer la requête PUT
        //        var response = await _httpClient.PutAsJsonAsync($"http://localhost:62869/api/Participants/{id}", formateurToUpdate);

        //        return response.IsSuccessStatusCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Erreur : {ex.Message}");
        //        return false;
        //    }
        //}
        public async Task<bool> UpdateFormateurProfile(int id, dynamic updatedFormateur)
        {
            try
            {
                // Récupérer les données actuelles pour le mot de passe
                var existingFormateur = await GetParticipantById(id);

                if (existingFormateur == null)
                    throw new Exception("Participant introuvable.");

                // Préparer les données à mettre à jour
                var formateurToUpdate = new
                {
                    Id = id,
                    Name = updatedFormateur.Name,
                    Prenom = updatedFormateur.Prenom,
                    Email = updatedFormateur.Email,
                    RoleId = 3, // Fixé à 3 pour ce cas
                    Password = existingFormateur.Password // Garder le mot de passe actuel
                };

                // Envoyer la requête PUT
                var response = await _httpClient.PutAsJsonAsync($"http://localhost:62869/api/Participants/{id}", formateurToUpdate);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour : {ex.Message}");
                return false;
            }
        }

        public async Task<int> GetCParticipantCount()
        {
            var response = await _httpClient.GetAsync("http://localhost:62869/api/Participants");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var participants = JsonSerializer.Deserialize<List<Participant>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return participants?.Count ?? 0; // Retourne 0 si la liste est nulle
        }


    }
}
