using System.Collections.Generic;
using Mcg.Webservice.Cncf.Api.Models;

namespace Mcg.Webservice.Cncf.Api.DataAccess
{
    public interface IExampleDataRepository
    {
        void Delete(ExampleModel model);
        void Insert(ExampleModel model);
        IEnumerable<ExampleModel> SelectAll();
        ExampleModel SelectOneByEmail(string email);
        ExampleModel SelectOneById(int id);
        void Update(ExampleModel model);
        bool Contains(int id);
        bool Contains(string email);
        int NextId();
    }
}
