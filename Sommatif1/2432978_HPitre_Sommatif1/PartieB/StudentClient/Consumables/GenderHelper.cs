using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StudentClient.Configs;
using StudentClient.Models;

namespace StudentClient.Consumables
{
    public static class GenderHelper
    {
        private static readonly HttpClient client = new()
        {
            BaseAddress = new Uri(ConfigHelper.localAPIBaseUrl)
        };

        #region GET all/id Methods
        public static async Task<List<Gender>?> GetAllAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("Gender");
                if (!response.IsSuccessStatusCode) return null;//If response = 200, return true, else false

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Gender>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Gender?> GetByIdAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"Gender/{id}"); //Pour spécifier quel dept, ajouter id dans l'url de requête
            if (!response.IsSuccessStatusCode) return null; //If response = 200, return true, else false

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Gender>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        #endregion
    }
}
