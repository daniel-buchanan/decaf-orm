using pdq.common;

namespace pdq.state
{
	internal class DeleteQueryContext : QueryContext, IDeleteQueryContext
	{
		private DeleteQueryContext(IAliasManager aliasManager)
			: base(aliasManager, QueryTypes.Delete)
        {
			Table = null;
			WhereClause = null;
        }

		/// <inheritdoc/>
		public ITableTarget Table { get; private set; }

		/// <inheritdoc/>
		public IWhere WhereClause { get; private set; }

		/// <inheritdoc/>
		public IDeleteQueryContext From(ITableTarget target)
        {
			Table = target;
			((IQueryContextInternal)this).AddQueryTarget(target);
			return this;
        }

		/// <inheritdoc/>
		public IDeleteQueryContext Where(IWhere where)
        {
			WhereClause = where;
			return this;
        }

		/// <summary>
        /// Create a <see cref="IDeleteQueryContext"/> Context.
        /// </summary>
        /// <param name="aliasManager">The <see cref="IAliasManager"/> to use.</param>
        /// <returns>A new instance which implements <see cref="IDeleteQueryContext"/>.</returns>
		public static IDeleteQueryContext Create(IAliasManager aliasManager)
			=> new DeleteQueryContext(aliasManager);

		/// <inheritdoc/>
		public override void Dispose()
        {
			Table = null;
			WhereClause = null;
		}
    }
}

