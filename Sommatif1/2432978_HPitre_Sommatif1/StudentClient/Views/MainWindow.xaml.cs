using Newtonsoft.Json;
using StudentClient.Configs;
using StudentClient.Views;
using System.IO;
using System.Net.Http;
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

namespace StudentClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            new Students().Show(); //GOTO Students window <---------------------   

        }
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            #region Properties

            string email = txtEmail.Text;
            string password = txtPassword.Password;

            var loginData = new {  email, password }; //Un string[] ne marche pas ??
            string json = JsonConvert.SerializeObject(loginData);


            #endregion

            //Instanciate Client and assign base properties (api/link)
            using HttpClient client = new();
            client.BaseAddress = new System.Uri(ConfigHelper.publicAPIBaseUrl);
            client.DefaultRequestHeaders.Add("x-api-key", ConfigHelper.ApiKey);

            //Get basic header properties of the incoming rquest
            StringContent content = new(json, Encoding.UTF8, "application/json");
            
            try
            {
                HttpResponseMessage response = await client.PostAsync("login", content);
                if (response.IsSuccessStatusCode) //is respondes ok ? (200) or bad ? (4XX Bad request)
                {
                    lblStatus.Text = "Redirection...";

                    //deserialize the response into a LoginResponse object with the Token property instanciated
                    LoginResponse? result = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync()); 
                    SaveToken(result.Token); //get token of response


                    new Students().Show(); //GOTO Students window <---------------------   
                    this.Close(); //close login page
                }
                else
                    lblStatus.Text = "Identifiants invalides.";
            }
            catch (HttpRequestException)
            {
                lblStatus.Text = "Erreur de connexion.";
            }
        }

        private static void SaveToken(string token)
        {
            // Save le token in appsettings.json for this instance (local)
            string json = File.ReadAllText("Configs/appsettings.json");

            //Get config as dynamic to modify the token property (Aide avec l'IA ici)
            dynamic config = JsonConvert.DeserializeObject(json);
            if (config != null ) config["Token"] = token; 
             
            //Write the token
            File.WriteAllText("appsettings.json", JsonConvert.SerializeObject(config, Formatting.Indented));
        }
    }

    //json class to deseralise response of login (get token)
    public class LoginResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}