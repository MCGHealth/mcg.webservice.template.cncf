using System;
using System.Linq;
using System.Threading.Tasks;
using Mcg.Webservice.Cncf.Api.Infrastructure.Instrumentation;
using Mcg.Webservice.Cncf.Api.Infrastructure.Logging;
using Mcg.Webservice.Cncf.Api.Infrastructure.Tracing;

namespace Mcg.Webservice.UnitTests.InfrastructureTests
{
    /// <summary>
    /// This class is used to help test the different aspects.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class ObservedTarget
    {
        public ObservedTarget() { }

        internal bool ThrowsEx { get; } = false;

        public ObservedTarget(bool throwsEx)
        {
            this.ThrowsEx = throwsEx;
        }

        [Instrument, Log, Trace]
        public int Add(params int[] values)
        {
            System.Threading.Thread.Sleep(50); // -> create an exaggerated elapsed ms for test verification.

            if (ThrowsEx)
            {
                throw new InvalidOperationException("the exception is invalid.");
            }

            return values.Sum();
        }

        [Instrument, Log, Trace]
        public async Task<int> AddAsync(params int[] values)
        {
            return await Task.Run(() => Add(values));
        }
    }
}
