using System;
using pdq.common;
using pdq.state;

namespace pdq
{
	public interface IDeleteFrom : IBuilder, IExecute
	{
		IDeleteFrom Where(Action<IWhereBuilder> builder);
	}
}

