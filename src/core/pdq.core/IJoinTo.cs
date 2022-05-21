using System;
using pdq.core.state;

namespace pdq.core
{
	public interface IJoinTo<T>
	{
		T Join(Table from, IWhere conditions, JoinType type);
		T Join(Table from, Action<IBuilder> query, string alias, JoinType type);
	}
}
