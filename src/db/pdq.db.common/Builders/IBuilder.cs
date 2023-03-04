using System;
using System.Collections.Generic;
using pdq.common;
using pdq.common.Templates;

namespace pdq.db.common.Builders
{
	public interface IBuilder<T>
		where T: IQueryContext
	{
		SqlTemplate Build(T context);

		SqlTemplate Build(T context, ISqlBuilder sqlBuilder);

		Dictionary<string, object> GetParameters(T context);
	}
}

