using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace decaf.services;

/// <summary>
/// A Service for making modifications to a given <see cref="TEntity"/>. 
/// </summary>
/// <typeparam name="TEntity">The type of <see cref="IEntity{TKey}"/> to work with.</typeparam>
/// <typeparam name="TKey">The data type of the primary key.</typeparam>
public partial interface ICommand<TEntity, TKey>
{
	/// <summary>
	/// Add an item to the database.
	/// </summary>
	/// <param name="item">The <see cref="TEntity"/> to be added.</param>
	/// <param name="cancellationToken">The cancellation token to use.</param>
	/// <returns>The updated <see cref="TEntity"/> which has been added.</returns>
	new Task<TEntity> AddAsync(TEntity item, CancellationToken cancellationToken = default);

	/// <summary>
	/// Add one or more items to the database.
	/// </summary>
	/// <param name="items">The <see cref="TEntity"/> to be added.</param>
	/// <param name="cancellationToken">The cancellation token to use.</param>
	/// <returns>The updated <see cref="IEnumerable{TEntity}"/> which has been added.</returns>
	new Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);

	/// <summary>
	/// Update the entire item, using it's Primary Key.
	/// </summary>
	/// <param name="item">The item to update.</param>
	/// <param name="cancellationToken">The cancellation token to use.</param>
	Task UpdateAsync(TEntity item, CancellationToken cancellationToken = default);

	/// <summary>
	/// Update only select fields for a specific item.
	/// </summary>
	/// <param name="toUpdate">The fields/columns to update.</param>
	/// <param name="key">The <see cref="TKey"/> of the item to update.</param>
	/// <param name="cancellationToken">The cancellation token to use.</param>
	Task UpdateAsync(dynamic toUpdate, TKey key, CancellationToken cancellationToken = default);

	/// <summary>
	/// Delete a specific item.
	/// </summary>
	/// <param name="key">The <see cref="TKey"/> of the item to delete.</param>
	/// <param name="cancellationToken">The cancellation token to use.</param>
	Task DeleteAsync(TKey key, CancellationToken cancellationToken = default);

	/// <summary>
	/// Delete a number of specified items.
	/// </summary>
	/// <param name="keys">The set of <see cref="TKey"/>s to delete. </param>
	/// <param name="cancellationToken">The cancellation token to use.</param>
	Task DeleteAsync(TKey[] keys, CancellationToken cancellationToken = default);

	/// <summary>
	/// Delete a number of specified items.
	/// </summary>
	/// <param name="keys">The set of <see cref="TKey"/>s to delete. </param>
	/// <param name="cancellationToken">The cancellation token to use.</param>
	Task DeleteAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default);
}