using System.Collections.Generic;
using System.Linq;
using decaf.common;
using decaf.common.Utilities;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("decaf.core.tests")]
namespace decaf.state
{
	internal class SelectQueryContext : QueryContext, ISelectQueryContext
	{
        private readonly List<Column> columns;
        private readonly List<Join> joins;
        private IWhere where;
        private readonly List<OrderBy> orderByClauses;
        private readonly List<GroupBy> groupByClauses;
        private int? limit;

		private SelectQueryContext(
            IAliasManager aliasManager,
            IHashProvider hashProvider)
            : base(aliasManager, QueryTypes.Select, hashProvider)
		{
            columns = new List<Column>();
            joins = new List<Join>();
            orderByClauses = new List<OrderBy>();
            groupByClauses = new List<GroupBy>();
		}

        internal static ISelectQueryContext Create(IAliasManager aliasManager, IHashProvider hashProvider)
            => new SelectQueryContext(aliasManager, hashProvider);

        /// <inheritdoc/>
        public IReadOnlyCollection<Column> Columns => columns.AsReadOnly();

        /// <inheritdoc/>
        public IReadOnlyCollection<Join> Joins => joins.AsReadOnly();

        /// <inheritdoc/>
        public IWhere WhereClause => where;

        /// <inheritdoc/>
        public IReadOnlyCollection<OrderBy> OrderByClauses => orderByClauses.AsReadOnly();

        /// <inheritdoc/>
        public IReadOnlyCollection<GroupBy> GroupByClauses => groupByClauses.AsReadOnly();

        /// <inheritdoc/>
        public int? RowLimit => limit;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            columns.DisposeAll();
            joins.DisposeAll();
            orderByClauses.DisposeAll();
            groupByClauses.DisposeAll();
            where = null;
        }

        /// <inheritdoc/>
        public void From(IQueryTarget table)
        {
            var item = QueryTargets.FirstOrDefault(t => t.IsEquivalentTo(table));
            if (item != null) return;

            var internalContext = this as IQueryContextExtended;
            internalContext.AddQueryTarget(table);
        }

        /// <inheritdoc/>
        public void GroupBy(GroupBy groupBy)
        {
            var item = groupByClauses.FirstOrDefault(c => c.IsEquivalentTo(groupBy));
            if (item != null) return;

            groupByClauses.Add(groupBy);
        }

        /// <inheritdoc/>
        public void Join(Join join)
        {
            var existing = joins.FirstOrDefault(j => j.From.IsEquivalentTo(join.From) &&
                j.To.IsEquivalentTo(join.To));

            if (existing != null) return;

            joins.Add(join);
        }

        /// <inheritdoc/>
        public void OrderBy(OrderBy orderBy)
        {
            var item = orderByClauses.FirstOrDefault(c => c.IsEquivalentTo(orderBy));
            if (item != null) return;

            orderByClauses.Add(orderBy);
        }

        /// <inheritdoc/>
        public void Select(Column column)
        {
            var item = columns.FirstOrDefault(c => c.IsEquivalentTo(column));
            if (item != null) return;

            columns.Add(column);
        }

        /// <inheritdoc/>
        public void Where(IWhere where) => this.where = where;

        /// <inheritdoc/>
        public void Limit(int limit)
            => this.limit = limit;
    }
}

