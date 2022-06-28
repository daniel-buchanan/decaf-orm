using System;
using System.Collections.Generic;

namespace pdq
{
    public interface IWhereBuilder
	{
		IColumnWhereBuilder Column(string name, string targetAlias = null);

		IWhereBuilder And(Action<IWhereBuilder> builder);

		IWhereBuilder Or(Action<IWhereBuilder> builder);

		IClauseHandlingBehaviour ClauseHandling();

		internal void AddClause(state.IWhere item);

		internal IEnumerable<state.IWhere> GetClauses();
	}
}

