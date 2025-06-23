using decaf.services;
using Microsoft.Extensions.DependencyInjection;

namespace decaf;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollectionHandler AddDecafService<TEntity>(this IServiceCollection services)
        where TEntity : class, IEntity, new() => ServiceCollectionHandler<TEntity>.Create(services);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollectionHandler AddDecafService<TEntity, TKey>(this IServiceCollection services)
        where TEntity : class, IEntity<TKey>, new() => ServiceCollectionHandler<TEntity, TKey>.Create(services);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey1"></typeparam>
    /// <typeparam name="TKey2"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollectionHandler AddDecafService<TEntity, TKey1, TKey2>(this IServiceCollection services)
        where TEntity : class, IEntity<TKey1, TKey2>, new() => ServiceCollectionHandler<TEntity, TKey1, TKey2>.Create(services);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey1"></typeparam>
    /// <typeparam name="TKey2"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollectionHandler AddDecafService<TEntity, TKey1, TKey2, TKey3>(this IServiceCollection services)
        where TEntity : class, IEntity<TKey1, TKey2, TKey3>, new() => ServiceCollectionHandler<TEntity, TKey1, TKey2, TKey3>.Create(services);
}