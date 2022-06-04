using pdq.core.common;

namespace pdq.core.state
{
	public class DeleteQueryContext : QueryContext, IDeleteQueryContext
	{
		private Table? table;
		private IWhere? where;

		private DeleteQueryContext() : base(QueryType.Delete)
        {
			this.table = null;
			this.where = null;
        }

		public Table? Table => this.table;

		public IWhere? WhereClause => this.where;

		public IDeleteQueryContext From(Table table)
        {
			this.table = table;
			return this;
        }

		public IDeleteQueryContext Where(IWhere where)
        {
			this.where = where;
			return this;
        }

		public static IDeleteQueryContext Create() => new DeleteQueryContext();
    }
}

