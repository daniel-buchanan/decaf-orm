using System.Threading;
using System.Threading.Tasks;
using pdq.common.Connections;

namespace pdq.common
{
	public interface IPdq
	{
        /// <summary>
        /// Begin a Query, using injected connection details
        /// </summary>
        /// <returns>An <see cref="IQueryContainer"/> ready to be used.</returns>
        IQueryContainer BeginQuery();

        /// <summary>
		/// Begin a Query, providing connection details
		/// </summary>
		/// <param name="connectionDetails">The <see cref="IConnectionDetails"/> to use.</param>
		/// <returns>An <see cref="IQueryContainer"/> ready to be used.</returns>
		IQueryContainer BeginQuery(IConnectionDetails connectionDetails);

        /// <summary>
        /// Begin a Query, using injected connection details (async)
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to use (optional).</param>
        /// <returns>An <see cref="IQueryContainer"/> ready to be used.</returns>
        Task<IQueryContainer> BeginQueryAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begin a Query, providing connection details (async)
        /// </summary>
        /// <param name="connectionDetails">The connection details to use.</param>
        /// <param name="cancellationToken">The cancellation token to use (optional).</param>
        /// <returns>A query ready to be used.</returns>
        Task<IQueryContainer> BeginQueryAsync(IConnectionDetails connectionDetails, CancellationToken cancellationToken = default);

		/// <summary>
		/// Begin a transient allowing you to execute multiple queries.
		/// </summary>
		/// <returns>An <see cref="IUnitOfWork"/>.</returns>
		IUnitOfWork Begin();

        /// <summary>
        /// Begin a transient allowing you to execute multiple queries.
        /// </summary>
        /// <param name="connectionDetails">The <see cref="IConnectionDetails"/> to use.</param>
        /// <returns>An <see cref="IUnitOfWork"/>.</returns>
        IUnitOfWork Begin(IConnectionDetails connectionDetails);

        /// <summary>
		/// Begin a transient allowing you to execute multiple queries. (async)
		/// </summary>
        /// <param name="cancellationToken">The cancellation token to use (optional).</param>
		/// <returns>An <see cref="IUnitOfWork"/>.</returns>
		Task<IUnitOfWork> BeginAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begin a transient allowing you to execute multiple queries. (async)
        /// </summary>
        /// <param name="connectionDetails">The <see cref="IConnectionDetails"/> to use.</param>
        /// <param name="cancellationToken">The cancellation token to use (optional).</param>
        /// <returns>An <see cref="IUnitOfWork"/>.</returns>
		Task<IUnitOfWork> BeginAsync(IConnectionDetails connectionDetails, CancellationToken cancellationToken = default);
	}
}

