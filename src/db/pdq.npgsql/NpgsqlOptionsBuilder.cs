using System.Data;
using pdq.common.Connections;
using pdq.db.common;

namespace pdq.npgsql
{
    public class NpgsqlOptionsBuilder :
        SqlOptionsBuilder<NpgsqlOptions, INpgsqlOptionsBuilder, INpgsqlConnectionDetails>,
		INpgsqlOptionsBuilder
	{
        /// <inheritdoc/>
        public INpgsqlOptionsBuilder SetIsolationLevel(IsolationLevel level)
            => ConfigureProperty(nameof(NpgsqlOptions.TransactionIsolationLevel), level);

        /// <inheritdoc/>
        public INpgsqlOptionsBuilder UseQuotedIdentifiers()
			=> ConfigureProperty(nameof(NpgsqlOptions.QuotedIdentifiers), true);

        /// <inheritdoc/>
        public INpgsqlOptionsBuilder WithConnectionDetails(IConnectionDetails connectionDetails)
            => ConfigureProperty(nameof(NpgsqlOptions.ConnectionDetails), connectionDetails);

        /// <inheritdoc/>
        public override INpgsqlOptionsBuilder WithConnectionString(string connectionString)
            => ConfigureProperty(
                nameof(NpgsqlOptions.ConnectionDetails),
                NpgsqlConnectionDetails.FromConnectionString(connectionString));
    }
}

