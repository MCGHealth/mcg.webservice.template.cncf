using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using AspectInjector.Broker;
using Mcg.Webservice.Cncf.Api.Infrastructure.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace Mcg.Webservice.Cncf.Api.Infrastructure.Logging
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	[Injection(typeof(LogAspect))]
	public sealed class LogAttribute : Attribute { }

	/// <summary>
	/// Adds logging to the decorated method.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	[Aspect(Scope.Global, Factory = typeof(AspectFactory))]
	public class LogAspect
	{
		internal const string INF_LOG_TEMPLATE = "{event} {host_name} {process_name} {elapsed_\u03BCs:0.0000} {method}";
		internal const string ERR_LOG_TEMPLATE = INF_LOG_TEMPLATE + " {arguments} {error_type} {error_message} {error_stack_trace}";

		internal static ILogger Log { get; set; } = Serilog.Log.Logger;

		private static readonly MethodInfo AsyncHandler = typeof(LogAspect)
			.GetMethod(nameof(LogAspect.WrapAsync), BindingFlags.NonPublic | BindingFlags.Static);

		private static readonly MethodInfo SyncHandler = typeof(LogAspect)
			.GetMethod(nameof(LogAspect.Wrap), BindingFlags.NonPublic | BindingFlags.Static);

		private static T Wrap<T>(string owningTypeName, string targetName, Func<object[], object> target, object[] args)
		{
			var sw = Stopwatch.StartNew();
			Exception ex = null;

			try
			{
				return (T)target(args);
			}
			catch (Exception e)
			{
				ex = GetRootException(e);
				throw;
			}
			finally
			{
				Write(owningTypeName, targetName, args, sw.ElapsedTicks, ex);
			}
		}

		private static async Task<T> WrapAsync<T>(string owningTypeName, string targetName, Func<object[], object> target, object[] args)
		{
			var sw = Stopwatch.StartNew();
			Exception ex = null;


			try
			{
				var result = await (Task<T>)target(args);
				return result;
			}
			catch (Exception e)
			{
				ex = GetRootException(e);
				throw;
			}
			finally
			{
				Write(owningTypeName, targetName, args, sw.ElapsedTicks, ex);
			}
		}

		private static void Write(string owningTypeName, string methodName, object[] args, long duration, Exception exception = null)
		{
			var eventName = $"{owningTypeName}_{methodName}".SafeString();
			double elapsed = (double)duration / AspectFactory.TickPerMicrosecond;

			if (exception != null)
			{
				Log.Error(
					ERR_LOG_TEMPLATE
					, eventName
					, Environment.MachineName
					, AppSettings.ServiceName
					, elapsed
					, methodName
					, JsonConvert.SerializeObject(args)
					, exception.GetType().FullName
					, exception.Message
					, HttpUtility.JavaScriptStringEncode(exception.StackTrace)
				);

				return;
			}

			Log.Information(
				INF_LOG_TEMPLATE
				, eventName
				, Environment.MachineName
				, AppSettings.ServiceName
				, elapsed
				, methodName
			);
		}

		internal static Exception GetRootException(Exception ex)
		{
			if (ex is TargetInvocationException)
			{
				GetRootException(ex.InnerException);
			}

			return ex;
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

			object[] invocationArgs =
				{
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
