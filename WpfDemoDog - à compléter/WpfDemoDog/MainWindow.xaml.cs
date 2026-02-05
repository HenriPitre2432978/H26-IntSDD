using LibraryAPI;
using LibraryAPI.DogAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDemoDog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Création du helper DogAPI
        DogAPIHelper dogApiHelper = new DogAPIHelper();


        //TODO:Modifier l’application pour gérer la clé d'API dans un fichier appsettings.json
        private readonly string API_KEY = ConfigHelper.Config["DogApi:ApiKey"]; //confighelper va chercher la clé depuis appsettingss

        public MainWindow()
        {
            InitializeComponent();
        }

        // Charge une liste de chiens et retourne le résultat
        private async void LoadDogListAsync()
        {
            // Vide la liste
            listBoxChiens.Items.Clear();

            try
            {
                // Appel asynchrone à l’API Dog
                List<DogAPIResponse> chiens = await dogApiHelper.LoadDogsWithImagesAsync(API_KEY, 0);

                // Foreach pour ajouter chaque chien
                foreach (DogAPIResponse chien in chiens)
                {
                    listBoxChiens.Items.Add($"{chien.Id} - {chien.Name} ({chien.Origin})");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex.Message);
                MessageBox.Show($"Erreur lors de l'appel à l'API Dog : {ex.Message}",
                                "Erreur",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        // Charge les informations pour un chien choisi
        private async void DisplayDogInfos(int breedID)
        {
            if (breedID <= 0)
            {
                AfficherErreur("ID de la race obligatoire");
                throw new Exception($"ID de la race obligatoire");
            }

            //TODO : Implémenter une méthode qui affiche les informations d’un chien lorsqu’une race est sélectionnée


            //portion de Code pour afficher une photo
            var uriSource = new Uri(«Url_de_la_photo », UriKind.Absolute);
            dogImageCurrent.Source = new BitmapImage(uriSource);


        }


        private void AfficherErreur(string messageErreur)
        {
            lblErreur.Content = "";
            lblErreur.Content = messageErreur;

        }

        private void ButtonAPI_Click(object sender, RoutedEventArgs e)
        {
            LoadDogListAsync();
        }

        private void listBoxChiens_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO : Appeler la méthode DisplayDogInfos dans l’événement approprié du ListBox
        }

        

    }
}
