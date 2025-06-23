using System.Data;
using decaf.db.common;

namespace decaf.npgsql;

public interface INpgsqlOptionsBuilder :
    ISqlOptionsBuilder<NpgsqlOptions, INpgsqlOptionsBuilder, INpgsqlConnectionDetails>
{
    /// <summary>
    /// Sets the transaction isolation level.<br/>
    /// The default is <see cref="IsolationLevel.ReadCommitted"/>.
    /// </summary>
    /// <param name="level">The transaction isolation level to use.</param>
    INpgsqlOptionsBuilder SetIsolationLevel(IsolationLevel level);

    /// <summary>
    /// Quote all identifiers when generating SQL.
    /// </summary>
    INpgsqlOptionsBuilder UseQuotedIdentifiers();
}