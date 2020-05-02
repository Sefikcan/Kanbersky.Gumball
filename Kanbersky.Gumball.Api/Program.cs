using Kanbersky.Gumball.Core.Helpers.Logging;
using Kanbersky.Gumball.Core.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Kanbersky.Gumball.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logServerSettings = BaseHelpers.GetConfigurationRoot(args).GetSection("ElasticSearchSettings").Get<ElasticSearchSettings>();
            Log.Logger = new LoggerHelpers(logServerSettings).Register(typeof(Startup).Assembly.GetName().Name, LogEventLevel.Warning);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
