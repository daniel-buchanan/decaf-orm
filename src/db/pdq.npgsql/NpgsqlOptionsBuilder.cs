using System.Data;
using pdq.db.common;

namespace pdq.npgsql
{
	public class NpgsqlOptionsBuilder
		: OptionsBuilder<NpgsqlOptions>,
		INpgsqlOptionsBuilder
	{
		/// <inheritdoc/>
		public void SetIsolationLevel(IsolationLevel level)
			=> ConfigureProperty(nameof(NpgsqlOptions.TransactionIsolationLevel), level);

		/// <inheritdoc/>
		public void UseQuotedIdentifiers()
			=> ConfigureProperty(nameof(NpgsqlOptions.QuotedIdentifiers), true);
    }
}

