using System;
using System.Collections.Generic;
using System.Linq;
using Mcg.Webservice.Cncf.Api.Infrastructure.Instrumentation;
using Mcg.Webservice.Cncf.Api.Infrastructure.Logging;
using Mcg.Webservice.Cncf.Api.Models;

namespace Mcg.Webservice.Cncf.Api.DataAccess
{
    /// <summary>
    /// This is just a class to provide a simple in-memory data layer so the
    /// template can demonstrate accessing data.  Please delete this code file
    /// before testing.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ExampleDataRepository : IExampleDataRepository
    {
        private readonly Dictionary<int, ExampleModel> Table = new Dictionary<int, ExampleModel>();


        [Instrument, Log]
        public IEnumerable<ExampleModel> SelectAll()
        {
            return Table.Values.ToArray();
        }

        [Instrument, Log]
        public ExampleModel SelectOneById(int id)
        {
            return Table[id];
        }

        [Instrument, Log]
        public ExampleModel SelectOneByEmail(string email)
        {
            return Table.Values.FirstOrDefault(m => m.EmailAddress.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        [Instrument, Log]
        public void Insert(ExampleModel model)
        {
            Table.Add(model.ID, model);
        }

        [Instrument, Log]
        public void Update(ExampleModel model)
        {
            Table[model.ID] = model;
        }

        [Instrument, Log]
        public void Delete(ExampleModel model)
        {
            if (!Table.ContainsKey(model.ID))
            {
                return;
            }

            Table.Remove(model.ID, out _);
        }

        public bool Contains(int id)
        {
            return Table.ContainsKey(id);
        }

        public bool Contains(string email)
        {
            return Table.Values.Any(m => m.EmailAddress.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public int NextId()
        {
            if (Table.Count == 0)
            {
                return 1;
            }

            return Table.Count() + 1;
        }
    }
}
