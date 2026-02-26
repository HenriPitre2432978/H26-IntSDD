using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryDetails.Models
{
    public class RESTCountry
    {
        [JsonProperty(PropertyName = "capital")]
        public string[] Capital { get; set; }

        [JsonProperty(PropertyName = "region")]
        public string Region { get; set; }

        [JsonProperty(PropertyName = "subregion")]
        public string Subregion { get; set; }

        [JsonProperty(PropertyName = "area")]
        public double Area { get; set; }

        [JsonProperty(PropertyName = "population")]
        public int Population { get; set; }
    }
}
