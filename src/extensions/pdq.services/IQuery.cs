using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace pdq.services
{
	public interface IQuery<TEntity> : IDisposable where TEntity : IEntity
	{
		IEnumerable<TEntity> All();

		IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> query);
	}

	public interface IQuery<TEntity, TKey> : IQuery<TEntity>
		where TEntity : IEntity<TKey>
    {
		TEntity Get(TKey key);

		IEnumerable<TEntity> Get(params TKey[] keys);

		IEnumerable<TEntity> Get(IEnumerable<TKey> keys);
    }
}

