using Microsoft.Extensions.Configuration;

namespace Paradaim.BaseGateway.Infrastructures
{
    public class AppConfig
    {
        public static string Get(string key)
        {            
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var basePath = Environment.GetEnvironmentVariable("CONFIG_BASE_PATH") ?? Directory.GetCurrentDirectory();
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile($"appsettings.{environment}.json").Build();
            return config[key];
        }
    }
}
