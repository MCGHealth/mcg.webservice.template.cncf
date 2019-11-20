using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Mcg.Webservice.Cncf.Api.Infrastructure.HealthChecks
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class HealthChecksExtensions
    {
        /// <summary>
        /// Adds the health check endpoint required by SysOps.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceHealthChecks(this IServiceCollection services)
        {
            //--> see https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2 for more info.
            return services.AddHealthChecks().Services;
        }

        /// <summary>
        /// Adds a call to a specified RESTful endpoint, perferrably a healthcheck endpoint, of a referenced service to determine it's health.
        /// </summary>
        /// <remarks>
        /// Aligns with core principle https://mcghealth.atlassian.net/wiki/spaces/ARC/pages/361431168/03.04+Health+Reporting+and+Alerts
        /// </remarks>
        public static IHealthChecksBuilder AddDependencyHealthCheck(
            this Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder builder,
            string dependencyUrl,
            string name,
            HealthStatus? failureStatus = HealthStatus.Healthy,
            IEnumerable<string> tags = default)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!Uri.IsWellFormedUriString(dependencyUrl, UriKind.RelativeOrAbsolute))
            {
                throw new UriFormatException($"invalid URI: {dependencyUrl}");
            }

            return builder.Add(new HealthCheckRegistration(
                name,
                sp => new RestfulEndpointHealthCheck(new Uri(dependencyUrl)),
                failureStatus,
                tags));
        }

        /// <summary>
        /// Adds the Microsoft healthcheck service middleware.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseServiceHealthChecks(this IApplicationBuilder app)
        {
            return app.UseHealthChecks("/ops/health", new HealthCheckOptions()
            {
                ResponseWriter = RestfulEndpointHealthCheck.WriteResponse,
                AllowCachingResponses = false,
            });
        }

    }
}
