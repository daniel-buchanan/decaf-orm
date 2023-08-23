using System;
using System.Threading;
using System.Threading.Tasks;

namespace pdq.common.Connections
{
	public interface ITransientFactory : IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionDetails"></param>
		/// <returns></returns>
		ITransient Create(IConnectionDetails connectionDetails);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionDetails"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<ITransient> CreateAsync(IConnectionDetails connectionDetails, CancellationToken cancellationToken = default);
	}

	internal interface ITransientFactoryInternal : ITransientFactory
    {
		/// <summary>
		/// Notify the transient factory that the transient has been disposed.
		/// </summary>
		/// <param name="id">The ID of the transient that was disposed.</param>
		void NotifyTransientDisposed(Guid id);
	}
}

