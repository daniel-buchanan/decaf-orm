using Microsoft.Extensions.DependencyInjection;
using pdq.services;

namespace pdq
{
	public static class ServiceCollectionExtensions
	{
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollectionHandler AddPdqService<TEntity>(this IServiceCollection services)
            where TEntity : class, IEntity, new() => ServiceCollectionHandler<TEntity>.Create(services);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollectionHandler AddPdqService<TEntity, TKey>(this IServiceCollection services)
            where TEntity : class, IEntity<TKey>, new() => ServiceCollectionHandler<TEntity, TKey>.Create(services);
    }
}

