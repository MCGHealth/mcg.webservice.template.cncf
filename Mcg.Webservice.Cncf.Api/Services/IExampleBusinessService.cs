using System.Collections.Generic;
using Mcg.Webservice.Cncf.Api.Models;

namespace Mcg.Webservice.Cncf.Api.Services
{
    public interface IExampleBusinessService
    {
        (bool ok, string error) Delete(ExampleModel model);
        (bool ok, string error, ExampleModel newModel) Insert(ExampleModel model);
        IEnumerable<ExampleModel> SelectAll();
        ExampleModel SelectByEmail(string email);
        ExampleModel SeledtById(int id);
        (bool ok, string error) Update(ExampleModel model);
    }
}
