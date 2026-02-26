using Microsoft.Extensions.Configuration;
using System.IO;
class ConfigHelper
{
    public static IConfiguration Config =
    new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Configs/appsettings.json", optional: true, reloadOnChange: true)
    .Build();

    public static string APIBaseUrl => Config["APIBaseUrl"];
}