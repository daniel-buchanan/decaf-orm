using System;

namespace pdq
{
	public interface ISelectFrom :
		ISelectColumn<ISelectFrom>,
		IJoinTo<ISelectFrom>,
		IDisposable
	{
		IWhere Where(Action<IWhereBuilder> builder);
	}
}