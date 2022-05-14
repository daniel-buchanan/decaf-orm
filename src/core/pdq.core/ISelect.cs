using System;

namespace pdq.core
{
	public interface ISelect
	{
		ISelectFrom From(string table, string alias, string? schema = null);
		ISelectFrom From(Action<IBuilder> query, string alias);
	}
}

