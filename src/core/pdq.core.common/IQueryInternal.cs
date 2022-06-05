using System;

[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("pdq")]
namespace pdq.common
{
	public interface IQueryInternal : IQuery
	{
		internal string GetHash();

		internal IAliasManager AliasManager { get; }
	}
}