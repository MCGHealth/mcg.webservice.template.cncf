# Mcg.Webservice.Cncf.Api.Infrastructure

Contains all logic related to cross cutting and operational concerns, i.e., logging, tracing, metrics, APM, etc..

## Files

| **Item**                                              | **Description**                                                                                                                                                      |
| ----------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [**HealthChecks**](./HealthChecks/Readme.md)          | HealthCheck functionality build upon ASP.Net Core's[HealthCheck API](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.0) |
| [**Configuration**](./Configuration/Readme.md)        | Configuration functionality build upon ASP.Net Core's[Configuration](https://docs.microsoft.com/en-us/zaspnet/core/fundamentals/configuration/?view=aspnetcore-3.0)  |
| [**Instrumentation.cs**](./Instrumentation/Readme.md) | Instrumentation (metrics) using [Prometheus](https://prometheus.io)                                                                                                  |
| [**Logging**](./Logging/Readme.md)                    | Structured, leveled logging [Serilog](https://serilog.net/)                                                                                                          |
| [**Tracing.cs**](./Tracing/Readme.md)                 | Distributed tracing using [OpenTracing](https://opentracing.io/)                                                                                                     |
|                                                       |                                                                                                                                                                      |

## Aspect Oriented Programming

This solution makes use of AOP paradigms to cleanly separate out code for cross-cutting concerns from business logic. The key framework that allows this to happen is a brilliant piece of work called [Aspect-Injector](https://github.com/pamidur/aspect-injector) by [Oleksandr Hulyi](https://github.com/pamidur).

In a nutshell what is allows is to change something like this, which may be repeated throughout the codebase :

```csharp
public void Insert(object model)
{
    var metricName = "mytype_foo";
    var sw = Stopwatch.StartNew();
    var success = true;

    metrics.IncGauge($"{metricName}_gauge");

    try
    {
        someDataConnector.Insert(model)

        Log.Information(
             INF_LOG_TEMPLATE
             , eventName
             , Environment.MachineName
             , AppSettings.ServiceName
             , sw.ElapsedMilliseconds
             , nameof(Insert)
         );
    }
    catch(Exception ex)
    {
        success = false;
        Log.Error(
                 ERR_LOG_TEMPLATE
                 , eventName
                 , Environment.MachineName
                 , AppSettings.ServiceName
                 , sw.ElapsedMilliseconds
                 , nameof(Insert)
                 , JsonConvert.SerializeObject(model)
                 , exception.GetType().FullName
                 , exception.Message
                 , HttpUtility.JavaScriptStringEncode(ex.StackTrace)
             );
    }
    finally
    {
        var eventName = $"{owningTypeName}_{methodName}".SafeString();
        double elapsed = (double) sw.ElapsedTicks / (double)(TimeSpan.TicksPerMillisecond / 1000);

        metrics.DecGauge($"{eventName}_gauge");
        metrics.IncCounter($"{eventName}_count", success);
        metrics.IncHistogram($"{eventName}_elapsed_microseconds", elapsed, success);
    }
}
```

To something much, _MUCH_ more concise, like this:

```csharp
[Log, Instrument]
public void Insert(object model)
{
    someDataConnector.Insert(model);
}
```

It can really make the size of your souce code shrink dramatically, plus it makes it much easer to see what the business logic is doing without having to comb through the boilerplate.

The files that implement the cross-cutting concerns are:

- [LogAttribute.cs](./Logging/LogAttribute.cs)
- [MetricsAttribute.cs](./Metrics/MetricsAttribute.cs)
- [TraceAttribute.cs](./Tracing/TraceAttribute.cs)

For more details on how to use this fantastic framework, go read the [Aspect Injector Docs](https://github.com/pamidur/aspect-injector/tree/master/docs).
