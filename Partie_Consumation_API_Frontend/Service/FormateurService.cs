
﻿using Newtonsoft.Json;
using Partie_Api_Amd_Logique_Metier.Models;
using System.Net.Http.Headers;
using System.Text.Json;

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
    }

}

