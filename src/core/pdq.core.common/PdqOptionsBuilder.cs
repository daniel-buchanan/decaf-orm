using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.common
{
    public class PdqOptionsBuilder : IPdqOptionsBuilderInternal
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public PdqOptionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <inheritdoc/>
        public PdqOptions Build()
        {
            var options = new PdqOptions();
            var optionsType = typeof(PdqOptions);
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var properties = optionsType.GetProperties(flags);

            foreach(var p in values)
            {
                var prop = properties.FirstOrDefault(op => op.Name == p.Key);
                if (prop == null) continue;
                prop.SetValue(options, p.Value);
            }

            return options;
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
        public void SetConnectionFactory<T>() where T : IConnectionFactory
            => ConfigureProperty(nameof(PdqOptions.ConnectionFactoryType), typeof(T));

        /// <inheritdoc/>
        public void SetLoggerProxy<T>() where T : ILoggerProxy
            => ConfigureProperty(nameof(PdqOptions.LoggerProxyType), typeof(T));

        /// <inheritdoc/>
        public void SetSqlFactory<T>() where T : ISqlFactory
            => ConfigureProperty(nameof(PdqOptions.SqlFactoryType), typeof(T));

        /// <inheritdoc/>
        public void SetTransactionFactory<T>() where T : ITransactionFactory
            => ConfigureProperty(nameof(PdqOptions.TransactionFactoryType), typeof(T));

        /// <inheritdoc/>
        private void ConfigureProperty<T>(string property, T value)
            => this.values.Add(property, value);
    }
}

