using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing.Util;

namespace Mcg.Webservice.Cncf.Api.Infrastructure.Tracing
{
    /// <summary>
    /// Implements <see cref="IServiceCollection"/> extensions for ease of configuring the tracing in the Startup.cs file.
    /// </summary>
    public static class TracingServicesExtension
    {
        public static IServiceCollection AddDistributedTracing(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenTracing(builder =>
            {
                builder.ConfigureAspNetCore(options =>
                {
                    options.Hosting.IgnorePatterns.Add(x =>
                    {
                        return (
                            x.Request.Path == "/swagger" ||
                            x.Request.Path == "/swagger/index.html" ||
                            x.Request.Path == "/swagger/favicon-16x16.png" ||
                            x.Request.Path == "/swagger/v1/swagger.json" ||
                            x.Request.Path == "/ops/metrics" ||
                            x.Request.Path == "/ops/health"
                        );
                    });
                });
            });

            ILoggerFactory loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();

            return services.AddSingleton(serviceProvider =>
            {
                var config = Jaeger.Configuration.FromIConfiguration( loggerFactory, configuration);
                var tracer = config.GetTracer();

                if (!GlobalTracer.IsRegistered())
                {
                    GlobalTracer.Register(tracer);
                }

                return tracer;
            });
        }
    }
}
