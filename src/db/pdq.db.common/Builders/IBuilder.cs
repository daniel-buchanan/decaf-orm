using System;
using pdq.common;

namespace pdq.db.common.Builders
{
	public interface IBuilder<T>
		where T: IQueryContext
	{
		SqlTemplate Build(T context);
	}
}

