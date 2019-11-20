using Mcg.Webservice.Cncf.Api.DataAccess;
using Mcg.Webservice.Cncf.Api.Infrastructure;
using Mcg.Webservice.Cncf.Api.Infrastructure.Configuration;
using Mcg.Webservice.Cncf.Api.Infrastructure.HealthChecks;
using Mcg.Webservice.Cncf.Api.Infrastructure.Instrumentation;
using Mcg.Webservice.Cncf.Api.Infrastructure.Logging;
using Mcg.Webservice.Cncf.Api.Infrastructure.Tracing;
using Mcg.Webservice.Cncf.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace Mcg.Webservice.Cncf.Api
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            

            services.AddSingleton<IAppSettings, AppSettings>();
            services.AddSingleton<IAppMetrics, AppMetrics>();
            services.AddSingleton<IExampleDataRepository, ExampleDataRepository>();
            services.AddTransient<IExampleBusinessService, ExampleBusinessService>();

            services.AddServiceHealthChecks();
            services.AddDistributedTracing(Configuration);
            services.AddSwaggerDocumentation();
        }

        public void Configure(IApplicationBuilder app)
        {
            AspectFactory.Metrics = app.ApplicationServices.GetService<IAppMetrics>();
            AspectFactory.Settings = app.ApplicationServices.GetService<IAppSettings>();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerDocumentation();
            app.UseServiceHealthChecks();

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                appBuilder.UseRequestLogging();
            });

            //--> see https://github.com/prometheus-net/prometheus-net/blob/master/README.md for more details
            app.UseMetricServer(url: "/ops/metrics");
            app.UseHttpMetrics();

            app.UseCors(options =>
            {
                /************************************************************************
                 * WARNING!!! The development configuration is set to '*' and should NOT
                 *            be allowed to go into higher environments with the setting!
                 ************************************************************************/
                options.WithOrigins(Configuration["CORS_ALLOWED_URLS"].Split(','));
                options.SetIsOriginAllowedToAllowWildcardSubdomains();
                options.AllowAnyMethod();
            });
        }
    }
}
