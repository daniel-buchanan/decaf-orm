using System.Data;
using pdq.db.common;

namespace pdq.sqlserver
{
    public interface ISqlServerOptionsBuilder :
        ISqlOptionsBuilder<SqlServerOptions, ISqlServerOptionsBuilder, ISqlServerConnectionDetails>
    {
        /// <summary>
        /// Sets the transaction isolation level.<br/>
        /// The default is <see cref="IsolationLevel.ReadCommitted"/>.
        /// </summary>
        /// <param name="level">The transaction isolation level to use.</param>
        ISqlServerOptionsBuilder SetIsolationLevel(IsolationLevel level);

        /// <summary>
        /// Quote all identifiers when generating SQL.
        /// </summary>
        ISqlServerOptionsBuilder UseQuotedIdentifiers();
    }
}

