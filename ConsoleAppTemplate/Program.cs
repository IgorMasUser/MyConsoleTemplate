using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ConsoleAppTemplate
{
    partial class Program
    {

        static void Main(string[] arrs)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                var host = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) =>
                    {
                        services.AddTransient<IGreatingService, GreatingService>();
                    })
                    .UseSerilog()
                    .Build();

                logger.Information("Application started");

                var svc = ActivatorUtilities.CreateInstance<GreatingService>(host.Services, logger);
                svc.ToGreat();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Fatal error occured while running application.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", false, true);

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (!string.IsNullOrEmpty(environment))
            {
                var envSpecificJsonFile = $"appsettings.{environment}.json";
                builder.AddJsonFile(envSpecificJsonFile, optional: true, reloadOnChange: true);
            }
        }

    }
}
