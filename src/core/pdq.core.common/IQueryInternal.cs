using System;

[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("pdq")]
namespace pdq.common
{
	internal interface IQueryInternal : IQuery
	{
		string GetHash();

		IAliasManager AliasManager { get; }

		ITransient Transient { get; }

		IQueryContext Context { get; }

		void SetContext(IQueryContext context);

		PdqOptions Options { get; }
    }
}