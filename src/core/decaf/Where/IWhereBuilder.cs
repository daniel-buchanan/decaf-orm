using System;
using System.Collections.Generic;
using decaf.common;

namespace decaf
{
    public interface IWhereBuilder
	{
		IColumnWhereBuilder Column(string name, string targetAlias = null);

		IWhereBuilder And(Action<IWhereBuilder> builder);

		IWhereBuilder Or(Action<IWhereBuilder> builder);

		IClauseHandlingBehaviour ClauseHandling { get; }
	}

	internal interface IWhereBuilderInternal : IWhereBuilder
    {
		void AddClause(IWhere item);

		IEnumerable<IWhere> GetClauses();

		ClauseHandling DefaultClauseHandling { get; }
	}
}

