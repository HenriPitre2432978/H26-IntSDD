using Microsoft.Extensions.Configuration;
using System.IO;

namespace StudentClient.Configs
{
    public static class ConfigHelper
    {
        public static IConfiguration Config =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // SetBasePath nécessite le package Microsoft.Extensions.Configuration.Json
                .AddJsonFile("Configs/appsettings.json", optional: true, reloadOnChange: true)
                .Build();

        //Liasion des paramètres de config dans appsettings.json en paramètres "appelables" dans ce projet
        public static string publicAPIBaseUrl => Config["publicAPIBaseUrl"];
        public static string localAPIBaseUrl => Config["localAPIBaseUrl"];
        public static string ApiKey => Config["ApiKey"];
        public static string Token => Config["Token"];
    }
}