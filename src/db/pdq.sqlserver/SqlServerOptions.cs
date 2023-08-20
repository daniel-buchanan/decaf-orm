using System.Data;
using pdq.db.common;

namespace pdq.sqlserver
{
    public class SqlServerOptions : DatabaseOptions
	{
		public SqlServerOptions()
		{
			TransactionIsolationLevel = IsolationLevel.ReadCommitted;
		}

		/// <summary>
		/// The transaction isolation level, <see cref="IsolationLevel"/>.
		/// </summary>
		public IsolationLevel TransactionIsolationLevel { get; private set; }
    }
}

