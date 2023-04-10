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
        public IPdqOptionsBuilder EnableTransientTracking()
            => ConfigureProperty(nameof(PdqOptions.TrackTransients), true);

        /// <inheritdoc/>
        public IPdqOptionsBuilder OverrideDefaultClauseHandling(ClauseHandling handling)
            => ConfigureProperty(nameof(PdqOptions.DefaultClauseHandling), handling);

        /// <inheritdoc/>
        public IPdqOptionsBuilder OverrideDefaultLogLevel(LogLevel level)
            => ConfigureProperty(nameof(PdqOptions.DefaultLogLevel), level);

        /// <inheritdoc/>
        public IPdqOptionsBuilder CloseConnectionsOnCommitOrRollback()
            => ConfigureProperty(nameof(PdqOptions.CloseConnectionOnCommitOrRollback), true);

        /// <inheritdoc/>
        public IPdqOptionsBuilder DisableSqlHeaderComments()
            => ConfigureProperty(nameof(PdqOptions.IncludeHeaderCommentsInSql), false);

        /// <inheritdoc/>
        protected IPdqOptionsBuilder SetConnectionFactory<T>() where T : IConnectionFactory
            => ConfigureProperty(nameof(PdqOptions.ConnectionFactoryType), typeof(T));

        /// <inheritdoc/>
        protected IPdqOptionsBuilder SetLoggerProxy<T>() where T : ILoggerProxy
            => ConfigureProperty(nameof(PdqOptions.LoggerProxyType), typeof(T));

        /// <inheritdoc/>
        protected IPdqOptionsBuilder SetSqlFactory<T>() where T : ISqlFactory
            => ConfigureProperty(nameof(PdqOptions.SqlFactoryType), typeof(T));

        /// <inheritdoc/>
        protected IPdqOptionsBuilder SetTransactionFactory<T>() where T : ITransactionFactory
            => ConfigureProperty(nameof(PdqOptions.TransactionFactoryType), typeof(T));

        /// <inheritdoc/>
        public IPdqOptionsBuilder ConfigureDbImplementation<TSqlFactory, TConnectionFactory, TTransactionFactory>()
            where TSqlFactory : ISqlFactory
            where TConnectionFactory : IConnectionFactory
            where TTransactionFactory : ITransactionFactory
        {
            base.ConfigureProperty(nameof(PdqOptions.SqlFactoryType), typeof(TSqlFactory));
            base.ConfigureProperty(nameof(PdqOptions.ConnectionFactoryType), typeof(TConnectionFactory));
            base.ConfigureProperty(nameof(PdqOptions.TransactionFactoryType), typeof(TTransactionFactory));
            return this;
        }

        private new IPdqOptionsBuilder ConfigureProperty<TValue>(string name, TValue value)
        {
            base.ConfigureProperty(name, value);
            return this;
        }
    }
}

