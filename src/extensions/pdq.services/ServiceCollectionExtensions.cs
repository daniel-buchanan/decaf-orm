using Microsoft.Extensions.DependencyInjection;

namespace pdq.services
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollectionHandler<TEntity> AddPdqService<TEntity>(this IServiceCollection services)
			where TEntity : IEntity
        {
			return new ServiceCollectionHandler<TEntity>(services);
        }

		public static IServiceCollectionHandler<TEntity> AddPdqService<TEntity, TKey>(this IServiceCollection services)
			where TEntity : IEntity<TKey>
		{
			return new ServiceCollectionHandler<TEntity, TKey>(services);
		}
	}
}

