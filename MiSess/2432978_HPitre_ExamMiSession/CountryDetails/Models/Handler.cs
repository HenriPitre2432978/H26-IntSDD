using CountryDetails.Consumables;
using CountryDetails.ViewModel;
using System.Windows;

namespace CountryDetails.Models
{
    public class Handler : BaseViewModel
    {
        #region Propriétés

        private Country? _countrySelectionne;
        public Country? CountrySelectionne
        {
            get => _countrySelectionne;
            set
            {
                _countrySelectionne = value;
                OnPropertyChanged(nameof(CountrySelectionne));
            }
        }

        internal async Task LoadCountryAsync(string name)
        {
            RESTCountry? countryDetails = await CountryHelper.GetAsync(name);
            RESTFlag? flag = await CountryFlagHelper.GetAsync(name);
            RESTName? countryName = await CountryNameHelper.GetAsync(name);

            Country c = new(countryName, flag, countryDetails);

            if (countryDetails == null || flag == null|| countryName == null)
            {
                MessageBox.Show("!ERREUR! Get impossible.");
                return;
            }

            CountrySelectionne = c;
        }
        #endregion

    }
}
