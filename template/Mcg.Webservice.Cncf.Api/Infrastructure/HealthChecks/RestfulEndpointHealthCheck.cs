using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Mcg.Webservice.Cncf.Api.Infrastructure.Instrumentation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Mcg.Webservice.Cncf.Api.Infrastructure.HealthChecks
{
    /// <summary>
    /// Creates a health check for a restful endpoint that this one depends upon.
    /// </summary>
    /// <remarks>
    /// Aligns with core principle https://mcghealth.atlassian.net/wiki/spaces/ARC/pages/361431168/03.04+Health+Reporting+and+Alerts
    /// </remarks>
    public class RestfulEndpointHealthCheck : IHealthCheck
    {
        internal static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var heartbeat = new Heartbeat()
            {
                URL = httpContext.Request.Path,
                Status = result.Status.ToString(),
                RequestDurationMS = result.TotalDuration.TotalMilliseconds
            };

            var json = JsonConvert.SerializeObject(heartbeat, Formatting.Indented);

            return httpContext.Response.WriteAsync(json);
        }

        internal Uri DependencyUri { get; private set; }

        internal HttpClient DependencyClient { get; private set; }

        /// <summary>
        /// Creates a new instance of type <see cref="RestfulEndpointHealthCheck"/>.
        /// </summary>
        /// <param name="dependencyEndpoint">The base Uri of the service that is a dependency.</param>
        public RestfulEndpointHealthCheck(Uri dependencyEndpoint)
        {
            DependencyUri = dependencyEndpoint
                ?? throw new ArgumentNullException(nameof(dependencyEndpoint));

            DependencyClient = new HttpClient { BaseAddress = dependencyEndpoint };
        }

        /// <summary>
        /// Creates a new instance of type <see cref="RestfulEndpointHealthCheck"/>.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> to be used to check the dependency.</param>
        internal RestfulEndpointHealthCheck(HttpClient client)
        {
            DependencyClient = client;
            DependencyUri = client.BaseAddress;
        }

        [Instrument]
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            HealthCheckResult result;

            try
            {
                var response = await DependencyClient.GetAsync("ops/health");

                if (!response.IsSuccessStatusCode)
                {
                    return new HealthCheckResult(HealthStatus.Degraded, description: $"dependency {DependencyUri} return a status code of {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var hb = JsonConvert.DeserializeObject<Heartbeat>(json);

                result = hb.Status switch
                {
                    "Healthy" => HealthCheckResult.Healthy(),
                    "Degraded" => HealthCheckResult.Degraded(),
                    _ => HealthCheckResult.Unhealthy(),
                };
            }
            catch (Exception ex)
            {
                result = new HealthCheckResult(HealthStatus.Unhealthy, exception: ex);
            }

            return result;
        }
    }
}
