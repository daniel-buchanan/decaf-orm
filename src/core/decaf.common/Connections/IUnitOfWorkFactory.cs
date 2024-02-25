using System;
using System.Threading;
using System.Threading.Tasks;

namespace decaf.common.Connections
{
	public interface IUnitOfWorkFactory : IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionDetails"></param>
		/// <returns></returns>
		IUnitOfWork Create(IConnectionDetails connectionDetails);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionDetails"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<IUnitOfWork> CreateAsync(IConnectionDetails connectionDetails, CancellationToken cancellationToken = default);
	}

	internal interface IUnitOfWorkFactoryInternal : IUnitOfWorkFactory
    {
		/// <summary>
		/// Notify the transient factory that the transient has been disposed.
		/// </summary>
		/// <param name="id">The ID of the transient that was disposed.</param>
		void NotifyUnitOfWorkDisposed(Guid id);
	}
}

