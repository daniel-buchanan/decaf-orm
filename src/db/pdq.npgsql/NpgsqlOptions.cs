using System;
using System.Data;

namespace pdq.npgsql
{
	public class NpgsqlOptions
	{
		public NpgsqlOptions()
		{
			TransactionIsolationLevel = IsolationLevel.ReadCommitted;
		}

		/// <summary>
		/// 
		/// </summary>
		public IsolationLevel TransactionIsolationLevel { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public bool QuotedIdentifiers { get; private set; }
	}
}

