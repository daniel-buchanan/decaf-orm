using System;
using System.Data;
using pdq.db.common;

namespace pdq.npgsql
{
	public class NpgsqlOptions : IDatabaseOptions
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

		/// <inheritdoc/>
		public string CommentCharacter => Builders.Constants.Comment;
    }
}

