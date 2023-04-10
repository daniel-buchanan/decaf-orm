using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.common.Options
{
    public interface IPdqOptionsBuilder : IOptionsBuilder<PdqOptions>
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

        /// <summary>
        /// Close database connections after a commit or rollback. This will mean
        /// that any time a query is made it will first have to create and open
        /// a new connection to the database. If not enabled (default) then
        /// connections will be re-used for multiple queries across a transaction.
        /// </summary>
        void CloseConnectionsOnCommitOrRollback();

        /// <summary>
        /// Disables header comments for queries generated.
        /// This means primarily that the query hash and timestamp will not
        /// be included as comments at the beginning of the query.
        /// </summary>
        void DisableSqlHeaderComments();

        /// <summary>
        /// The Services Collection.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Configure a specific Database implementation with its factories.
        /// </summary>
        /// <typeparam name="TSqlFactory">The type of the SQL Factory for this DB Implementation.</typeparam>
        /// <typeparam name="TConnectionFactory">The type of the Connection Factory for this DB Implementation.</typeparam>
        /// <typeparam name="TTransactionFactory">The type of the Transaction Factory for this DB Implementation.</typeparam>
        void ConfigureDbImplementation<TSqlFactory, TConnectionFactory, TTransactionFactory>()
            where TSqlFactory: ISqlFactory
            where TConnectionFactory: IConnectionFactory
            where TTransactionFactory: ITransactionFactory;
    }
}

