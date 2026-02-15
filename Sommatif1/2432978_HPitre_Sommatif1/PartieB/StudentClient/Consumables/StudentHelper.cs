using StudentClient.Configs;
using StudentClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudentClient.Consumables
{
    /*
     NOTE M. EMMANUEL:

    Ceci est le gestionnaire des requêtes envoyées pour GET, POST, PUT et DELETE les students.
    Je sais que ceci est supposé fonctionner correctement puisque je l'ai essayé hier soir en travaillant sur ma Partie A avec les éléments Department au lieu de Student.
    À ce moment même, il semblerait que mon GetAsync reste bloqué dans une boucle infinie qui ne s'arrête jamais, rendant impossible l'obtention des Student. 
    Ce problème n'était et n'est toujours pas présent lorsque je GetAsync Department, et pourtant je le fais exactement de la même façon. 


 Au cours de ce sommatif, je n'ai pas eu le temps de déceler le problème exact et de le régler, alors rien ne s'affiche dans le ListView et au démarrage de mon projet. 
 Pour tenter de gagner des points, j'ai tout de même continué de coder la logique en faisant comme si tout fonctionnait, 
 mais je n'ai pas eu le temps de finir car j'ai passé une bonne partie du temps à essayer de régler le problème source.

 Alternativement, si vous estimez que vous ne pouvez pas évaluer grand chose de ce sommatif, 
 vous pouvez regarder mon Sommatif1 Partie A où je fais la consommation d'API sans problème pour la section département.
 Je l'avais fait à l'avance dans le but de mieux comprendre et me préparer pour aujourd'hui, mais il semblerait que le GetAsync m'en a empêché. Bonne journée.

 
     */


    public static class StudentHelper
    {
        private static readonly HttpClient client = new()
        {
            BaseAddress = new Uri(ConfigHelper.localAPIBaseUrl)
        };

        #region GET all/id Methods
        public static async Task<List<Student>?> GetAllAsync()
        {
            try
            {
                // ----------------- ICI --------------------- 
                HttpResponseMessage? response = await client.GetAsync("Student"); //CECI lance une boucle infinie qui empêche le programme de fonctionner. !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if (!response.IsSuccessStatusCode) return null;//If response = 200, return true, else false

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Student>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Student?> GetByIdAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"Student/{id}"); //Pour spécifier quel dept, ajouter id dans l'url de requête
            if (!response.IsSuccessStatusCode) return null; //If response = 200, return true, else false

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Student>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        #endregion

        #region POST Methods

        public static async Task<bool> PostAsync(Student std)
        {
            string json = JsonSerializer.Serialize(std);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("Student", content);
            return response.IsSuccessStatusCode; //If response = 200, return true, else false
        }

        #endregion

        #region PUT Methods 

        //Logiquement ^^ que POST sauf avec {id} en ajout dnas le lien
        public static async Task<bool> PutAsync(int id, Student std)
        {
            string json = JsonSerializer.Serialize(std);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"Student/{id}", content);
            return response.IsSuccessStatusCode; //If response = 200, return true, else false
        }

        #endregion

        #region DELETE Methods

        public static async Task<bool> DeleteAsync(int id)
        {
            var response = await client.DeleteAsync($"Student/{id}");
            return response.IsSuccessStatusCode; //If response = 200, return true, else false
        }

        #endregion
    }
}

