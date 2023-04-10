using System;
using System.Data;
using pdq.db.common;

namespace pdq.npgsql
{
	public interface INpgsqlOptionsBuilder :
        ISqlOptionsBuilder<NpgsqlOptions, INpgsqlOptionsBuilder>
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
}

