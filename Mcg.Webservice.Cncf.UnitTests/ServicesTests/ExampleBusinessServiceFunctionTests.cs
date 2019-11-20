using System;
using FluentAssertions;
using Mcg.Webservice.Cncf.Api.Services;
using NUnit.Framework;

namespace Mcg.Webservice.UnitTests.ServicesTests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestFixture]
    public class ExampleBusinessServiceFunctionTests
    {
        [Test]
        public void Ctor_throws_ArgumentNullException_for_null_dataAccess_arg()
        {
            Action action = () =>
            {
                _ = new ExampleBusinessService(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
