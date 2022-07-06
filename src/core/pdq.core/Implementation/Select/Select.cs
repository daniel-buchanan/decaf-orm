using System;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
	internal class Select
        : Execute, ISelect, ISelectWithAlias, ISelectFrom, IWhere, IOrderByThen, IGroupBy, IGroupByThen
	{
        private readonly ISelectQueryContext context;

        public string Alias { get; private set; }

        private Select(IQuery query) : base((IQueryInternal)query)
        {
            this.context = SelectQueryContext.Create(this.query.AliasManager);
            this.query.SetContext(this.context);
        }

        public static Select Create(IQuery query) => new Select(query);

        internal ISelectQueryContext GetContext() => this.context;

        public void Dispose() => this.context.Dispose();

        public ISelectFrom Column(
            string name,
            string table = null,
            string tableAlias = null,
            string newName = null)
        {
            var managedTable = this.query.AliasManager.GetAssociation(tableAlias) ?? table;
            var managedAlias = this.query.AliasManager.Add(table, tableAlias);
            this.context.Select(state.Column.Create(name, state.QueryTargets.TableTarget.Create(managedTable, managedAlias), newName));
            return this;
        }

        public ISelectFrom Join(
            IQueryTarget from,
            state.IWhere conditions,
            IQueryTarget to,
            JoinType type = JoinType.Default)
        {
            this.context.Join(state.Join.Create(from, to, type, conditions));
            return this;
        }

        public ISelectFrom Join(
            IQueryTarget from,
            state.IWhere conditions,
            Action<ISelectWithAlias> query,
            JoinType type = JoinType.Default)
        {
            var select = Create(this.query);
            query(select);
            var to = state.QueryTargets.SelectQueryTarget.Create(select.context, select.Alias);

            this.context.Join(state.Join.Create(from, to, type, conditions));
            return this;
        }

        public IWhere Where(Action<IWhereBuilder> builder)
        {
            var b = WhereBuilder.Create(this.context);
            builder(b);
            this.context.Where(b.GetClauses().First());
            return this;
        }

        public ISelectFrom From(
            string table,
            string alias,
            string schema = null)
        {
            var managedTable = this.query.AliasManager.GetAssociation(alias) ?? table;
            var managedAlias = this.query.AliasManager.Add(table, alias);
            this.context.From(state.QueryTargets.TableTarget.Create(managedTable, managedAlias, schema));
            return this;
        }

        public ISelectFrom From(Action<ISelect> query, string alias)
        {
            var select = Create(this.query);
            query(select);

            this.query.AliasManager.Add(null, alias);
            var builtQuery = state.QueryTargets.SelectQueryTarget.Create(select.context, alias);
            this.context.From(builtQuery);

            return this;
        }

        public ISelectFromTyped<T> From<T>()
        {
            var table = this.context.Helpers().GetTableName<T>();
            var alias = this.query.AliasManager.Add(null, table);
            this.context.From(state.QueryTargets.TableTarget.Create(table, alias));
            return Select<T>.Create(this.context, this.query);
        }

        public ISelectFromTyped<T> From<T>(Expression<Func<T, T>> expression)
        {
            var table = this.context.Helpers().GetTableName(expression);
            var alias = this.context.Helpers().GetTableAlias(expression);

            var managedTable = this.query.AliasManager.GetAssociation(alias) ?? table;
            var managedAlias = this.query.AliasManager.Add(alias, table);

            this.context.From(state.QueryTargets.TableTarget.Create(managedTable, managedAlias));
            return Select<T>.Create(this.context, this.query);
        }

        public ISelectWithAlias KnownAs(string alias)
        {
            Alias = alias;
            return this;
        }

        public IOrderByThen OrderBy(string column, string tableAlias, SortOrder orderBy)
        {
            var managedTable = this.query.AliasManager.GetAssociation(tableAlias);
            var managedAlias = this.query.AliasManager.Add(tableAlias, managedTable);
            this.context.OrderBy(state.OrderBy.Create(column, state.QueryTargets.TableTarget.Create(managedTable, managedAlias), orderBy));
            return this;
        }

        public IOrderByThen ThenBy(string column, string tableAlias, SortOrder orderBy)
        {
            var managedTable = this.query.AliasManager.GetAssociation(tableAlias);
            var managedAlias = this.query.AliasManager.Add(tableAlias, managedTable);
            this.context.OrderBy(state.OrderBy.Create(column, state.QueryTargets.TableTarget.Create(managedTable, managedAlias), orderBy));
            return this;
        }

        public IGroupByThen GroupBy(string column, string tableAlias)
        {
            var managedTable = this.query.AliasManager.GetAssociation(tableAlias);
            var managedAlias = this.query.AliasManager.Add(tableAlias, managedTable);
            this.context.GroupBy(state.GroupBy.Create(column, state.QueryTargets.TableTarget.Create(managedTable, managedAlias)));
            return this;
        }

        public IOrderByThen ThenBy(string column, string tableAlias)
        {
            var managedTable = this.query.AliasManager.GetAssociation(tableAlias);
            var managedAlias = this.query.AliasManager.Add(tableAlias, managedTable);
            this.context.GroupBy(state.GroupBy.Create(column, state.QueryTargets.TableTarget.Create(managedTable, managedAlias)));
            return this;
        }
    }
}

