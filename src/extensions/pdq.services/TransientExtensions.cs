using System;
using pdq;
using pdq.common;

namespace pdq.services
{
	public static class TransientExtensions
	{
		public static IService<TEntity> GetService<TEntity>(this ITransient transient)
			where TEntity: IEntity
        {
			throw new NotImplementedException();
        }

		public static IQuery<TEntity> GetQuery<TEntity>(this ITransient transient)
			where TEntity: IEntity
        {
			throw new NotImplementedException();
		}

		public static ICommand<TEntity> GetCommand<TEntity>(this ITransient transient)
			where TEntity: IEntity
        {
			throw new NotImplementedException();
		}

		public static IService<TEntity> GetService<TEntity, TKey>(this ITransient transient)
			where TEntity : IEntity<TKey>
		{
			throw new NotImplementedException();
		}

		public static IQuery<TEntity> GetQuery<TEntity, TKey>(this ITransient transient)
			where TEntity : IEntity<TKey>
		{
			throw new NotImplementedException();
		}

		public static ICommand<TEntity> GetCommand<TEntity, TKey>(this ITransient transient)
			where TEntity : IEntity<TKey>
		{
			throw new NotImplementedException();
		}
	}
}

