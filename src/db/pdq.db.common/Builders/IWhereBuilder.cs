using System;
using pdq.common;
using pdq.common.Templates;
using pdq.state;

namespace pdq.db.common.Builders
{
	public interface IWhereBuilder
	{
		void AddWhere(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager);
	}

	public interface IWhereBuilder<T>
		where T: IQueryContext
	{
		void AddWhere(T context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

		IWhereBuilder Builder { get; }
	}
}

