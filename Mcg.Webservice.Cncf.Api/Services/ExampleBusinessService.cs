using System;
using System.Collections.Generic;
using Mcg.Webservice.Cncf.Api.DataAccess;
using Mcg.Webservice.Cncf.Api.Infrastructure.Configuration;
using Mcg.Webservice.Cncf.Api.Infrastructure.Instrumentation;
using Mcg.Webservice.Cncf.Api.Infrastructure.Logging;
using Mcg.Webservice.Cncf.Api.Models;

namespace Mcg.Webservice.Cncf.Api.Services
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Instrument, Log]
    public class ExampleBusinessService: IExampleBusinessService
    {
        internal IExampleDataRepository DataAccess { get; set; }

        public ExampleBusinessService(IExampleDataRepository dataAccess)
        {
            DataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        public (bool ok, string error) Delete(ExampleModel model)
        {
            if (model == null)
            {
                return (ok: false, error: "The model is null.");
            }

            DataAccess.Delete(model);

            return (ok: true, error: null);
        }

        public (bool ok, string error, ExampleModel newModel) Insert(ExampleModel model)
        {
            if (DataAccess.Contains(model.ID))
            {
                return (ok: false, error: $"A model with the id {model.ID} already exists.", newModel: null);
            }

            if (DataAccess.Contains(model.EmailAddress))
            {
                return (ok: false, error: $"A model with the email {model.EmailAddress} already exists.", newModel: null);
            }

            if (model.ID < 1)
            {
                model.ID = DataAccess.NextId();
            }

            DataAccess.Insert(model);

            return (ok: true, error: null, model);
        }

        public IEnumerable<ExampleModel> SelectAll()
        {
            return DataAccess.SelectAll();
        }

        public ExampleModel SelectByEmail(string email)
        {
            return DataAccess.SelectOneByEmail(email);
        }

        public ExampleModel SeledtById(int id)
        {
            return DataAccess.SelectOneById(id);
        }

        public (bool ok, string error) Update(ExampleModel model)
        {
            if (!DataAccess.Contains(model.ID))
            {
                return (ok: false, error: $"Entity not found in data store.");
            }

            return (ok: true, error: null);
        }
    }
}
