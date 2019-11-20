using System.Collections.Generic;
using Serilog.Events;

namespace Mcg.Webservice.Cncf.Api.Infrastructure.Configuration
{
    /// <summary>
    /// Allows access to the environmental vars that are used to configure the app.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// Returns the value associated with the key value.
        /// </summary>
        /// <param name="key">The key to be returned.</param>
        string this[string key] { get; }

        

        /// <summary>
        /// Returns a copy of the raw configuration data.
        /// </summary>
        IDictionary<string, string> RawConfiguration { get; }

        /// <summary>
        /// The currently configured <see cref="Serilog.Events.LogEventLevel"/> for the application.
        /// </summary>
        LogEventLevel LogLevel { get; }

        /// <summary>
        /// Indicates if the current environment is Development or not.
        /// </summary>
        bool IsDevEnvironment { get; }

        /// <summary>
        /// Returns the configuration as a string.
        /// </summary>
        /// <returns></returns>
        string ToString();

        /// <summary>
        /// Permits a configuration setting to be updated at runtime.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Update(string key, string value);
    }
}