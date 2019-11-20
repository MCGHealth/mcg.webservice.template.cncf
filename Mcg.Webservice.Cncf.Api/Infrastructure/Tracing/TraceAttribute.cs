using System;
using System.Reflection;
using System.Threading.Tasks;
using AspectInjector.Broker;
using Mcg.Webservice.Cncf.Api.Infrastructure.Configuration;
using OpenTracing;
using OpenTracing.Tag;
using OpenTracing.Util;

namespace Mcg.Webservice.Cncf.Api.Infrastructure.Tracing
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Method)]
    [Injection(typeof(TraceAspect))]
    public sealed class TraceAttribute : Attribute { }

    /// <summary>
    /// Adds distributed tracing to the decorated method.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Aspect(Scope.Global, Factory = typeof(AspectFactory))]
    public class TraceAspect
    {
        private static readonly MethodInfo AsyncHandler = typeof(TraceAspect)
            .GetMethod(nameof(TraceAspect.WrapAsync), BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo SyncHandler = typeof(TraceAspect)
            .GetMethod(nameof(TraceAspect.WrapSync), BindingFlags.NonPublic | BindingFlags.Static);

        [Advice(Kind.Around, Targets = Target.Method)]
        public object Handle(
            [Argument(Source.Type)] Type owningType,
            [Argument(Source.Name)] string targetName,
            [Argument(Source.Target)] Func<object[], object> targetFunc,
            [Argument(Source.Arguments)] object[] inputArgs,
            [Argument(Source.ReturnType)] Type returnType)
        {
            object[] invocationArgs = {
                $"{owningType.Name.ToLower()}_{targetName.ToLower()}",
                targetFunc,
                inputArgs,
            };

            if (typeof(Task).IsAssignableFrom(returnType))
            {
                var syncResultType = returnType.IsConstructedGenericType ? returnType.GenericTypeArguments[0] : AspectFactory.VoidTaskResult;
                return AsyncHandler.MakeGenericMethod(syncResultType).Invoke(this, invocationArgs);
            }
            else
            {
                returnType = returnType == typeof(void) ? typeof(object) : returnType;
                return SyncHandler.MakeGenericMethod(returnType).Invoke(this, invocationArgs);
            }
        }

        private static T WrapSync<T>(string eventName, Func<object[], object> target, object[] args)
        {
            ITracer tracer = GlobalTracer.Instance;

            using IScope scope = tracer.BuildSpan(eventName)
                .WithTag("machine", Environment.MachineName)
                .WithTag(Tags.Component.Key, AppSettings.ServiceName)
                .StartActive(true);
            try
            {
                var result = (T)target(args);
                return result;
            }
            catch
            {
                Tags.Error.Set(scope.Span, true);
                throw;
            }
        }

        private static async Task<T> WrapAsync<T>(string eventName, Func<object[], object> target, object[] args)
        {
            ITracer tracer = GlobalTracer.Instance;

            using IScope scope = tracer.BuildSpan(eventName)
                .WithTag("machine", Environment.MachineName)
                .WithTag(Tags.Component.Key, AppSettings.ServiceName)
                .StartActive(true);
            try
            {
                var result = await (Task<T>)target(args);
                return result;
            }
            catch
            {
                Tags.Error.Set(scope.Span, true);
                throw;
            }
        }
    }
}
