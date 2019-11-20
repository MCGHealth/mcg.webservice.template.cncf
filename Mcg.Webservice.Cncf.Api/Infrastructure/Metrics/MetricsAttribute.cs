using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using AspectInjector.Broker;

namespace Mcg.Webservice.Cncf.Api.Infrastructure.Instrumentation
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	[Injection(typeof(MetricsAspect))]
	public sealed class InstrumentAttribute : Attribute { }

	/// <summary>
	/// Adds instrumentation to the decorated method.
	/// </summary>
	/// <remarks>
	/// Fulfills requirements defined in https://mcghealth.atlassian.net/wiki/spaces/ARC/pages/361006696/03.01+Logging
	/// and in https://mcghealth.atlassian.net/wiki/spaces/ARC/pages/365723941/03.02+Performance+Metrics
	/// attributes weren't specifically ordered, the two were combined into one.
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	[Aspect(Scope.Global, Factory = typeof(AspectFactory))]
	public class MetricsAspect
	{
		internal IAppMetrics Metrics { get; }
		public MetricsAspect(IAppMetrics metrics)
		{
			this.Metrics = metrics;
		}

		private static readonly MethodInfo AsyncHandler = typeof(MetricsAspect)
			.GetMethod(nameof(MetricsAspect.WrapAsync), BindingFlags.NonPublic | BindingFlags.Static);

		private static readonly MethodInfo SyncHandler = typeof(MetricsAspect)
			.GetMethod(nameof(MetricsAspect.Wrap), BindingFlags.NonPublic | BindingFlags.Static);

		private static T Wrap<T>(IAppMetrics metrics, string owningTypeName, string targetName, Func<object[], object> target, object[] args)
		{
			var eventName = $"{owningTypeName}_{targetName}".SafeString();
			var sw = Stopwatch.StartNew();
			var success = true;

			metrics.IncGauge($"{eventName}_gauge");

			try
			{
				return (T)target(args);
			}
			catch
			{
				success = false;
				throw;
			}
			finally
			{
				Write(metrics, owningTypeName, targetName, sw.ElapsedTicks, success);
			}
		}

		private static async Task<T> WrapAsync<T>(IAppMetrics metrics, string owningTypeName, string targetName, Func<object[], object> target, object[] args)
		{
			var eventName = $"{owningTypeName}_{targetName}".SafeString();
			var sw = Stopwatch.StartNew();
			var success = true;

			metrics.IncGauge($"{eventName}_gauge");
			try
			{
				var result = await (Task<T>)target(args);
				return result;
			}
			catch
			{
				success = false;
				throw;
			}
			finally
			{
				Write(metrics, owningTypeName, targetName, sw.ElapsedTicks, success);
			}
		}

		private static void Write(IAppMetrics metrics, string owningTypeName, string methodName, long duration, bool success = true)
		{
			var eventName = $"{owningTypeName}_{methodName}".SafeString();
			double elapsed = (double)duration / AspectFactory.TickPerMicrosecond;

			metrics.DecGauge($"{eventName}_gauge");
			metrics.IncCounter($"{eventName}_count", success);
			metrics.IncHistogram($"{eventName}_elapsed_microseconds", elapsed, success);
		}

		[Advice(Kind.Around, Targets = Target.Method)]
		public object Handle(
			[Argument(Source.Type)] Type owningType,
			[Argument(Source.Name)] string targetName,
			[Argument(Source.Target)] Func<object[], object> targetFunc,
			[Argument(Source.Arguments)] object[] inputArgs,
			[Argument(Source.ReturnType)] Type returnType)
		{
			string typeName = owningType.Name.ToLower();

			Metrics.AddMetrics(typeName, targetName);

			object[] invocationArgs =
				{
					Metrics,
					typeName,
					targetName.ToLower(),
					targetFunc,
					inputArgs,
				};

			if (typeof(Task).IsAssignableFrom(returnType))
			{
				var syncResultType = returnType.IsConstructedGenericType
					? returnType.GenericTypeArguments[0]
					: AspectFactory.VoidTaskResult;

				return AsyncHandler.MakeGenericMethod(syncResultType).Invoke(this, invocationArgs);
			}

			returnType = returnType == typeof(void)
				? typeof(object)
				: returnType;

			return SyncHandler.MakeGenericMethod(returnType).Invoke(this, invocationArgs);
		}
	}
}
