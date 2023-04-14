using System.Data;
using pdq.common.Connections;
using pdq.common.Options;

namespace pdq.sqlserver
{
    public class SqlServerOptionsBuilder :
        OptionsBuilder<SqlServerOptions>,
		ISqlServerOptionsBuilder
	{
        /// <inheritdoc/>
        public ISqlServerOptionsBuilder SetIsolationLevel(IsolationLevel level)
            => ConfigureProperty(nameof(SqlServerOptions.TransactionIsolationLevel), level);

        /// <inheritdoc/>
        public ISqlServerOptionsBuilder UseQuotedIdentifiers()
			=> ConfigureProperty(nameof(SqlServerOptions.QuotedIdentifiers), true);

        /// <inheritdoc/>
        public ISqlServerOptionsBuilder WithConnectionDetails(IConnectionDetails connectionDetails)
            => ConfigureProperty(nameof(SqlServerOptions.ConnectionDetails), connectionDetails);

        /// <inheritdoc/>
        private new ISqlServerOptionsBuilder ConfigureProperty<TValue>(string name, TValue value)
        {
            base.ConfigureProperty(name, value);
            return this;
        }
    }
}

