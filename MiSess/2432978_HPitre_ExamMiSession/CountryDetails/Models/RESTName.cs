using CountryDetails.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryDetails.Models
{
    public class RESTName : BaseViewModel
    {
        class Name
        {
            [JsonProperty(PropertyName = "common")]
            public static string common { get; set; }
            [JsonProperty(PropertyName = "official")]
            public static string official { get; set; }
        }
        public string GetCommonName() => Name.common;
        public string GetOfficialName() => Name.official;
    }

   
}
