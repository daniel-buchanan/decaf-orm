using System;

namespace pdq
{
	public interface ISelectFrom :
		ISelectColumn<ISelectFrom>,
		IJoin<ISelectFrom>,
		IDisposable
	{
		IWhere Where(Action<IWhereBuilder> builder);
	}
}