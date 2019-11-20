namespace Mcg.Webservice.Cncf.Api.Infrastructure.Instrumentation
{
    /// <summary>
    /// When implemented by a class, manages the prometheus counters used by the application.
    /// </summary>
	public interface IAppMetrics
    {
        void AddMetrics(string owningType, string targetName);

        void IncCounter(string counterName, bool success = true);

        void IncHistogram(string histogramName, double value, bool success = true);

        void IncGauge(string gaugeName);

        void DecGauge(string gaugeName);
    }
}