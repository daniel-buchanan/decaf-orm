using System;
using pdq.common.Logging;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.playground")]
namespace pdq
{
	/// <summary>
    /// Set of options for configuring pdq.
    /// </summary>
	public class PdqOptions
	{
		/// <summary>
        /// The default log level to use, this is set to <see cref="LogLevel.Error"/>
        /// by default, unless changed.
        /// </summary>
		public LogLevel DefaultLogLevel { get; private set; } = LogLevel.Error;

		/// <summary>
        /// Whether or not to track transients as they are used, and disposed.
        /// </summary>
		public bool TrackTransients { get; private set; } = false;

		/// <summary>
        /// The type of the logger proxy to use.
        /// </summary>
		internal Type LoggerProxyType { get; private set; } = typeof(DefaultLogger);

		/// <summary>
        /// The type of the sql factory to use.
        /// </summary>
		internal Type SqlFactoryType { get; private set; }

        /// <summary>
        /// The type of connection factory to use.
        /// </summary>
        internal Type ConnectionFactoryType { get; set; }

        /// <summary>
        /// The type of transaction factory to use.
        /// </summary>
        internal Type TransactionFactoryType { get; set; }

        /// <summary>
        /// Override the default log level (<see cref="LogLevel.Error"/>), and set
        /// to one of the available options.
        /// </summary>
        /// <param name="level">
        /// The log level that you want to set, you should not require a lower
        /// log level than "<see cref="LogLevel.Error"/>" for most use cases,
        /// although "<see cref="LogLevel.Information"/>" may also
        /// be useful.
        /// </param>
        public void OverrideDefaultLogLevel(LogLevel level) => DefaultLogLevel = level;

		/// <summary>
        /// Enable tracking of Transients throughout their lifetime, by default
        /// they are not tracked, and unless you explicitly dispose of them they
        /// not be disposed of until the GC collects them. However if you enable
        /// tracking they are tracked by the Unit of Work allowing for debugging and
        /// discovery of issues.
        /// </summary>
		public void EnableTransientTracking() => TrackTransients = true;
	}
}

