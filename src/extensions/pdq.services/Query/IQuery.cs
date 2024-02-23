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

        /// <summary>
        /// Fetch a single item matching the predicate, if there is more than one throw an exception.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{Func{TEntity, bool}}"/> defining the query to run.</param>
        /// <returns>An <see cref="TEntity"/> which is found</returns>
        TEntity Single(Expression<Func<TEntity, bool>> expression);
        
        /// <summary>
        /// Fetch a single item matching the predicate, if there is more than one throw an exception.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{Func{TEntity, bool}}"/> defining the query to run.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="TEntity"/> which is found</returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        /// <summary>
        /// Fetch a single item matching the predicate, if none are found return default.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{Func{TEntity, bool}}"/> defining the query to run.</param>
        /// <returns>An <see cref="TEntity"/> which is found</returns>
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> expression);
        
        /// <summary>
        /// Fetch a single item matching the predicate, if none are found return default.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{Func{TEntity, bool}}"/> defining the query to run.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="TEntity"/> which is found</returns>
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        /// <summary>
        /// Fetch the first item matching the predicate, if none are found throw an exception.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{Func{TEntity, bool}}"/> defining the query to run.</param>
        /// <returns>An <see cref="TEntity"/> which is found</returns>
        TEntity First(Expression<Func<TEntity, bool>> expression);
        
        /// <summary>
        /// Fetch the first item matching the predicate, if none are found throw an exception.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{Func{TEntity, bool}}"/> defining the query to run.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="TEntity"/> which is found</returns>
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        /// <summary>
        /// Fetch the first item matching the predicate, if none are found return the default value.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{Func{TEntity, bool}}"/> defining the query to run.</param>
        /// <returns>An <see cref="TEntity"/> which is found</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression);
        
        /// <summary>
        /// Fetch the first item matching the predicate, if none are found return the default value.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{Func{TEntity, bool}}"/> defining the query to run.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="TEntity"/> which is found</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
	}
}

