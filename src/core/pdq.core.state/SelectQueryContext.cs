using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common;
using pdq.common.Utilities;

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


		private SelectQueryContext(
            IAliasManager aliasManager,
            IHashProvider hashProvider)
            : base(aliasManager, QueryTypes.Select, hashProvider)
		{
            this.columns = new List<Column>();
            this.joins = new List<Join>();
            this.orderByClauses = new List<OrderBy>();
            this.groupByClauses = new List<GroupBy>();
		}

        internal static ISelectQueryContext Create(IAliasManager aliasManager, IHashProvider hashProvider)
            => new SelectQueryContext(aliasManager, hashProvider);

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
            this.where = null;
        }

        /// <inheritdoc/>
        public void From(IQueryTarget table)
        {
            var item = this.QueryTargets.FirstOrDefault(t => t.IsEquivalentTo(table));
            if (item != null) return;

            var internalContext = this as IQueryContextInternal;
            internalContext.AddQueryTarget(table);
        }

        /// <inheritdoc/>
        public void GroupBy(GroupBy groupBy)
        {
            var item = this.groupByClauses.FirstOrDefault(c => c.IsEquivalentTo(groupBy));
            if (item != null) return;

            this.groupByClauses.Add(groupBy);
        }

        /// <inheritdoc/>
        public void Join(Join join)
        {
            var existing = this.joins.FirstOrDefault(j => j.From.IsEquivalentTo(join.From) &&
                j.To.IsEquivalentTo(join.To));

            if (existing != null) return;

            this.joins.Add(join);
        }

        /// <inheritdoc/>
        public void OrderBy(OrderBy orderBy)
        {
            var item = this.orderByClauses.FirstOrDefault(c => c.IsEquivalentTo(orderBy));
            if (item != null) return;

            this.orderByClauses.Add(orderBy);
        }

        /// <inheritdoc/>
        public void Select(Column column)
        {
            var item = this.columns.FirstOrDefault(c => c.IsEquivalentTo(column));
            if (item != null) return;

            this.columns.Add(column);
        }

        /// <inheritdoc/>
        public void Where(IWhere where) => this.where = where;
    }
}

