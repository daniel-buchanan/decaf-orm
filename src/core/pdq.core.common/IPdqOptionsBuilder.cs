using System;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.common
{
    public interface IPdqOptionsBuilder
    {
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
        void OverrideDefaultLogLevel(LogLevel level);

        /// <summary>
        /// Override the default where clause handling behaviour (<see cref="ClauseHandling.And"/>), and
        /// set to on eof the available options.
        /// </summary>
        /// <param name="handling">
        /// The default where clause handling you want to use.
        /// </param>
        void OverrideDefaultClauseHandling(ClauseHandling handling);

        /// <summary>
        /// Enable tracking of Transients throughout their lifetime, by default
        /// they are not tracked, and unless you explicitly dispose of them they
        /// not be disposed of until the GC collects them. However if you enable
        /// tracking they are tracked by the Unit of Work allowing for debugging and
        /// discovery of issues.
        /// </summary>
        void EnableTransientTracking();
    }

    internal interface IPdqOptionsBuilderInternal : IPdqOptionsBuilder
    {
        void SetLoggerProxy<T>() where T: ILoggerProxy;

        void SetSqlFactory<T>() where T : ISqlFactory;

        void SetConnectionFactory<T>() where T : IConnectionFactory;

        void SetTransactionFactory<T>() where T : ITransactionFactory;

        PdqOptions Build();
    }
}

