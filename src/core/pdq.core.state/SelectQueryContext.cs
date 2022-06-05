using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common;

namespace pdq.state
{
	public class SelectQueryContext : QueryContext, ISelectQueryContext
	{
        private readonly List<IQueryTarget> tables;
        private readonly List<Column> columns;
        private readonly List<Join> joins;
        private IWhere where;
        private readonly List<OrderBy> orderByClauses;
        private readonly List<GroupBy> groupByClauses;


		public SelectQueryContext() : base(QueryType.Select)
		{
            this.tables = new List<IQueryTarget>();
            this.columns = new List<Column>();
            this.joins = new List<Join>();
            this.orderByClauses = new List<OrderBy>();
            this.groupByClauses = new List<GroupBy>();
		}

        public static ISelectQueryContext Create() => new SelectQueryContext();

        public IReadOnlyCollection<IQueryTarget> Tables => this.tables.AsReadOnly();

        public IReadOnlyCollection<Column> Columns => this.columns.AsReadOnly();

        public IReadOnlyCollection<Join> Joins => this.joins.AsReadOnly();

        public IWhere WhereClause => this.where;

        public IReadOnlyCollection<OrderBy> OrderByClauses => this.orderByClauses.AsReadOnly();

        public IReadOnlyCollection<GroupBy> GroupByClauses => this.groupByClauses.AsReadOnly();

        public override void Dispose()
        {
            this.tables.Dispose();
            this.columns.Dispose();
            this.joins.Dispose();
            this.orderByClauses.Dispose();
            this.groupByClauses.Dispose();
            this.where = null;
        }

        public ISelectQueryContext From(IQueryTarget table)
        {
            var item = this.tables.FirstOrDefault(t => t.IsEquivalentTo(table));
            if (item != null) return this;

            this.tables.Add(table);
            return this;
        }

        public ISelectQueryContext GroupBy(GroupBy groupBy)
        {
            this.groupByClauses.Add(groupBy);
            return this;
        }

        public ISelectQueryContext Join(Join join)
        {
            this.joins.Add(join);
            return this;
        }

        public ISelectQueryContext OrderBy(OrderBy orderBy)
        {
            this.orderByClauses.Add(orderBy);
            return this;
        }

        public ISelectQueryContext Select(Column column)
        {
            var item = this.columns.FirstOrDefault(c => c.IsEquivalentTo(column));
            if (item != null) return this;

            this.columns.Add(column);
            return this;
        }

        public ISelectQueryContext Where(IWhere where)
        {
            this.where = where;
            return this;
        }
    }
}

