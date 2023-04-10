using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Connections;
using pdq.common.Logging;
using pdq.common.Options;

namespace pdq.common
{
    public class PdqOptionsBuilder :
        OptionsBuilder<PdqOptions>,
        IPdqOptionsBuilder
    {
        public PdqOptionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <inheritdoc/>
        public IServiceCollection Services { get; private set; }

        /// <inheritdoc/>
        public void EnableTransientTracking()
            => ConfigureProperty(nameof(PdqOptions.TrackTransients), true);

        /// <inheritdoc/>
        public void OverrideDefaultClauseHandling(ClauseHandling handling)
            => ConfigureProperty(nameof(PdqOptions.DefaultClauseHandling), handling);

        /// <inheritdoc/>
        public void OverrideDefaultLogLevel(LogLevel level)
            => ConfigureProperty(nameof(PdqOptions.DefaultLogLevel), level);

        /// <inheritdoc/>
        public void CloseConnectionsOnCommitOrRollback()
            => ConfigureProperty(nameof(PdqOptions.CloseConnectionOnCommitOrRollback), true);

        /// <inheritdoc/>
        public void DisableSqlHeaderComments()
            => ConfigureProperty(nameof(PdqOptions.IncludeHeaderCommentsInSql), false);

        /// <inheritdoc/>
        protected void SetConnectionFactory<T>() where T : IConnectionFactory
            => ConfigureProperty(nameof(PdqOptions.ConnectionFactoryType), typeof(T));

        /// <inheritdoc/>
        protected void SetLoggerProxy<T>() where T : ILoggerProxy
            => ConfigureProperty(nameof(PdqOptions.LoggerProxyType), typeof(T));

        /// <inheritdoc/>
        protected void SetSqlFactory<T>() where T : ISqlFactory
            => ConfigureProperty(nameof(PdqOptions.SqlFactoryType), typeof(T));

        /// <inheritdoc/>
        protected void SetTransactionFactory<T>() where T : ITransactionFactory
            => ConfigureProperty(nameof(PdqOptions.TransactionFactoryType), typeof(T));

        /// <inheritdoc/>
        public void ConfigureDbImplementation<TSqlFactory, TConnectionFactory, TTransactionFactory>()
            where TSqlFactory : ISqlFactory
            where TConnectionFactory : IConnectionFactory
            where TTransactionFactory : ITransactionFactory
        {
            ConfigureProperty(nameof(PdqOptions.SqlFactoryType), typeof(TSqlFactory));
            ConfigureProperty(nameof(PdqOptions.ConnectionFactoryType), typeof(TConnectionFactory));
            ConfigureProperty(nameof(PdqOptions.TransactionFactoryType), typeof(TTransactionFactory));
        }
    }
}

