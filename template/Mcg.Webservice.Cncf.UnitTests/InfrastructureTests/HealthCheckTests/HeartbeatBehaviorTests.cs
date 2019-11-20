using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Mcg.Webservice.Cncf.Api.Infrastructure.HealthChecks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Mcg.Webservice.UnitTests.InfrastructureTests.HealthChecks
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	[TestFixture]
	public class HeartbeatBehaviorTests
	{
		[Test]
		public void Heartbeat_is_decorated_with_JSON_attributes_as_expected()
		{
			Type target = typeof(Heartbeat);
			target.Should().BeDecoratedWith<JsonObjectAttribute>();

			using (new AssertionScope())
			{
				target.GetProperty("Status")
					.Should().BeDecoratedWith<JsonPropertyAttribute>(p => p.PropertyName == "status")
					.And.BeDecoratedWith<JsonRequiredAttribute>();

				target.GetProperty("URL")
					.Should().BeDecoratedWith<JsonPropertyAttribute>(p => p.PropertyName == "url")
					.And.BeDecoratedWith<JsonRequiredAttribute>();

				target.GetProperty("UTCDatetime")
					.Should().BeDecoratedWith<JsonPropertyAttribute>(p => p.PropertyName == "utcdatetime")
					.And.BeDecoratedWith<JsonRequiredAttribute>();

				target.GetProperty("Machine")
					.Should().BeDecoratedWith<JsonPropertyAttribute>(p => p.PropertyName == "machine")
					.And.BeDecoratedWith<JsonRequiredAttribute>();

				target.GetProperty("RequestDurationMS")
					.Should().BeDecoratedWith<JsonPropertyAttribute>(p => p.PropertyName == "request_duration_microseconds")
					.And.BeDecoratedWith<JsonRequiredAttribute>();

				target.GetProperty("Message")
					.Should().BeDecoratedWith<JsonPropertyAttribute>(p => p.PropertyName == "message" && p.NullValueHandling == NullValueHandling.Ignore);
			}
		}
	}
}
