using Microsoft.Extensions.DependencyInjection;

namespace pdq.services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceCollectionHandler
	{
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		IServiceCollection AsScoped();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		IServiceCollection AsSingleton();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		IServiceCollection AsTransient();
	}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="TEntity"></typeparam>
    ///// <typeparam name="TKey"></typeparam>
    //public interface IServiceCollectionHandler<TEntity, TKey> :
    //    IServiceCollectionHandler<TEntity>
    //    where TEntity : class, IEntity<TKey>
    //{ }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="TEntity"></typeparam>
    ///// <typeparam name="TKey1"></typeparam>
    ///// <typeparam name="TKey2"></typeparam>
    //public interface IServiceCollectionHandler<TEntity, TKey1, TKey2> :
    //    IServiceCollectionHandler<TEntity>
    //    where TEntity : class, IEntity<TKey1, TKey2>
    //{ }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="TEntity"></typeparam>
    ///// <typeparam name="TKey1"></typeparam>
    ///// <typeparam name="TKey2"></typeparam>
    ///// <typeparam name="TKey3"></typeparam>
    //public interface IServiceCollectionHandler<TEntity, TKey1, TKey2, TKey3> :
    //    IServiceCollectionHandler<TEntity>
    //    where TEntity : class, IEntity<TKey1, TKey2, TKey3>
    //{ }
}

