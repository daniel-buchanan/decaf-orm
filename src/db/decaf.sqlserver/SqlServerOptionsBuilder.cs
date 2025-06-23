using System.Data;
using decaf.db.common;

namespace decaf.sqlserver;

public class SqlServerOptionsBuilder :
    SqlOptionsBuilder<SqlServerOptions, ISqlServerOptionsBuilder, ISqlServerConnectionDetails>,
    ISqlServerOptionsBuilder
{
    /// <inheritdoc/>
    public ISqlServerOptionsBuilder SetIsolationLevel(IsolationLevel level)
        => ConfigureProperty(nameof(SqlServerOptions.TransactionIsolationLevel), level);

    /// <inheritdoc/>
    public ISqlServerOptionsBuilder UseQuotedIdentifiers()
        => ConfigureProperty(nameof(SqlServerOptions.QuotedIdentifiers), true);

    /// <inheritdoc/>
    public override ISqlServerOptionsBuilder WithConnectionString(string connectionString)
        => ConfigureProperty(
            nameof(SqlServerOptions.ConnectionDetails),
            SqlServerConnectionDetails.FromConnectionString(connectionString));

    /// <inheritdoc/>
    private new ISqlServerOptionsBuilder ConfigureProperty<TValue>(string name, TValue value)
    {
        base.ConfigureProperty(name, value);
        return this;
    }
}