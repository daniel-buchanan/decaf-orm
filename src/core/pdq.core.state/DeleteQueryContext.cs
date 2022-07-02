﻿using pdq.common;

namespace pdq.state
{
	public class DeleteQueryContext : QueryContext, IDeleteQueryContext
	{
		private DeleteQueryContext(IAliasManager aliasManager)
			: base(aliasManager, QueryType.Delete)
        {
			Table = null;
			WhereClause = null;
        }

		public ITableTarget Table { get; private set; }

		public IWhere WhereClause { get; private set; }

		public IDeleteQueryContext From(ITableTarget table)
        {
			Table = table;
			return this;
        }

		public IDeleteQueryContext Where(IWhere where)
        {
			WhereClause = where;
			return this;
        }

		public static IDeleteQueryContext Create(IAliasManager aliasManager)
			=> new DeleteQueryContext(aliasManager);

        public override void Dispose()
        {
			Table = null;
			WhereClause = null;
		}
    }
}

