using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.core-tests")]
namespace pdq.state
{
	internal class SelectQueryContext : QueryContext, ISelectQueryContext
	{
        private readonly List<Column> columns;
        private readonly List<Join> joins;
        private IWhere where;
        private readonly List<OrderBy> orderByClauses;
        private readonly List<GroupBy> groupByClauses;


		private SelectQueryContext(IAliasManager aliasManager)
            : base(aliasManager, QueryTypes.Select)
		{
            this.columns = new List<Column>();
            this.joins = new List<Join>();
            this.orderByClauses = new List<OrderBy>();
            this.groupByClauses = new List<GroupBy>();
		}

        internal static ISelectQueryContext Create(IAliasManager aliasManager)
            => new SelectQueryContext(aliasManager);

        /// <inheritdoc/>
        public IReadOnlyCollection<Column> Columns => this.columns.AsReadOnly();

        /// <inheritdoc/>
        public IReadOnlyCollection<Join> Joins => this.joins.AsReadOnly();

        /// <inheritdoc/>
        public IWhere WhereClause => this.where;

        /// <inheritdoc/>
        public IReadOnlyCollection<OrderBy> OrderByClauses => this.orderByClauses.AsReadOnly();

        /// <inheritdoc/>
        public IReadOnlyCollection<GroupBy> GroupByClauses => this.groupByClauses.AsReadOnly();

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.columns.DisposeAll();
            this.joins.DisposeAll();
            this.orderByClauses.DisposeAll();
            this.groupByClauses.DisposeAll();
            this.queryTargets.DisposeAll();
            this.where = null;
        }

        /// <inheritdoc/>
        public ISelectQueryContext From(IQueryTarget table)
        {
            var item = this.QueryTargets.FirstOrDefault(t => t.IsEquivalentTo(table));
            if (item != null) return this;

            this.queryTargets.Add(table);
            return this;
        }

        /// <inheritdoc/>
        public ISelectQueryContext GroupBy(GroupBy groupBy)
        {
            var item = this.groupByClauses.FirstOrDefault(c => c.IsEquivalentTo(groupBy));
            if (item != null) return this;

            this.groupByClauses.Add(groupBy);
            return this;
        }

        /// <inheritdoc/>
        public ISelectQueryContext Join(Join join)
        {
            var existing = this.joins.FirstOrDefault(j => j.From.IsEquivalentTo(join.From) &&
                j.To.IsEquivalentTo(join.To));

            if (existing != null) return this;

            this.joins.Add(join);
            return this;
        }

        /// <inheritdoc/>
        public ISelectQueryContext OrderBy(OrderBy orderBy)
        {
            var item = this.orderByClauses.FirstOrDefault(c => c.IsEquivalentTo(orderBy));
            if (item != null) return this;

            this.orderByClauses.Add(orderBy);
            return this;
        }

        /// <inheritdoc/>
        public ISelectQueryContext Select(Column column)
        {
            var item = this.columns.FirstOrDefault(c => c.IsEquivalentTo(column));
            if (item != null) return this;

            this.columns.Add(column);
            return this;
        }

        /// <inheritdoc/>
        public ISelectQueryContext Where(IWhere where)
        {
            this.where = where;
            return this;
        }
    }
}

