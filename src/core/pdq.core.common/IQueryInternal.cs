using System;
namespace pdq.core.common
{
	internal interface IQueryInternal : IQuery
	{
		internal void SetContext(IQueryContext context);

		internal T GetFluent<T>();
	}
}

