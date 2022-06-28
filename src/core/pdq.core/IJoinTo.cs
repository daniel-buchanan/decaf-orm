using System;
using pdq.common;
using pdq.state;

namespace pdq
{
	public interface IJoinTo<T>
	{
		T Join(IQueryTarget from, state.IWhere conditions, IQueryTarget to, JoinType type = JoinType.Default);

		T Join(IQueryTarget from, state.IWhere conditions, Action<ISelectWithAlias> query, JoinType type = JoinType.Default);
	}
}
