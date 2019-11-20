using System.Collections.Generic;
using System.IO;
using Mcg.Webservice.Cncf.Api.DataAccess;
using Mcg.Webservice.Cncf.Api.Infrastructure.Configuration;
using Mcg.Webservice.Cncf.Api.Models;
using Mcg.Webservice.Cncf.Api.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace Mcg.Webservice.UnitTests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [SetUpFixture]
    internal static class Helpers
    {
        internal static ExampleModel TestModel => new ExampleModel() { EmailAddress = "unit@test.cs", ID = 42, Username = "Testy McTestFace" };

        internal static ExampleModel NullModel => null;

        internal static IExampleDataRepository MockExampleDataAccess()
        {
            var da = new Mock<IExampleDataRepository>();

            da.Setup(b => b.SelectAll()).Returns(new[] { TestModel });
            da.Setup(b => b.SelectOneById(It.IsAny<int>()))
                 .Returns((int id) =>
                 {
                     if (id == 42)
                     {
                         return TestModel;
                     }
                     else
                     {
                         return NullModel;
                     }
                 });

            da.Setup(b => b.SelectOneByEmail(It.IsAny<string>()))
                .Returns((string email) =>
                {
                    if (email == "unit@test.cs")
                    {
                        return TestModel;
                    }
                    else
                    {
                        return NullModel;
                    }
                });

            da.Setup(b => b.Insert(It.IsAny<ExampleModel>()));

            da.Setup(b => b.Update(It.IsAny<ExampleModel>()));

            da.Setup(b => b.Delete(It.IsAny<ExampleModel>()));

            da.Setup(b => b.NextId()).Returns(43);

            return da.Object;
        }

        internal static IExampleBusinessService MockExampleBusinessLogic()
        {
            var buslog = new Mock<IExampleBusinessService>();

            buslog.Setup(b => b.SelectAll()).Returns(new[] { TestModel });

            buslog.Setup(b => b.SeledtById(It.IsAny<int>()))
                 .Returns((int id) =>
                 {
                     if (id == 42)
                     {
                         return TestModel;
                     }
                     else
                     {
                         return NullModel;
                     }
                 });

            buslog.Setup(b => b.SelectByEmail(It.IsAny<string>()))
                .Returns((string email) =>
                {
                    if (email == "unit@test.cs")
                    {
                        return TestModel;
                    }
                    else
                    {
                        return NullModel;
                    }
                });

            buslog.Setup(b => b.Insert(It.IsAny<ExampleModel>()))
                .Returns((ExampleModel newModel) =>
                {
                    if (newModel.Equals(TestModel))
                    {
                        return (true, null, newModel);
                    }
                    else
                    {
                        return (false, "test error message", null);
                    }
                });

            buslog.Setup(b => b.Update(It.IsNotNull<ExampleModel>()))
                 .Returns((ExampleModel newModel) =>
                 {
                     if (newModel.Equals(TestModel))
                     {
                         return (true, null);
                     }
                     else
                     {
                         return (false, "test error message");
                     }
                 });

            buslog.Setup(b => b.Delete(It.IsNotNull<ExampleModel>())).Returns((true, null));


            var mock = buslog.Object;

            return mock;
        }

        internal static IAppSettings MockIAppSettings(string logLevel = "debug")
        {
            var settings = new Mock<IAppSettings>();

            settings.SetupGet(m => m["LOG_LEVEL"]).Returns(logLevel);
            settings.SetupGet(m => m["AZURE_SVC_BUS_URI"]).Returns("sb://mcg-templates.servicebus.windows.net/");
            settings.SetupGet(m => m["AZURE_SHARED_ACCESS_KEY_NAME"]).Returns("RootManageSharedAccessKey");
            settings.SetupGet(m => m["AZURE_SHARED_ACCESS_KEY_VALUE"]).Returns("wYBGR0iaFwT5ESrHR34Z57JgV0hm2cs8lqMGIC1B");
            settings.SetupGet(m => m["AZURE_PUBLISH_QUEUE"]).Returns("example.request");
            settings.SetupGet(m => m["AZURE_DEFAULT_CONSUMER_MAX_CONCURRENT_CALLS"]).Returns("1");
            settings.SetupGet(m => m["AZURE_DEFAULT_CONSUMER_AUTO_COMPLETE"]).Returns("false");
            settings.SetupGet(m => m["OPS_PORT"]).Returns("1234");

            return settings.Object;
        }


        internal static IDictionary<string, string> TestConfigValues() =>
            new Dictionary<string, string>()
            {
                { "CORS_ALLOWED_URLS", "*"},
                { "ASPNETCORE_ENVIRONMENT", "Development" },
                { "ELASTIC_SEARCH_URI", "http://test/uri" },
                { "JAEGER_AGENT_PORT", "localhost" },
                { "JAEGER_SAMPLER_TYPE" , "6831"},
                { "JAEGER_SERVICE_NAME", "Mcg.Webservice" },
                { "LOG_LEVEL","verbose" },
                { "MEANING_OF_LIFE","42" },
            };


        internal static DefaultHttpContext TestContext()
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
    }
}
