using pdq.common;

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
		public static IService<TEntity> GetService<TEntity>(this ITransient transient)
			where TEntity : class, IEntity => Service<TEntity>.Create(transient);

		/// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static IQuery<TEntity> GetQuery<TEntity>(this ITransient transient)
			where TEntity : class, IEntity => Query<TEntity>.Create(transient);

		/// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static ICommand<TEntity> GetCommand<TEntity>(this ITransient transient)
			where TEntity : class, IEntity => Command<TEntity>.Create(transient);

		/// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static IService<TEntity, TKey> GetService<TEntity, TKey>(this ITransient transient)
			where TEntity : class, IEntity<TKey> => Service<TEntity, TKey>.Create(transient);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static IQuery<TEntity, TKey> GetQuery<TEntity, TKey>(this ITransient transient)
			where TEntity : class, IEntity<TKey> => Query<TEntity, TKey>.Create(transient);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="transient"></param>
        /// <returns></returns>
		public static ICommand<TEntity> GetCommand<TEntity, TKey>(this ITransient transient)
			where TEntity : class, IEntity<TKey> => Command<TEntity, TKey>.Create(transient);
	}
}

