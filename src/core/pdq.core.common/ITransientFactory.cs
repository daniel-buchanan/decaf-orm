using System;
using System.Threading.Tasks;

namespace pdq.common
{
	public interface ITransientFactory : IDisposable
	{
		ITransient Create(IConnectionDetails connectionDetails);

		Task<ITransient> CreateAsync(IConnectionDetails connectionDetails);

		internal void NotifyTransientDisposed(Guid id);
	}
}

