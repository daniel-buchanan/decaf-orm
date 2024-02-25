using System;
using decaf.common;
using decaf.common.Templates;
using decaf.state;

namespace decaf.db.common.Builders
{
	public interface IWhereBuilder
	{
		void AddWhere(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager);
		void AddJoin(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager);
	}
}

