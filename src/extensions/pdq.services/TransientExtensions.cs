using pdq.common.Connections;

namespace pdq.services
{
	public static class TransientExtensions
	{
		/// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static ICommand<TEntity> GetCommand<TEntity>(this ITransient transient)
			where TEntity : class, IEntity, new()
            => Command<TEntity>.Create(transient);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static ICommand<TEntity> GetCommand<TEntity, TKey>(this ITransient transient)
            where TEntity : class, IEntity<TKey>, new()
            => Command<TEntity, TKey>.Create(transient);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static ICommand<TEntity> GetCommand<TEntity, TKey1, TKey2>(this ITransient transient)
            where TEntity : class, IEntity<TKey1, TKey2>, new()
            => Command<TEntity, TKey1, TKey2>.Create(transient);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TKey3"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static ICommand<TEntity> GetCommand<TEntity, TKey1, TKey2, TKey3>(this ITransient transient)
            where TEntity : class, IEntity<TKey1, TKey2, TKey3>, new()
            => Command<TEntity, TKey1, TKey2, TKey3>.Create(transient);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static IQuery<TEntity> GetQuery<TEntity>(this ITransient transient)
            where TEntity : class, IEntity, new()
            => Query<TEntity>.Create(transient);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static IQuery<TEntity, TKey> GetQuery<TEntity, TKey>(this ITransient transient)
			where TEntity : class, IEntity<TKey>, new()
            => Query<TEntity, TKey>.Create(transient);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static IQuery<TEntity, TKey1, TKey2> GetQuery<TEntity, TKey1, TKey2>(this ITransient transient)
            where TEntity : class, IEntity<TKey1, TKey2>, new()
            => Query<TEntity, TKey1, TKey2>.Create(transient);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TKey3"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static IQuery<TEntity, TKey1, TKey2, TKey3> GetQuery<TEntity, TKey1, TKey2, TKey3>(this ITransient transient)
            where TEntity : class, IEntity<TKey1, TKey2, TKey3>, new()
            => Query<TEntity, TKey1, TKey2, TKey3>.Create(transient);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static IService<TEntity> GetService<TEntity>(this ITransient transient)
            where TEntity : class, IEntity, new()
            => Service<TEntity>.Create(transient);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static IService<TEntity, TKey> GetService<TEntity, TKey>(this ITransient transient)
            where TEntity : class, IEntity<TKey>, new()
            => Service<TEntity, TKey>.Create(transient);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static IService<TEntity, TKey1, TKey2> GetService<TEntity, TKey1, TKey2>(this ITransient transient)
            where TEntity : class, IEntity<TKey1, TKey2>, new()
            => Service<TEntity, TKey1, TKey2>.Create(transient);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TKey3"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static IService<TEntity, TKey1, TKey2, TKey3> GetService<TEntity, TKey1, TKey2, TKey3>(this ITransient transient)
            where TEntity : class, IEntity<TKey1, TKey2, TKey3>, new()
            => Service<TEntity, TKey1, TKey2, TKey3>.Create(transient);
    }
}

