using System.Data;
using pdq.common.Connections;
using pdq.db.common;

namespace pdq.sqlserver
{
    public class SqlServerOptions : IDatabaseOptions
	{
		public SqlServerOptions()
		{
			TransactionIsolationLevel = IsolationLevel.ReadCommitted;
			QuotedIdentifiers = false;
		}

		/// <summary>
		/// The transaction isolation level, <see cref="IsolationLevel"/>.
		/// </summary>
		public IsolationLevel TransactionIsolationLevel { get; private set; }

		/// <inheritdoc/>
		public bool QuotedIdentifiers { get; private set; }

        /// <inheritdoc/>
        public IConnectionDetails ConnectionDetails { get; private set; }
    }
}

