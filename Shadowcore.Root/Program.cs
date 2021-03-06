using System.Diagnostics.CodeAnalysis;
using Autofac.Extensions.DependencyInjection;
using Shadowcore.Root.Configuration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Shadowcore.Root
{
    [SuppressMessage("", "CS1591:MissingXmlDocumentation")]
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            DatabaseInitializer.SeedDatabases(host.Services.CreateScope());

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddAutofac())
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddSerilog();   
                })
                .UseStartup<Startup>()
                .Build();
    }
}
