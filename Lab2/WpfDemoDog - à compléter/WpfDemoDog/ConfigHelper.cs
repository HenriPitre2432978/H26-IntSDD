using Microsoft.Extensions.Configuration;
using System.IO;

namespace WpfDemoDog
{
    class ConfigHelper
    {
        public static IConfiguration Config =
            new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }
}
