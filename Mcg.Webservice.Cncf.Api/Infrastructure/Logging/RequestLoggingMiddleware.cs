using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mcg.Webservice.Cncf.Api.Infrastructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Serilog.Events;

namespace Mcg.Webservice.Cncf.Api.Infrastructure.Logging
{
    /// <summary>
    /// An extension method used to register the <see cref="RequestLoggingMiddleware"/>.
    /// </summary>
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }

    /// <summary>
    /// Adds logging to the all RESTful endpoints in the solution.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        internal const string INF_LOG_TEMPLATE = "{event} {host_name} {process_name} {elapsed_ms} {http_method} {http_request_path} {user_id} {http_status_code}";
        internal const string DBG_LOG_TEMPLATE = INF_LOG_TEMPLATE + " {http_request_body} {http_response_body}";
        internal const string ERR_LOG_TEMPLATE = DBG_LOG_TEMPLATE + " {error_type} {error_message} {error_stack_trace}";

        internal static string[] IgnoredPaths => new[] { "/ops", "/swagger" };

        internal RequestDelegate Next { get; set; }

        internal Serilog.ILogger Logger { get; set; }

        internal IAppSettings Settings { get; set; }

        internal LogEventLevel CurrentLoggingLevel { get; set; }

        /// <summary>
        /// Default ctor.
        /// </summary>
        /// <param name="nextAction">The request delegate to be called next.</param>
        public RequestLoggingMiddleware(RequestDelegate nextAction, IAppSettings appSettings) : this(nextAction, appSettings, Serilog.Log.Logger) { }

        /// <summary>
        /// Used internally for testing and mock injection.
        /// </summary>
        /// <param name="nextAction">The request delegate to be called next.</param>
        /// <param name="appSettings">The application settings to be used to fulfil the call.</param>
        internal RequestLoggingMiddleware(RequestDelegate nextAction, IAppSettings appSettings, Serilog.ILogger logger)
        {
            Next = nextAction ?? throw new ArgumentNullException(nameof(nextAction));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Settings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopWatch = System.Diagnostics.Stopwatch.StartNew();

            context.Request.EnableBuffering();

            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            var level = CurrentLoggingLevel;
            HttpEventData httpEventData = new HttpEventData();

            var template = level < LogEventLevel.Information
                ? DBG_LOG_TEMPLATE
                : INF_LOG_TEMPLATE;

            try
            {
                await Next(context);
                httpEventData = await ReadRequestInfo(context);
            }
            catch (Exception ex)
            {
                template = ERR_LOG_TEMPLATE;
                level = LogEventLevel.Error;
                httpEventData.Exception = ex;
                context.Response.StatusCode = 500;
                httpEventData.StatusCode = 500;
            }
            finally
            {
                httpEventData.ElapsedMS = stopWatch.ElapsedMilliseconds;
                var data = httpEventData.GetEventData(Settings.LogLevel);

                if (level >= LogEventLevel.Error)
                {
                    Logger.Error(template, data);
                }
                else
                {
                    Logger.Information(template, data);
                }

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }


        private async Task<HttpEventData> ReadRequestInfo(HttpContext context)
        {
            var body = context.Request.Body;

            var routeData = context.GetRouteData();

            var controller = routeData != null ? routeData.Values["controller"]?.ToString().SafeString() :
                "controller_not_available";

            var action = routeData != null ? routeData.Values["action"]?.ToString().SafeString() :
               "action_not_available";

            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];

            await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);

            context.Request.Body = body;

            var requestData = new HttpEventData()
            {
                ControllerName = controller,
                ActionName = action,
                RequestBody = System.Text.Encoding.UTF8.GetString(buffer),
                Method = context.Request.Method,
                UserID = context.User.Identity.Name
            };

            context.Response.Body.Seek(0, SeekOrigin.Begin);

#pragma warning disable IDE0067
            requestData.ResponseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
#pragma warning restore IDE0067 

            requestData.StatusCode = context.Response.StatusCode;
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            requestData.RequestPath = context.Request.GetDisplayUrl();

            return requestData;
        }
    }
}
