using System;
using System.Collections.Generic;
using Serilog.Events;

namespace Mcg.Webservice.Cncf.Api.Infrastructure.Logging
{
    ///<summary>
    /// Used by the RequestLoggingMiddleware to ensure that all reqired information about a request/response is logged.
    ///</summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class HttpEventData
    {
        internal string ControllerName { get; set; }

        internal string ActionName { get; set; }

        internal string EventName { get { return $"{ControllerName}_{ActionName}".SafeString(); } }

        internal string RequestPath { get; set; }

        internal string RequestBody { get; set; }

        internal string Method { get; set; }

        internal string UserID { get; set; }

        internal int StatusCode { get; set; }

        internal string ResponseBody { get; set; }

        internal Exception Exception { get; set; }

        internal long ElapsedMS { get; set; }

        internal string ServiceName =>  typeof(HttpEventData).Assembly.GetName().Name.SafeString();

        internal object[] GetEventData(LogEventLevel logLevel)
        {
            var data = new List<object>
                    {
                        EventName,
                        Environment.MachineName,
                        ServiceName,
                        ElapsedMS,
                        Method,
                        RequestPath,
                        UserID,
                        StatusCode,
                    };

            if (logLevel < LogEventLevel.Information || logLevel > LogEventLevel.Warning)
            {
                data.AddRange(new string[]
                {
                        RequestBody ?? "[not available]",
                        ResponseBody ?? "[not available]"
                });
            }

            if (Exception != null)
            {
                data.AddRange(new string[]
                {
                        Exception.GetType().FullName,
                        Exception.Message,
                        Exception.StackTrace
                });
            }

            return data.ToArray();
        }
    }
}
