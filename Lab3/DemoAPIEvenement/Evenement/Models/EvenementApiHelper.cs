using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace DemoEvent.Evenement.Models
{
    public class EvenementApiHelper
    {
        // Récupère l'URL de base depuis le fichier de configuration "appsettings.json" via le ConfigHelper
        private static readonly string baseUrl = ConfigHelper.Config["baseURL"] ?? "";

        /// <summary>
        /// Récupère la liste complète de tous les événements de l'API.
        /// Rôle : Effectue une requête HTTP GET.
        /// </summary>
        /// <returns>Une liste d'objets Evenement ou null en cas d'erreur.</returns>
        public static async Task<List<Evenement>>? GetAllAsync()
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(baseUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                // Désérialisation : Transforme le texte JSON reçu en objets C#
                return JsonConvert.DeserializeObject<List<Evenement>>(json);
            }
            return null;
        }

        /// <summary>
        /// Récupère un événement spécifique à partir de son identifiant unique.
        /// Rôle : Effectue une requête HTTP GET pointant vers une ressource précise.
        /// </summary>
        /// <param name="id">L'identifiant de l'événement.</param>
        /// <returns>L'objet Evenement trouvé ou null.</returns>
        public static async Task<Evenement?> GetByIdAsync(string id)
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{baseUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Evenement>(content);
            }

            return null;
        }

        /// <summary>
        /// Crée un nouvel événement dans l'API.
        /// Rôle : Envoie des données via une requête HTTP POST.
        /// </summary>
        /// <param name="evt">L'objet Evenement à créer.</param>
        /// <returns>True si la création a réussi, sinon False.</returns>
        public static async Task<bool> PostEvenementAsync(Evenement evt)
        {
            using HttpClient client = new HttpClient();

            // Configuration pour transformer les noms de propriétés C# (PascalCase) 
            // en format JSON standard (camelCase)
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string json = JsonConvert.SerializeObject(evt, settings);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(baseUrl, content);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Met à jour un événement existant.
        /// Rôle : Envoie une requête HTTP PUT pour remplacer les données d'une ressource.
        /// </summary>
        /// <param name="id">L'identifiant de l'événement à modifier.</param>
        /// <param name="evt">Les nouvelles données de l'événement.</param>
        /// <returns>True si la mise à jour a réussi.</returns>
        public static async Task<bool> PutEvenementAsync(string id, Evenement evt)
        {
            using HttpClient client = new HttpClient();

            // Configuration pour transformer les noms de propriétés C# (PascalCase) 
            // en format JSON standard (camelCase)
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string json = JsonConvert.SerializeObject(evt, settings);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // On cible l'URL spécifique avec l'ID pour la modification
            HttpResponseMessage response = await client.PutAsync($"{baseUrl}/{id}", content);
            return response.IsSuccessStatusCode;
        }


        //PatchEventAsync
        public static async Task<bool> PatchEvenementAsync(string id, Evenement evt)
        {
            using HttpClient client = new HttpClient();

            // Configuration pour transformer les noms de propriétés C# (PascalCase) 
            // en format JSON standard (camelCase)
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string json = JsonConvert.SerializeObject(evt, settings);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // Envoyer avec PATCH
            HttpResponseMessage response = await client.PatchAsync($"{baseUrl}/{id}", content);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Supprime un événement de l'API.
        /// Rôle : Envoie une requête HTTP DELETE.
        /// </summary>
        /// <param name="id">L'identifiant de l'événement à supprimer.</param>
        /// <returns>True si la suppression a réussi.</returns>
        public static async Task<bool> DeleteEvenementAsync(string id)
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.DeleteAsync($"{baseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}