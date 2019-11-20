#pragma warning disable IDE1006


using System;
using System.Collections.Generic;
using System.Linq;
using Mcg.Webservice.Cncf.Api.Infrastructure.Configuration;
using Prometheus;

namespace Mcg.Webservice.Cncf.Api.Infrastructure.Instrumentation
{
    /// <summary>
    /// Manages the prometheus counters used by the application.
    /// </summary>
    public class AppMetrics : IAppMetrics
    {
        private static readonly object padlock = new object();

        internal IAppSettings Settings { get; }

        /// <summary>
        /// Default ctor.
        /// </summary>
        /// <param name="settings">The application settings <see cref="IAppSettings"/> instance used to configure this instance.</param>
        public AppMetrics(IAppSettings settings)
        {
            Settings = settings;
        }

        internal IDictionary<string, Counter> Counters = new Dictionary<string, Counter>();
        internal IDictionary<string, Histogram> Histograms = new Dictionary<string, Histogram>();
        internal IDictionary<string, Gauge> Gauges = new Dictionary<string, Gauge>();
        internal List<string> MetricsPrefixes = new List<string>();
        private List<string> DefaultLabels => new List<string> { "host", "service", "outcome" };

        private void LogException(string typeName, Exception ex)
        {
            const string ERR_TEMPLATE = "{type} {method} {error_type} {error_message} {error_stack_trace}";
            Serilog.Log.Error(ERR_TEMPLATE, nameof(AppMetrics), typeName, ex.GetType().Name, ex.Message, ex.StackTrace);
        }

        /// <summary>
        /// Increments the given counter with the given label values.
        /// </summary>
        /// <param name="counterName"></param>
        /// <param name="success">indicates if the counter is a success or failure counter.</param>
        public void IncCounter(string counterName, bool success = true)
        {
            try
            {
                var labelValues = new[]
                {
                    Environment.MachineName.SafeString(),
                    Settings["JAEGER_SERVICE_NAME"].SafeString(),
                    success? "success" : "error"
                };

                var ln = Counters[counterName].LabelNames;

                Counters[counterName].WithLabels(labelValues).Inc();
            }
            catch (Exception ex)
            {
                LogException(nameof(IncCounter), ex);
            }
        }

        /// <summary>
        /// Increments the given counter with the given label values.
        /// </summary>
        /// <param name="histogramName">The name of the counter to increment.</param>
        /// <param name="success">indicates if the counter is a success or failure counter.</param>
        public void IncHistogram(string histogramName, double value, bool success = true)
        {
            try
            {
                var labelValues = new[]
                {
                    Environment.MachineName.SafeString(),
                    Settings["JAEGER_SERVICE_NAME"].SafeString(),
                    success? "success" : "error"
                };

                Histograms[histogramName].WithLabels(labelValues).Observe(value);
            }
            catch (Exception ex)
            {
                LogException(nameof(IncHistogram), ex);
            }
        }

        /// <summary>
        /// Increments the given counter with the given label values.
        /// </summary>
        public void IncGauge(string gaugeName)
        {
            try
            {
                Gauges[gaugeName].Inc();
            }
            catch (Exception ex)
            {
                LogException(nameof(IncCounter), ex);
            }
        }

        /// <summary>
        /// Increments the given counter with the given label values.
        /// </summary>
        public void DecGauge(string gaugeName)
        {
            try
            {
                Gauges[gaugeName].Dec();
            }
            catch (Exception ex)
            {
                LogException(nameof(DecGauge), ex);
            }
        }

        /// <summary>
        /// Ensures that counters are added as needed and only one time during the lifetime of the application.
        /// </summary>
        /// <param name="owningType">The name of the type that ownes the target method.</param>
        /// <param name="targetName">The name of the method to be monitored.</param>
        public void AddMetrics(string owningType, string targetName)
        {
            string prefix = $"{owningType}_{targetName}".SafeString();

            if (MetricsPrefixes.Any(p => p == prefix))
            {
                return;
            }

            lock (padlock)
            {
                if (MetricsPrefixes.Any(p => p == prefix))
                {
                    return;
                }

                MetricsPrefixes.Add(prefix);

                try
                {
                    Counters.Add($"{prefix}_count",
                        Prometheus.Metrics.CreateCounter(
                            $"{prefix}_count",
                            null,
                            new CounterConfiguration
                            {
                                LabelNames = DefaultLabels.ToArray()
                            }));

                    Histograms.Add($"{prefix}_elapsed_microseconds",
                        Prometheus.Metrics.CreateHistogram(
                            $"{prefix}_elapsed_microseconds",
                            null,
                            new HistogramConfiguration
                            {
                                Buckets = new double[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 4096 * 2, 4096 * 4, 4096 * 8 },
                                LabelNames = DefaultLabels.ToArray()
                            }));

                    Gauges.Add($"{prefix}_gauge",
                        Prometheus.Metrics.CreateGauge(
                            $"{prefix}_gauge",
                            null,
                            new GaugeConfiguration
                            {
                                LabelNames = new[] { "host", "service" }
                            }));
                }
                catch (Exception ex)
                {
                    LogException(nameof(AddMetrics), ex);
                }
            }
        }
    }
}
#pragma warning restore IDE1006