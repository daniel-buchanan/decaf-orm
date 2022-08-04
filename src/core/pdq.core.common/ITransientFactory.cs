using System;
using System.Threading.Tasks;

namespace pdq.common
{
	public interface ITransientFactory : IDisposable
	{
		ITransient Create(IConnectionDetails connectionDetails);

		Task<ITransient> CreateAsync(IConnectionDetails connectionDetails);
	}

	internal interface ITransientFactoryInternal : ITransientFactory
    {
		void NotifyTransientDisposed(Guid id);
	}
}

