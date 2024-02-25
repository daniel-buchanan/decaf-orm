using System.Data;
using decaf.db.common;

namespace decaf.sqlserver
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

