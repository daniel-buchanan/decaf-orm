using System;
namespace pdq.core.state
{
	public class DeleteQueryContext : QueryContext, IDeleteQueryContext
	{
		private Table? table;
		private IWhere? where;

        public override ContextKind Kind => ContextKind.Delete;

		public Table Table => this.table;

		public IWhere WhereClause => this.where;

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
    }
}

