using System;
using Mcg.Webservice.Cncf.Api.Infrastructure.Configuration;
using Mcg.Webservice.Cncf.Api.Infrastructure.Instrumentation;
using Mcg.Webservice.Cncf.Api.Infrastructure.Logging;
using Mcg.Webservice.Cncf.Api.Infrastructure.Tracing;

namespace Mcg.Webservice.Cncf.Api.Infrastructure
{
	public class AspectFactory
	{
		internal static double TickPerMicrosecond => (double)(TimeSpan.TicksPerMillisecond / 1000);

		internal static Type VoidTaskResult => Type.GetType("System.Threading.Tasks.VoidTaskResult");

		internal static IAppMetrics Metrics;

		internal static IAppSettings Settings;

		public static object GetInstance(Type aspectType)
		{
			if (aspectType == typeof(MetricsAspect))
			{
				return new MetricsAspect(Metrics);
			}

			if (aspectType == typeof(LogAspect))
			{
				return new LogAspect();
			}

			if (aspectType == typeof(TraceAspect))
			{
				return new TraceAspect();
			}

			throw new ApplicationException($"Unknown aspect type: {aspectType}.");
		}
	}
}
