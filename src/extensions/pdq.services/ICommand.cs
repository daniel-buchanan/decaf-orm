using System;
namespace pdq.services
{
	public interface ICommand<TEntity> : IDisposable where TEntity : IEntity
	{
		
	}

	public interface ICommand<TEntity, TKey> : ICommand<TEntity>
		where TEntity: IEntity<TKey>
    {

    }
}

