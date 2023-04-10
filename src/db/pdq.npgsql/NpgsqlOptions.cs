using System.Data;
using pdq.common.Connections;
using pdq.db.common;

namespace pdq.npgsql
{
    public class NpgsqlOptions : IDatabaseOptions
	{
		public NpgsqlOptions()
		{
			TransactionIsolationLevel = IsolationLevel.ReadCommitted;
			QuotedIdentifiers = false;
		}

		/// <summary>
		/// The transaction isolation level, <see cref="IsolationLevel"/>.
		/// </summary>
		public IsolationLevel TransactionIsolationLevel { get; private set; }

		/// <summary>
		/// Determines whether or not to use quoted identifiers.
		/// </summary>
		public bool QuotedIdentifiers { get; private set; }

		/// <inheritdoc/>
		public string CommentCharacter => Builders.Constants.Comment;

        /// <inheritdoc/>
        public IConnectionDetails ConnectionDetails { get; private set; }
    }
}

