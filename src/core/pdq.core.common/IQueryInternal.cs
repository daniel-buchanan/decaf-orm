using System;

[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("pdq.core")]
namespace pdq.core.common
{
	public interface IQueryInternal : IQuery
	{
		internal void SetContext(IQueryContext context);

		internal T GetFluent<T>();
	}
}

