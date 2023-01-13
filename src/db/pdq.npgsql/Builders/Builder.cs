using System;
namespace pdq.npgsql.Builders
{
	public abstract class Builder
	{
        private readonly string quote = string.Empty;

        protected Builder(NpgsqlOptions options)
		{
            if (options.QuotedIdentifiers)
                this.quote = "\"";
        }
	}
}

