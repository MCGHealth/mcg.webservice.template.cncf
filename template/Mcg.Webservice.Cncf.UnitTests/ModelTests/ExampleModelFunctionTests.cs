using Mcg.Webservice.Cncf.Api.Models;
using NUnit.Framework;
using FluentAssertions;

namespace Mcg.Webservice.UnitTests.ModelTests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestFixture]
    public class ExampleModelFunctionTests
    {
        private ExampleModel control => new ExampleModel() { ID = 42, Username = "ArthurD", EmailAddress = "Ard@magrathea.com" };
        
        [TestCase(1, "user_1", "email_1@mcg.win", false)]
        [TestCase(42, "user_2", "email_2@mcg.win", true)]
        [TestCase(1, "ArthurD", "email_1@mcg.win", true)]
        [TestCase(1, "user_3", "Ard@magrathea.com", true)]
        public void Equals_returns_the_correct_value(int id, string username, string email, bool expectedResult)
        {
            ExampleModel other = new ExampleModel() { ID = id, Username = username, EmailAddress = email };

            _ = control.Equals(other).Should().Be(expectedResult);
        }

        [Test]
        public void Equals_returns_false_if_other_is_not_of_type_DefaultModel()
        {
            object other = new object();

            _ = control.Equals(other).Should().Be(false);
        }

        [Test]
        public void Equals_returns_false_if_other_is_null()
        {
            ExampleModel other = null;

            _ = control.Equals(other).Should().Be(false);
        }
    }
}