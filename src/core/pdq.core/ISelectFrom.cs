using System;

namespace pdq
{
	public interface ISelectFrom :
		IJoin<ISelectFrom>
	{
		IOrderBy Where(Action<IWhereBuilder> builder);
	}
}