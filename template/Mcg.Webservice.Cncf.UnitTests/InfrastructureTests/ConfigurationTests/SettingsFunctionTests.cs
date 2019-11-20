using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Execution;
using Mcg.Webservice.Cncf.Api.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Serilog.Events;

namespace Mcg.Webservice.UnitTests.ConfigurationTests
{

    [TestFixture]
    public class SettingsFunctionTests
    {
       [Test]
       public void Ctor_throws_ArgumentNullException_if_arg_config_is_null()
        {
            Action action = () =>
            {
                IConfiguration config = null;
                _ = new AppSettings(config);
            };

            action.Should().Throw<NullReferenceException>();
        }

        [Test]
        public void Ctor_throws_ArgumentNullException_if_arg_configData_is_null()
        {
            Action action = () =>
            {
                IDictionary<string,string> configData = null;
                _ = new AppSettings(configData);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void RawConfiguration_Count_returns_expected_value()
        {
            var control = Helpers.TestConfigValues();
            var settings = new AppSettings(control);

            settings.RawConfiguration.Count.Should().Be(13);
        }

        [Test]
        public void RawConfiguration_contains_expected_keys()
        {
            var control = Helpers.TestConfigValues();
            var settings = new AppSettings(control);


            using var scope = new AssertionScope();

            foreach(var k in control.Keys)
            {
                settings.RawConfiguration.Keys.Contains(k)
                    .Should()
                    .BeTrue();
            }

            foreach (var k in control.Keys)
            {
                settings.RawConfiguration.Keys.Contains(k)
                    .Should()
                    .BeTrue();
            }

            var roValues = new[] { "IS_DEV_ENVIRONMENT", "SERVICE_NAME", "HOST_NAME", "OS_PLATFORM", "OS_VERSION" };

            foreach (var k in roValues)
            {
                settings.RawConfiguration.Keys.Contains(k)
                    .Should()
                    .BeTrue();
            }
        }

        [Test]
        public void Instance_contains_expected_values()
        {
            var control = Helpers.TestConfigValues();
            var settings = new AppSettings(control);


            using var scope = new AssertionScope();
            settings.IsDevEnvironment.Should().BeTrue();
            settings.LogLevel.Should().Be(LogEventLevel.Verbose);

            foreach (var kp in control)
            {
                settings[kp.Key].Should().Be(kp.Value);
            }
        }

        [Test]
        public void Update_correctly_updates_value()
        {
            var control = Helpers.TestConfigValues();
            var settings = new AppSettings(control);


            using var scope = new AssertionScope();
            settings.LogLevel.Should().Be(LogEventLevel.Verbose);

            settings.Update("LOG_LEVEL", "warning");

            settings.LogLevel.Should().Be(LogEventLevel.Warning);
        }

        [Test]
        public void Update_does_nothing_if_not_LOG_LEVEL()
        {
            var control = Helpers.TestConfigValues();
            var settings = new AppSettings(control);


            using var scope = new AssertionScope();
            var expected = settings["CORS_ALLOWED_URLS"];

            settings.Update("CORS_ALLOWED_URLS", "thunk");

            settings["CORS_ALLOWED_URLS"].Should().Be(expected);
        }

        [Test]
        public void Update_throws_InvalidCastException()
        {
            var control = Helpers.TestConfigValues();
            var settings = new AppSettings(control);

            Action action = () =>
            {
                settings.Update("LOG_LEVEL", "borked");
            };

            action.Should().Throw<TargetInvocationException>()
                .WithInnerException< InvalidCastException>();
        }
    }
}
