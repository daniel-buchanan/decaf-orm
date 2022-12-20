using System;
using System.Data;

namespace pdq.npgsql
{
	public interface INpgsqlOptionsBuilder
	{
		/// <summary>
		/// Sets the transaction isolation level.<br/>
		/// The default is <see cref="IsolationLevel.ReadCommitted"/>.
		/// </summary>
		/// <param name="level">The transaction isolation level to use.</param>
		void SetIsolationLevel(IsolationLevel level);

		/// <summary>
		/// Quote all identifiers when generating SQL.
		/// </summary>
		void UseQuotedIdentifiers();
	}
}

