using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace pdq.services
{
    public interface IQuery : IService, IExecutionNotifiable { }

    /// <summary>
    /// Service for querying a given Entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of <see cref="IEntity"/> to query.</typeparam>
	public interface IQuery<TEntity> : IQuery
		where TEntity : IEntity
	{
		/// <summary>
        /// Get all known items of this type.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{TEntity}"/> set of the results.</returns>
		IEnumerable<TEntity> All();

		/// <summary>
		/// Get all known items of this type.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An <see cref="IEnumerable{TEntity}"/> set of the results.</returns>
		Task<IEnumerable<TEntity>> AllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Find items using a query.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{Func{TEntity, bool}}"/> defining the query to run.</param>
        /// <returns>An <see cref="IEnumerable{TEntity}"/> set of the results.</returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Find items using a query.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{Func{TEntity, bool}}"/> defining the query to run.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="IEnumerable{TEntity}"/> set of the results.</returns>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
	}
}

