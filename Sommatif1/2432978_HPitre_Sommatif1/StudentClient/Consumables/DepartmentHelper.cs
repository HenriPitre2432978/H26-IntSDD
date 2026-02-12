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
    public static class DepartmentApiHelper
    {
        private static readonly HttpClient client = new()
        {
            BaseAddress = new Uri(ConfigHelper.localAPIBaseUrl)
        };

        #region GET all/id Methods
        public static async Task<List<Department>?> GetAllAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("Department");
                if (!response.IsSuccessStatusCode) return null;//If response = 200, return true, else false

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Department>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Department?> GetByIdAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"Department/{id}"); //Pour spécifier quel dept, ajouter id dans l'url de requête
            if (!response.IsSuccessStatusCode) return null; //If response = 200, return true, else false

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Department>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        #endregion

        #region POST Methods

        public static async Task<bool> PostAsync(Department dept)
        {
            string json = JsonSerializer.Serialize(dept);
             StringContent content = new(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("Department", content);
            return response.IsSuccessStatusCode; //If response = 200, return true, else false
        }

        #endregion

        #region PUT Methods 
        
        //Logiquement ^^ que POST sauf avec {id} en ajout dnas le lien
        public static async Task<bool> PutAsync(int id, Department dept)
        {
            string json = JsonSerializer.Serialize(dept);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"Department/{id}", content);
            return response.IsSuccessStatusCode; //If response = 200, return true, else false
        }

        #endregion

        #region DELETE Methods
        
        public static async Task<bool> DeleteAsync(int id)
        {
            var response = await client.DeleteAsync($"Department/{id}");
            return response.IsSuccessStatusCode; //If response = 200, return true, else false
        }

        #endregion
    }
}
