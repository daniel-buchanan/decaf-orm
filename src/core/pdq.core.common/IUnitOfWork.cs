using System;
namespace pdq.core.common
{
	public interface IUnitOfWork
	{
		ITransient Begin();
	}
}

