using Microsoft.Extensions.Configuration;

namespace DemoEvent
{
    class ConfigHelper
    {
        public static IConfiguration Config =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // SetBasePath nécessite le package Microsoft.Extensions.Configuration.Json
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
    }
}