using System;
namespace pdq.core
{
	public interface IUnitOfWork
	{
		ITransient Begin();
	}
}

