using System;
using Newtonsoft.Json;

namespace Mcg.Webservice.Cncf.Api.Infrastructure.HealthChecks
{
    /// <summary>
    /// Implements the response description defined in the 03.04: Health Reporting and Alerts: ResponseDescription
    /// https://mcghealth.atlassian.net/wiki/spaces/ARC/pages/361431168/03.04+Health+Reporting+and+Alerts#id-03.04:HealthReportingandAlerts-HealthResponseDescription
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [JsonObject(Description = "Implements the response description defined in the 01.05.05: Health Reporting and Alerts")]
    public class Heartbeat
    {
        /// <summary>
        /// The current status of the service at the time reported.
        /// </summary>
        [JsonProperty("status"), JsonRequired]
        public string Status { get; set; }

        /// <summary>
        /// The URL of this call
        /// </summary>
        [JsonProperty("url"), JsonRequired]
        public string URL { get; set; }

        /// <summary>
        /// The machine (or container name) for this endpoint.
        /// </summary>
        [JsonProperty("machine"), JsonRequired]
        public string Machine { get; set; } = Environment.MachineName;

        /// <summary>
        /// The date and time the response was created.
        /// </summary>
        [JsonProperty("utcdatetime"), JsonRequired]
        public DateTime UTCDatetime { get; set; } = DateTime.Now;

        /// <summary>
        /// The duration in MS it took to process the call.
        /// </summary>
        [JsonProperty("request_duration_microseconds"), JsonRequired]
        public Double RequestDurationMS { get; set; }

        /// <summary>
        /// Anything helpful to respond to the current status.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
