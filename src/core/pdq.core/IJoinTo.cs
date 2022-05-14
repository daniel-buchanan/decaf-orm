using System;
namespace pdq.core
{
	public interface IJoinTo<T>
	{
		T Join(JoinProp from, JoinProp to, JoinType type);
		T Join(JoinProp from, Action<IBuilder> query, string alias, JoinType type);
	}
}

