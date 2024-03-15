using decaf.common.Connections;

namespace decaf.services
{
	public static class UnitOfWorkExtensions
	{
		/// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
		public static ICommand<TEntity> GetCommand<TEntity>(this IUnitOfWork unitOfWork)
			where TEntity : class, IEntity, new()
            => Command<TEntity>.Create(unitOfWork);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
		public static ICommand<TEntity> GetCommand<TEntity, TKey>(this IUnitOfWork unitOfWork)
            where TEntity : class, IEntity<TKey>, new()
            => Command<TEntity, TKey>.Create(unitOfWork);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
		public static ICommand<TEntity> GetCommand<TEntity, TKey1, TKey2>(this IUnitOfWork unitOfWork)
            where TEntity : class, IEntity<TKey1, TKey2>, new()
            => Command<TEntity, TKey1, TKey2>.Create(unitOfWork);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TKey3"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
		public static ICommand<TEntity> GetCommand<TEntity, TKey1, TKey2, TKey3>(this IUnitOfWork unitOfWork)
            where TEntity : class, IEntity<TKey1, TKey2, TKey3>, new()
            => Command<TEntity, TKey1, TKey2, TKey3>.Create(unitOfWork);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
		public static IQuery<TEntity> GetQuery<TEntity>(this IUnitOfWork unitOfWork)
            where TEntity : class, IEntity, new()
            => Query<TEntity>.Create(unitOfWork);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
		public static IQuery<TEntity, TKey> GetQuery<TEntity, TKey>(this IUnitOfWork unitOfWork)
			where TEntity : class, IEntity<TKey>, new()
            => Query<TEntity, TKey>.Create(unitOfWork);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
		public static IQuery<TEntity, TKey1, TKey2> GetQuery<TEntity, TKey1, TKey2>(this IUnitOfWork unitOfWork)
            where TEntity : class, IEntity<TKey1, TKey2>, new()
            => Query<TEntity, TKey1, TKey2>.Create(unitOfWork);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TKey3"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
		public static IQuery<TEntity, TKey1, TKey2, TKey3> GetQuery<TEntity, TKey1, TKey2, TKey3>(this IUnitOfWork unitOfWork)
            where TEntity : class, IEntity<TKey1, TKey2, TKey3>, new()
            => Query<TEntity, TKey1, TKey2, TKey3>.Create(unitOfWork);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
		public static IService<TEntity> GetService<TEntity>(this IUnitOfWork unitOfWork)
            where TEntity : class, IEntity, new()
            => Service<TEntity>.Create(unitOfWork);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
		public static IService<TEntity, TKey> GetService<TEntity, TKey>(this IUnitOfWork unitOfWork)
            where TEntity : class, IEntity<TKey>, new()
            => Service<TEntity, TKey>.Create(unitOfWork);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
		public static IService<TEntity, TKey1, TKey2> GetService<TEntity, TKey1, TKey2>(this IUnitOfWork unitOfWork)
            where TEntity : class, IEntity<TKey1, TKey2>, new()
            => Service<TEntity, TKey1, TKey2>.Create(unitOfWork);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TKey3"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
		public static IService<TEntity, TKey1, TKey2, TKey3> GetService<TEntity, TKey1, TKey2, TKey3>(this IUnitOfWork unitOfWork)
            where TEntity : class, IEntity<TKey1, TKey2, TKey3>, new()
            => Service<TEntity, TKey1, TKey2, TKey3>.Create(unitOfWork);
    }
}

