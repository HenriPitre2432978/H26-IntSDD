using LibraryAPI.DogAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI
{
    public class DogAPIHelper
    {
        private readonly string apikey;

        //TODO: Créer les classes modèle nécessaire pour représenter correctement les données dont vous avez besoin.
        public async Task<List<DogAPIResponse>> LoadDogsWithImagesAsync(string apikey, int breedId)
        {
            try
            {
                string uri = $"https://api.thedogapi.com/v1/breeds/{breedId}";
                if (breedId <= 0) uri = $"https://api.thedogapi.com/v1/breeds/";
                //TODO:Modifier l’application pour gérer l'url de base dans un fichier appsettings.json

                using HttpClient client = new HttpClient();

                // Clé dans le header x-api-key
                // La clé de cette API n'est pas fourni comme "params" mais dans l'entête de la requête
                client.DefaultRequestHeaders.Add("x-api-key", apikey);

                HttpResponseMessage response = await client.GetAsync(uri);

                // Erreur HTTP (401, 404, etc.)
                if (!response.IsSuccessStatusCode)
                {
                    // On lève une exception plutôt que retourner null
                    throw new Exception(
                        $"Erreur HTTP : {(int)response.StatusCode} - {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();

                // Déclare le résultat comme List (toujours)
                List<DogAPIResponse> result;

                // si breedId EST fourni: pour un seul chien, sinon pour la liste des chiens
                if (breedId > 0)
                {
                    // Un seul chien : objet → List avec 1 élément
                    var singleDog = JsonConvert.DeserializeObject<DogAPIResponse>(json);
                    result = new List<DogAPIResponse> { singleDog };
                }
                else
                {
                    // Liste de chiens : tableau JSON
                    result = JsonConvert.DeserializeObject<List<DogAPIResponse>>(json);
                }

                if (result == null)
                {
                    throw new Exception("Désérialisation JSON retournée null.");
                }

                return result;
            }
            catch (HttpRequestException ex)
            {
                // On remonte l'erreur au caller
                throw new Exception("Erreur réseau : " + ex.Message, ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Erreur lors du traitement du JSON : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur inattendue : " + ex.Message, ex);
            }
        }

    }
}
