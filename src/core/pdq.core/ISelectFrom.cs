using System;

namespace pdq
{
	public interface ISelectFrom :
		ISelectColumn<ISelectFrom>,
		IJoinTo<ISelectFrom>,
		IDisposable
	{
		ISelectFrom Where(state.IWhere where);
	}
}