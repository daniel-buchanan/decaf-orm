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
        /// Begin a Query, using injected connection details (async)
        /// </summary>
        /// <returns>An <see cref="IQueryContainer"/> ready to be used.</returns>
        Task<IQueryContainer> BeginQueryAsync();

		/// <summary>
		/// Begin a Query, providing connection details
		/// </summary>
		/// <param name="connectionDetails">The <see cref="IConnectionDetails"/> to use.</param>
		/// <returns>An <see cref="IQueryContainer"/> ready to be used.</returns>
		IQueryContainer BeginQuery(IConnectionDetails connectionDetails);

        /// <summary>
        /// Begin a Query, providing connection details (async)
        /// </summary>
        /// <param name="connectionDetails">The connection details to use.</param>
        /// <returns>A query ready to be used.</returns>
        Task<IQueryContainer> BeginQueryAsync(IConnectionDetails connectionDetails);

		/// <summary>
		/// Begin a transient allowing you to execute multiple queries.
		/// </summary>
		/// <returns>An <see cref="ITransient"/>.</returns>
		ITransient Begin();

        /// <summary>
        /// Begin a transient allowing you to execute multiple queries.
        /// </summary>
        /// <param name="connectionDetails">The <see cref="IConnectionDetails"/> to use.</param>
        /// <returns>An <see cref="ITransient"/>.</returns>
        ITransient Begin(IConnectionDetails connectionDetails);

        /// <summary>
		/// Begin a transient allowing you to execute multiple queries. (async)
		/// </summary>
		/// <returns>An <see cref="ITransient"/>.</returns>
		Task<ITransient> BeginAsync();

        /// <summary>
        /// Begin a transient allowing you to execute multiple queries. (async)
        /// </summary>
        /// <param name="connectionDetails">The <see cref="IConnectionDetails"/> to use.</param>
        /// <returns>An <see cref="ITransient"/>.</returns>
		Task<ITransient> BeginAsync(IConnectionDetails connectionDetails);
	}
}

