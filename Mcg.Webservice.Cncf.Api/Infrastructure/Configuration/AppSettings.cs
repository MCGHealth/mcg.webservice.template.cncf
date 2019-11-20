using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Mcg.Webservice.Cncf.Api.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Serilog.Events;

namespace Mcg.Webservice.Cncf.Api.Infrastructure.Configuration
{
    /// <summary>
    /// Allows access to the environmental vars that are used to configure the app.
    /// </summary> 
    public sealed class AppSettings : IAppSettings
    {
        private const string LOG_LEVEL = nameof(LOG_LEVEL);
        private const string ASPNETCORE_ENVIRONMENT = nameof(ASPNETCORE_ENVIRONMENT);
        private const string DEV_ENVIRONMENT = "Development";

        public static string ServiceName => typeof(AppSettings).Assembly.GetName().Name.SafeString();

        /// <summary>
        /// The dictionary that contains the configuration values read from the environmental variables.
        /// </summary>
        private IDictionary<string, string> configData { get; set; }

        /// <summary>
        /// Returns a copy of the raw configuration data.
        /// </summary>
        public IDictionary<string, string> RawConfiguration
        {
            get
            {
                IDictionary<string, string> clone = new Dictionary<string, string>();

                foreach (var kp in configData)
                {
                    clone.Add(kp.Key, kp.Value);
                }

                clone.Add("IS_DEV_ENVIRONMENT", IsDevEnvironment.ToString());
                clone.Add("SERVICE_NAME", ServiceName);
                clone.Add("HOST_NAME", Environment.MachineName);
                clone.Add("OS_PLATFORM", Environment.OSVersion.Platform.ToString());
                clone.Add("OS_VERSION", Environment.OSVersion.Version.ToString());

                return clone;
            }
        }

        /// <summary>
        /// The currently configured <see cref="LogEventLevel"/> for the application.
        /// </summary>
        public LogEventLevel LogLevel { get; internal set; }

        /// <summary>
        /// Indicates if the current runtime environment is Development or not.
        /// </summary>
        public bool IsDevEnvironment { get; internal set; }

        /// <summary>
        /// Default ctor.
        /// </summary>
        public AppSettings(IConfiguration config) : this(config.AsEnumerable().ToDictionary(k => k.Key, v => v.Value)) { }

        internal AppSettings(IDictionary<string, string> configData)
        {

            this.configData = configData ?? throw new ArgumentNullException(nameof(configData));
            LogLevel = this[LOG_LEVEL].ToEnum<LogEventLevel>().newValue;
            IsDevEnvironment = this[ASPNETCORE_ENVIRONMENT].Equals(DEV_ENVIRONMENT, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Returns the newValue associated with the key newValue.
        /// </summary>
        /// <param name="key">The key to be returned.</param>
        /// <exception cref="ConfigurationErrorsException">Thrown if the requested key does not exist.</exception>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (!configData.ContainsKey(key))
                {
                    throw new ConfigurationErrorsException($"Configuration key '{key}' does not exist.");
                }

                return configData[key];
            }
        }


        [Log]
        public void Update(string key, string newValue)
        {
            if (key == LOG_LEVEL)
            {
                var results = newValue.ToEnum<LogEventLevel>();

                if (!results.success)
                {
                    throw new InvalidCastException($"The value {newValue} is not a valid SeverityLevel.");
                }

                LogLevel = results.newValue;
            }
        }
    }
}
