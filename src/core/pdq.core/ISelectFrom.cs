using System;

namespace pdq
{
	public interface ISelectFrom :
		ISelectColumn<ISelectFrom>,
		IJoin<ISelectFrom>
	{
		IOrderBy Where(Action<IWhereBuilder> builder);
	}
}