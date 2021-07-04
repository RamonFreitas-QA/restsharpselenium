using Itau.Processo.Core.Data.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;

namespace Itau.Processo.Core.Data
{
    public class ContextSettings
    {
        public static IConfiguration configuration;

        public ContextSettings()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
        public static string GetSection(string pathJson)
        {
            return configuration.GetSection(pathJson).Value;
        }

        public static AppSettings ReturnJson()
        {
            return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(Directory
                                .GetCurrentDirectory() + @"\AppSettings.json"));
            //CultureInfo("pt-BR")
        }
    }
}
