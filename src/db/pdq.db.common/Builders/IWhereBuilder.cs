using System;
using pdq.common;
using pdq.common.Templates;
using pdq.state;

namespace pdq.db.common.Builders
{
	public interface IWhereBuilder
	{
		void AddWhere(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager);
		void AddJoin(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager);
	}
}

