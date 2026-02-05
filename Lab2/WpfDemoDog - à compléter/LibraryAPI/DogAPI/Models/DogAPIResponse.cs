using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.DogAPI.Models
{
    public class DogAPIResponse
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        //TODO : Compléter... Ajouter d'autre modèle au besoin...
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}
