using System;
namespace pdq.services
{
	public interface IService<TEntity> : IQuery<TEntity>, ICommand<TEntity>, IDisposable
		where TEntity : IEntity
	{
	}

	public interface IService<TEntity, TKey> : IQuery<TEntity, TKey>, ICommand<TEntity, TKey>, IDisposable
		where TEntity: IEntity<TKey>
    {

    }
}

