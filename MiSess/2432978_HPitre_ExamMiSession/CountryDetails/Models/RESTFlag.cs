using CountryDetails.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryDetails.Models
{
    public class RESTFlag : BaseViewModel
    {
        [JsonProperty(PropertyName = "png")]
        public string png { get; set; }
        [JsonProperty(PropertyName = "svg")]
        public string svg { get; set; }

    }
}
