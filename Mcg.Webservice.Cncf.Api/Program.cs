using System;
using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Mcg.Webservice.Cncf.UnitTests")]
namespace Mcg.Webservice.Cncf.Api
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                .UseSerilog((ctx, config) =>
                {
                    var elasticUri = Environment.GetEnvironmentVariable("APP_ELASTIC_SEARCH_URI");

                    if (string.IsNullOrWhiteSpace(elasticUri))
                    {
                        throw new ConfigurationErrorsException("Missing configuruation value: APP_ELASTIC_SEARCH_URI");
                    }

                    if (!Uri.IsWellFormedUriString(elasticUri, UriKind.RelativeOrAbsolute))
                    {
                        throw new ConfigurationErrorsException("Invalid configuruation value for setting: APP_ELASTIC_SEARCH_URI");
                    }

                    config
                        .MinimumLevel.Information()
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                        {
                            AutoRegisterTemplate = true,
                           
                        });

                })
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.ConfigureKestrel(serverOptions =>
                     {
                         serverOptions.AddServerHeader = true;
                     })
                     .UseStartup<Startup>();
                 })
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.AddEnvironmentVariables("APP_");
                 });
    }
}
