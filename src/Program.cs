using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LetsGoSEA.WebSite
{
    /// <summary>
    /// Program generates a .NET Generic Host. Host encapsulates resources: 
    /// HTTP server, middleware, logging, DI, and configuration. 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point of this program
        /// </summary>
        /// <param name="args">command-line arguments</param>
        public static void Main(string[] args)
        {
            // Configures host with set of default options
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Initialize the website host webBuilder
        /// </summary>
        /// <param name="args">command-line arguments passed from Main</param>
        /// <returns>An IHostBuilder object</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Specifies the Startup class when host is built
                    webBuilder.UseStartup<Startup>();
                });
    }
}
