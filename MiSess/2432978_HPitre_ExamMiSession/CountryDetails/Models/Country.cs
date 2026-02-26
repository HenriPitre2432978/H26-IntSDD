using CountryDetails.ViewModel;

namespace CountryDetails.Models
{
    public class Country : BaseViewModel
    {
        internal RESTName Name { get; set; } //get from objet RestName
        public RESTFlag Flag { get; set; }
        public string[] Capital { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
        public double Area { get; set; }
        public int Population { get; set; }

        public Country(RESTName? n, RESTFlag? f, RESTCountry? c)
        {
            Name = n;
            Flag = f;
            Capital = c.Capital;
            Region = c.Region;
            Subregion = c.Subregion;
            Area = c.Area;
            Population = c.Population;
        }
    }
}
