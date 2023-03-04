using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace pdq.services
{
    public interface IQuery : IExecutionNotifiable { }

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
        /// Find items using a query.
        /// </summary>
        /// <param name="query">The <see cref="Expression{Func{TEntity, bool}}"/> defining the query to run.</param>
        /// <returns>An <see cref="IEnumerable{TEntity}"/> set of the results.</returns>
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> query);
    }
}

