using System;
using pdq.common;
using pdq.state;

namespace pdq.db.common.Builders
{
	public interface IWhereBuilder
	{
		void AddWhere(IWhere whereClause, ISqlBuilder sqlBuilder);
	}

	public interface IWhereBuilder<T>
		where T: IQueryContext
	{
		void AddWhere(T context, ISqlBuilder sqlBuilder);

		IWhereBuilder Builder { get; }
	}
}

