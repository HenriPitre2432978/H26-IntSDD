using CountryDetails.Models;
using System;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CountryDetails
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Handler GestionnairePays;
        public MainWindow()
        {
            InitializeComponent();
            GestionnairePays = new();
        }

        private async void BtnCharger_Click(object sender, RoutedEventArgs e)
        {
            await GestionnairePays.LoadCountryAsync(TxtNomPays.Text); //lancer la méthode en async mais discard la task

            TxtNomCommun.Text = GestionnairePays.CountrySelectionne.Name.GetCommonName();
            TxtNomOfficiel.Text = GestionnairePays.CountrySelectionne.Name.GetOfficialName();

            TxtPopulation.Text = GestionnairePays.CountrySelectionne.Population.ToString();
            TxtCapitales.Text = GestionnairePays.CountrySelectionne.Capital.ToString();


            TxtRegion.Text = GestionnairePays.CountrySelectionne.Region;
            TxtSousRegion.Text = GestionnairePays.CountrySelectionne.Subregion.ToString();
            TxtSuperficie.Text = GestionnairePays.CountrySelectionne.Area.ToString();

            //Uri? uri = new Uri(GestionnairePays.CountrySelectionne.Flag.png, UriKind.Absolute);
            //ImgDrapeau.Source = new BitmapImage(uri);
        }
    }
}