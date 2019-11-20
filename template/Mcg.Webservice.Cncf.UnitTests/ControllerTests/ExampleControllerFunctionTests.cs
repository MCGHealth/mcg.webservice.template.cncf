using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Mcg.Webservice.Cncf.Api.Controllers;
using Mcg.Webservice.Cncf.Api.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Mcg.Webservice.UnitTests.ControllerTests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestFixture]
    public class ExampleControllerFunctionTests
    {
        [Test]
        public void Get_returns_expected_results()
        {
            var controller = new ExampleController(Helpers.MockExampleBusinessLogic(), Helpers.MockIAppSettings());

            var response = controller.Get() as OkObjectResult;

            response.StatusCode.Should().Be(200);

            var value = response.Value as IEnumerable<ExampleModel>;

            value.Should().NotBeEmpty();

            var enumerator = value.GetEnumerator();

            enumerator.MoveNext().Should().BeTrue();

            enumerator.Current.Should().BeEquivalentTo(Helpers.TestModel);
        }

        [TestCase(12, typeof(NotFoundResult))]
        [TestCase(42, typeof(OkObjectResult))]
        [TestCase(0, typeof(BadRequestResult))]
        public void Get_by_id_returns_expected_results(int id, Type responseType)
        {
            var controller = new ExampleController(Helpers.MockExampleBusinessLogic(), Helpers.MockIAppSettings());

            var response = controller.GetById(id);

            response.Should().BeOfType(responseType);

            if (response is OkObjectResult)
            {
                var result = ((OkObjectResult)response).Value as ExampleModel;

                Assert.IsTrue(result.Equals(Helpers.TestModel));
            }
        }

        [TestCase("", typeof(BadRequestResult))]
        [TestCase("     ", typeof(BadRequestResult))]
        [TestCase(null, typeof(BadRequestResult))]
        [TestCase("unit@test.cs", typeof(OkObjectResult))]
        [TestCase("jim@aol.com", typeof(NotFoundResult))]
        public void Get_by_email_returns_expected_results(string email, Type responseType)
        {
            var controller = new ExampleController(Helpers.MockExampleBusinessLogic(), Helpers.MockIAppSettings());

            var response = controller.GetByEmail(email);

            response.Should().BeOfType(responseType);

            if (response is OkObjectResult)
            {
                var result = ((OkObjectResult)response).Value as ExampleModel;

                Assert.IsTrue(result.Equals(Helpers.TestModel));
            }
        }

        [Test]
        public void Post_returns_expected_responses()
        {
            var controller = new ExampleController(Helpers.MockExampleBusinessLogic(), Helpers.MockIAppSettings());

            using var scope = new AssertionScope();
            var response = controller.Post(Helpers.TestModel);
            response.Should().BeAssignableTo<CreatedAtActionResult>();

            response = controller.Post(Helpers.NullModel);
            response.Should().BeAssignableTo<BadRequestResult>();

            response = controller.Post(new ExampleModel());
            response.Should().BeAssignableTo<UnprocessableEntityObjectResult>();
        }

        [Test]
        public void Put_returns_NoContent()
        {
            var controller = new ExampleController(Helpers.MockExampleBusinessLogic(), Helpers.MockIAppSettings());

            var response = controller.Put(Helpers.TestModel);
            response.Should().BeAssignableTo<NoContentResult>();
        }

        [Test]
        public void Put_returns_BadRequest()
        {
            var controller = new ExampleController(Helpers.MockExampleBusinessLogic(), Helpers.MockIAppSettings());
            var response = controller.Put(Helpers.NullModel);
            response.Should().BeAssignableTo<BadRequestResult>();
        }



        [Test]
        public void Delete_returns_NoContent()
        {
            var controller = new ExampleController(Helpers.MockExampleBusinessLogic(), Helpers.MockIAppSettings());
            var response = controller.Delete(Helpers.TestModel);
            response.Should().BeAssignableTo<NoContentResult>();
        }

        [Test]
        public void Delete_returns_BadRequest()
        {
            var controller = new ExampleController(Helpers.MockExampleBusinessLogic(), Helpers.MockIAppSettings());
            var response = controller.Delete(Helpers.NullModel);
            response.Should().BeAssignableTo<BadRequestResult>();
        }
    }
}
