using System;
using System.Collections.Generic;
using pdq.common;

namespace pdq
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
		void AddClause(state.IWhere item);

		IEnumerable<state.IWhere> GetClauses();

		ClauseHandling DefaultClauseHandling { get; }
	}
}

