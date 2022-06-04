using System;
namespace pdq.common
{
	public interface IUnitOfWork
	{
		ITransient Begin();
	}
}

