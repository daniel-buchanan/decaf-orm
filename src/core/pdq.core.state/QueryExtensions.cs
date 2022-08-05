using pdq.common;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq")]
namespace pdq.state
{
	internal static class QueryExtensions
	{
		/// <summary>
        /// Create an <see cref="IQueryContext"/> from an <see cref="IQuery"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IQueryContext"/> to create.</typeparam>
        /// <param name="query">Self.</param>
        /// <returns>A newly created <see cref="IQueryContext"/>.</returns>
		public static T CreateContext<T>(this IQuery query) where T: IQueryContext
        {
			var internalQuery = query as IQueryInternal;
			var queryContextType = typeof(T);

			if (internalQuery == null) return default(T);

			if(queryContextType.IsAssignableFrom(typeof(ISelectQueryContext)))
				return (T)SelectQueryContext.Create(internalQuery.AliasManager);

			if (queryContextType.IsAssignableFrom(typeof(IDeleteQueryContext)))
				return (T)DeleteQueryContext.Create(internalQuery.AliasManager);

			if (queryContextType.IsAssignableFrom(typeof(IInsertQueryContext)))
				return (T)InsertQueryContext.Create(internalQuery.AliasManager);

			return default(T);
        }
	}
}

