using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pdq.common.Connections;
using pdq.common.Logging;

namespace pdq.common
{
    public class PdqOptionsBuilder : IPdqOptionsBuilderInternal
    {
        private readonly Dictionary<string, object> values;

        public PdqOptionsBuilder()
        {
            values = new Dictionary<string, object>();
        }

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

        public void EnableTransientTracking()
            => ConfigureProperty(nameof(PdqOptions.TrackTransients), false);

        public void OverrideDefaultClauseHandling(ClauseHandling handling)
            => ConfigureProperty(nameof(PdqOptions.DefaultClauseHandling), handling);

        public void OverrideDefaultLogLevel(LogLevel level)
            => ConfigureProperty(nameof(PdqOptions.DefaultLogLevel), level);

        public void SetConnectionFactory<T>() where T : IConnectionFactory
            => ConfigureProperty(nameof(PdqOptions.ConnectionFactoryType), typeof(T));

        public void SetLoggerProxy<T>() where T : ILoggerProxy
            => ConfigureProperty(nameof(PdqOptions.LoggerProxyType), typeof(T));

        public void SetSqlFactory<T>() where T : ISqlFactory
            => ConfigureProperty(nameof(PdqOptions.SqlFactoryType), typeof(T));

        public void SetTransactionFactory<T>() where T : ITransactionFactory
            => ConfigureProperty(nameof(PdqOptions.TransactionFactoryType), typeof(T));

        private void ConfigureProperty<T>(string property, T value)
        {
            this.values.Add(property, value);
        }
    }
}

