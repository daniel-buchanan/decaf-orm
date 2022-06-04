using System;
using pdq.state;

namespace pdq
{
	public interface IJoinTo<T>
	{
		T Join(Table from, IWhere conditions, JoinType type);
		T Join(Table from, Action<IBuilder> query, string alias, JoinType type);
	}
}
