using System.Data;
using decaf.db.common;

namespace decaf.npgsql;

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
    public override INpgsqlOptionsBuilder WithConnectionString(string connectionString)
        => ConfigureProperty(
            nameof(NpgsqlOptions.ConnectionDetails),
            NpgsqlConnectionDetails.FromConnectionString(connectionString));
}