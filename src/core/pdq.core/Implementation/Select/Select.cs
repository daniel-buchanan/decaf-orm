using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using pdq.common;
using pdq.common.Utilities;
using pdq.state;
using pdq.state.Utilities;
using static Dapper.SqlMapper;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.core-tests")]
namespace pdq.Implementation
{
    internal class Select :
        SelectCommon,
        ISelectWithAlias,
        ISelectFrom,
        IOrderByThen,
        IGroupBy,
        IGroupByThen
	{
        private Select(
            ISelectQueryContext context,
            IQueryContainerInternal query)
            : base(context, query) { }

        public static Select Create(
            ISelectQueryContext context,
            IQueryContainerInternal query)
            => new Select(context, query);

        /// <inheritdoc/>
        internal ISelectQueryContext GetContext() => this.context;

        /// <inheritdoc/>
        public string Alias { get; private set; }

        /// <inheritdoc/>
        public IJoinFrom Join()
            => Implementation.Join.Create(this, context, options, query, JoinType.Default);

        /// <inheritdoc/>
        public IJoinFrom InnerJoin()
            => Implementation.Join.Create(this, context, options, query, JoinType.Inner);

        /// <inheritdoc/>
        public IJoinFrom LeftJoin()
            => Implementation.Join.Create(this, context, options, query, JoinType.Left);

        /// <inheritdoc/>
        public IJoinFrom RightJoin()
            => Implementation.Join.Create(this, context, options, query, JoinType.Right);

        /// <inheritdoc/>
        public IGroupBy Where(Action<IWhereBuilder> builder)
        {
            var b = WhereBuilder.Create(this.options, this.context) as IWhereBuilderInternal;
            builder(b);
            var clause = b.GetClauses().First();
            if((clause is state.Conditionals.And ||
               clause is state.Conditionals.Or) &&
               clause.Children.Count == 1)
            {
                clause = clause.Children.First();
            }

            this.context.Where(clause);
            return this;
        }

        /// <inheritdoc/>
        public ISelectFrom From(
            string table,
            string alias,
            string schema = null)
        {
            var managedTable = this.query.AliasManager.GetAssociation(alias) ?? table;
            var managedAlias = this.query.AliasManager.Add(alias, managedTable);
            this.context.From(state.QueryTargets.TableTarget.Create(managedTable, managedAlias, schema));
            return this;
        }

        /// <inheritdoc/>
        public ISelectFrom From(Action<ISelect> query, string alias)
        {
            var selectContext = SelectQueryContext.Create(AliasManager.Create(), HashProvider.Create());
            var select = Create(selectContext, this.query);
            query(select);

            this.query.AliasManager.Add(alias, "query");
            var builtQuery = state.QueryTargets.SelectQueryTarget.Create(select.context, alias);
            this.context.From(builtQuery);

            return this;
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T> From<T>()
        {
            AddFrom<T>();
            return SelectTyped<T>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T> From<T>(Expression<Func<T, T>> aliasExpression)
        {
            AddFrom<T>(aliasExpression);
            return SelectTyped<T>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T> From<T>(Action<ISelectWithAlias> query, string alias)
        {
            var selectContext = SelectQueryContext.Create(AliasManager.Create(), this.query.HashProvider);
            var selectQuery = QueryFramework.Create(this.query.Options, this.query.Logger, this.query.Transient, this.query.HashProvider) as IQueryContainerInternal;
            var select = Create(selectContext, selectQuery);
            query(select);

            var target = state.QueryTargets.SelectQueryTarget.Create(selectContext, select.Alias ?? alias);

            var table = this.context.Helpers().GetTableName<T>();
            this.query.AliasManager.Add(table, select.Alias ?? alias);

            this.context.From(target);
            return SelectTyped<T>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public ISelectWithAlias KnownAs(string alias)
        {
            Alias = alias;
            return this;
        }

        /// <inheritdoc/>
        public IOrderByThen OrderBy(string column, string tableAlias, SortOrder orderBy)
        {
            var managedTable = this.query.AliasManager.GetAssociation(tableAlias);
            var managedAlias = this.query.AliasManager.Add(tableAlias, managedTable);
            this.context.OrderBy(state.OrderBy.Create(column, state.QueryTargets.TableTarget.Create(managedTable, managedAlias), orderBy));
            return this;
        }

        /// <inheritdoc/>
        public IGroupByThen GroupBy(string column, string tableAlias)
        {
            var managedTable = this.query.AliasManager.GetAssociation(tableAlias);
            var managedAlias = this.query.AliasManager.Add(tableAlias, managedTable);
            this.context.GroupBy(state.GroupBy.Create(column, state.QueryTargets.TableTarget.Create(managedTable, managedAlias)));
            return this;
        }

        /// <inheritdoc/>
        public IOrderByThen ThenBy(string column, string tableAlias, SortOrder orderBy)
            => OrderBy(column, tableAlias, orderBy);

        /// <inheritdoc/>
        public IGroupByThen ThenBy(string column, string tableAlias)
            => GroupBy(column, tableAlias);

        /// <inheritdoc/>
        IExecuteDynamic ISelectColumn.Select(Expression<Func<ISelectColumnBuilder, dynamic>> expression)
        {
            base.AddColumns(expression);
            return this;
        }

        /// <inheritdoc/>
        IExecute<TResult> ISelectColumn.Select<TResult>(Expression<Func<ISelectColumnBuilder, TResult>> expression)
        {
            base.AddColumns(expression);
            return Execute<TResult, ISelectQueryContext>.Create(this.query, this.context);
        }

        /// <inheritdoc/>
        public IExecute<TResult> SelectAll<TResult>(string alias)
        {
            base.AddAllColumns<TResult>(alias);
            return Execute<TResult, ISelectQueryContext>.Create(this.query, this.context);
        }
    }
}

