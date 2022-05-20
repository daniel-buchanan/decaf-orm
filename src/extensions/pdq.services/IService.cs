using System;
namespace pdq.services
{
	public interface IService<TEntity> : IQuery<TEntity>, ICommand<TEntity>
		where TEntity : IEntity
	{
	}
}

