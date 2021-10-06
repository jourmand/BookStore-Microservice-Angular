using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BookStore.Endpoints.HttpGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((host, config) =>
            {
                config
                .SetBasePath(host.HostingEnvironment.ContentRootPath)
                .AddJsonFile("configuration.json", true, true)
                .AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", true, true)
                .AddJsonFile(Path.Combine("configuration", "ocelot.json"))
                .AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
