using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace pdq.services
{
    /// <summary>
    /// A Service for making modifications to a given <see cref="TEntity"/>. 
    /// </summary>
    /// <typeparam name="TEntity">The type of <see cref="IEntity"/> to work with.</typeparam>
	public partial interface ICommand<TEntity>
        where TEntity : IEntity, new()
    {
        /// <summary>
        /// Add an item to the database.
        /// </summary>
        /// <param name="toAdd">The <see cref="TEntity"/> to be added.</param>
        /// <returns>The updated <see cref="TEntity"/> which has been added.</returns>
        Task<TEntity> AddAsync(TEntity toAdd, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add an item to the database.
        /// </summary>
        /// <param name="items">The <see cref="TEntity"/> to be added.</param>
        /// <returns>The updated <see cref="TEntity"/> which has been added.</returns>
		Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update the provided item.
        /// </summary>
        /// <param name="toUpdate">The details to update.</param>
        /// <param name="expression">An expression specifying which item(s) to update.</param>
        Task UpdateAsync(TEntity toUpdate, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update the provided item.
        /// </summary>
        /// <param name="toUpdate">The details to update.</param>
        /// <param name="expression">An expression specifying which item(s) to update.</param>
        Task UpdateAsync(dynamic toUpdate, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete one or more items.
        /// </summary>
        /// <param name="expression">An expression specifying which item(s) should be deleted.</param>
        Task DeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    }
}

