using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Mcg.Webservice.Cncf.Api.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;

namespace Mcg.Webservice.UnitTests.InfrastructureTests.Logging
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestFixture]
    public class RequestLoggingMiddlewareTests
    {
        private DefaultHttpContext TestContext()
        {
            DefaultHttpContext context = new DefaultHttpContext();
            context.Request.Body = new MemoryStream();
            context.Request.Method = "GET";
            context.Request.Path = "/this/is/a/path";
            context.Request.PathBase = new PathString("/test");
            context.Request.Scheme = "http";
            context.Request.Host = new HostString("www.unittests.fake");
            context.Response.Body = new MemoryStream();

            return context;
        }

        [Test]
        public async Task Invoke_logs_as_expected()
        {
            var logger = new LoggerConfiguration()
                               .WriteTo.TestCorrelator()
                               .CreateLogger();


            var mw = new RequestLoggingMiddleware((innerHttpContext) => Task.FromResult(0), Helpers.MockIAppSettings(), logger);

            var context = TestContext();

            using (TestCorrelator.CreateContext())
            {
                await mw.InvokeAsync(context);

                var logEvent = TestCorrelator.GetLogEventsFromCurrentContext().FirstOrDefault();

                using var scope = new AssertionScope();
                logEvent.Should().NotBeNull();
                Assert.AreEqual(RequestLoggingMiddleware.DBG_LOG_TEMPLATE, logEvent.MessageTemplate.Text);
                logEvent.Level.Should().Be(LogEventLevel.Information);
            }
        }
    }
}
