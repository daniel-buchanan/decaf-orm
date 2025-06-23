using System.Data;
using decaf.db.common;

namespace decaf.npgsql;

public class NpgsqlOptions : DatabaseOptions
{
	public NpgsqlOptions()
	{
		TransactionIsolationLevel = IsolationLevel.ReadCommitted;
	}

	/// <summary>
	/// The transaction isolation level, <see cref="IsolationLevel"/>.
	/// </summary>
	public IsolationLevel TransactionIsolationLevel { get; private set; }
}