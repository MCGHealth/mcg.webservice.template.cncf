using System;
using Mcg.Webservice.Cncf.Api.Infrastructure.Configuration;
using Mcg.Webservice.Cncf.Api.Models;
using Mcg.Webservice.Cncf.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mcg.Webservice.Cncf.Api.Controllers
{
    /// <summary>
    /// The default controller for the solution.
    /// </summary>
    /// <remarks>
    /// Feel free to reuse or delete this controller.  It's meant as an example.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController, Produces("application/json"), Consumes("application/json")]
    public class ExampleController : ControllerBase
    {
        internal IExampleBusinessService Logic { get; }

        internal IAppSettings Settings { get; }

        public ExampleController(IExampleBusinessService busLogic, IAppSettings settings)
        {
            this.Logic = busLogic ?? throw new ArgumentNullException(nameof(busLogic));
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), 200)] // ok
        [ProducesResponseType(typeof(string), 400)] // bad request
        [ProducesResponseType(typeof(string), 401)] // unauthorized
        [ProducesResponseType(typeof(string), 500)] // internal server error
        [ProducesResponseType(typeof(string), 502)] // upstream resource isn't available...ref https://tools.ietf.org/html/rfc2616?spm=5176.doc32013.2.3.Aimyd7#section-10.5.3
        public IActionResult Get()
        {
            var result = Logic.SelectAll();
            return Ok(result);
        }

        [HttpGet("id/{id}")]
        [ProducesResponseType(typeof(string), 200)] // ok
        [ProducesResponseType(typeof(string), 400)] // bad request
        [ProducesResponseType(typeof(string), 401)] // unauthorized
        [ProducesResponseType(typeof(string), 404)] // not found
        [ProducesResponseType(typeof(string), 500)] // internal server error
        [ProducesResponseType(typeof(string), 502)] // upstream resource isn't available...ref https://tools.ietf.org/html/rfc2616?spm=5176.doc32013.2.3.Aimyd7#section-10.5.3
        public IActionResult GetById(int id)
        {
            if(id < 1)
            {
                return BadRequest();
            }

            var r = Logic.SeledtById(id);

            if(r == null)
            {
                return NotFound();
            }

            return Ok(r);
        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(string), 200)] // ok
        [ProducesResponseType(typeof(string), 400)] // bad request
        [ProducesResponseType(typeof(string), 401)] // unauthorized
        [ProducesResponseType(typeof(string), 404)] // not found
        [ProducesResponseType(typeof(string), 500)] // internal server error
        [ProducesResponseType(typeof(string), 502)] // upstream resource isn't available...ref https://tools.ietf.org/html/rfc2616?spm=5176.doc32013.2.3.Aimyd7#section-10.5.3
        public IActionResult GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest();
            }

            var r = Logic.SelectByEmail(email);

            if (r == null)
            {
                return NotFound();
            }

            return Ok(r);
        }

        [HttpPost]
        [ProducesResponseType(202)]                 // since this is an async command, the return value is simply 'Accepted'.
        [ProducesResponseType(typeof(string), 400)] // bad request
        [ProducesResponseType(typeof(string), 401)] // unauthorized
        [ProducesResponseType(typeof(string), 409)] // conflict: resource already exists
        [ProducesResponseType(typeof(string), 412)] // required request headers are wrong/missing
        [ProducesResponseType(typeof(string), 422)] // 422 Unprocessable Entity => resource already exists...ref https://tools.ietf.org/html/rfc4918#section-11.2
        [ProducesResponseType(typeof(string), 500)] // internal server error
        [ProducesResponseType(typeof(string), 502)] // upstream resource isn't available...ref https://tools.ietf.org/html/rfc2616?spm=5176.doc32013.2.3.Aimyd7#section-10.5.3
        public IActionResult Post([FromBody] ExampleModel value)
        {
            if( value == null)
            {
                return BadRequest();
            }

            var (ok, error, newModel) = Logic.Insert(value);

            if (!ok)
            {
                return new UnprocessableEntityObjectResult(error);
            }

            var result = new CreatedAtActionResult(nameof(this.GetById), "example", new { id = newModel.ID }, newModel);

            return result;
        }

        [HttpPut]
        [ProducesResponseType(204)]                 // updated successfully
        [ProducesResponseType(typeof(string), 400)] // bad request
        [ProducesResponseType(typeof(string), 401)] // unauthorized
        [ProducesResponseType(typeof(string), 404)] // resource not found
        [ProducesResponseType(typeof(string), 412)] // required request headers are wrong/missing
        [ProducesResponseType(typeof(string), 500)] // internal server error
        [ProducesResponseType(typeof(string), 502)] // upstream resource isn't available...ref https://tools.ietf.org/html/rfc2616?spm=5176.doc32013.2.3.Aimyd7#section-10.5.3
        public IActionResult Put([FromBody] ExampleModel value)
        {
            if (value == null)
            {
                return BadRequest();
            }

            var (ok, error) = this.Logic.Update(value);
            if (!ok)
            {
                return new UnprocessableEntityObjectResult(error);
            }

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(204)]                 // deleted successfully
        [ProducesResponseType(typeof(string), 400)] // bad request
        [ProducesResponseType(typeof(string), 401)] // unauthorized
        [ProducesResponseType(typeof(string), 412)] // required request headers are wrong/missing
        [ProducesResponseType(typeof(string), 500)] // internal server error
        [ProducesResponseType(typeof(string), 502)] // upstream resource isn't available...ref https://tools.ietf.org/html/rfc2616?spm=5176.doc32013.2.3.Aimyd7#section-10.5.3
        public IActionResult Delete(ExampleModel value)
        {
            if (value == null)
            {
                return BadRequest();
            }

            var (ok, error) = this.Logic.Delete(value);
            if (!ok)
            {
                return new UnprocessableEntityObjectResult(error);
            }

            return NoContent();
        }
    }
}
