using System;

namespace pdq
{
	public interface ISelect : IDisposable
	{
		ISelectFrom From(string table, string alias, string schema = null);
		ISelectFrom From(Action<ISelect> query, string alias);
	}

	public interface ISelectWithAlias : ISelect
    {
		string Alias { get; }

		ISelectWithAlias KnownAs(string alias);
    }
}

