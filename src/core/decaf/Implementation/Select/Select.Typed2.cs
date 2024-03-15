using System;
using System.Linq.Expressions;
using decaf.common;
using decaf.state;

namespace decaf.Implementation
{
    internal class SelectTyped<T1, T2>
        : SelectTyped,
        ISelectFromTyped<T1, T2>,
        IGroupByTyped<T1, T2>,
        IGroupByThenTyped<T1, T2>,
        IOrderByThenTyped<T1, T2>
	{
        private SelectTyped(
            ISelectQueryContext context,
            IQueryContainerInternal query)
            : base(context, query)
        {
        }

        public static SelectTyped<T1, T2> Create(
            ISelectQueryContext context,
            IQueryContainerInternal query)
            => new SelectTyped<T1, T2>(context, query);

        /// <inheritdoc/>
        public ISelectFromTyped<T1, T2, T3> From<T3>()
        {
            AddFrom<T3>();
            return SelectTyped<T1, T2, T3>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T1, T2, T3> From<T3>(Expression<Func<T3, T3>> expression)
        {
            AddFrom<T3>(expression);
            return SelectTyped<T1, T2, T3>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public IGroupByThenTyped<T1, T2> GroupBy(Expression<Func<T1, T2, object>> builder)
        {
            this.AddGroupBy(builder);
            return this;
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T1, T2, TDestination> Join<T, TDestination>(Expression<Func<T, TDestination, bool>> joinExpression, JoinType type = JoinType.Default) where T : T1
        {
            this.AddJoin<T, TDestination>(joinExpression, type);
            return SelectTyped<T1, T2, TDestination>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T1, T2, TDestination> Join<TDestination>(Expression<Func<T2, TDestination, bool>> joinExpression, JoinType type = JoinType.Default)
        {
            this.AddJoin<T2, TDestination>(joinExpression, type);
            return SelectTyped<T1, T2, TDestination>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T1, T2, TDestination> Join<TDestination>(Action<ISelectWithAlias> query, Expression<Func<T2, TDestination, bool>> joinExpression, JoinType type = JoinType.Default)
        {
            this.AddJoin<T2, TDestination>(query, joinExpression, type);
            return SelectTyped<T1, T2, TDestination>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public IOrderByThenTyped<T1, T2> OrderBy(Expression<Func<T1, T2, object>> builder, SortOrder order = SortOrder.Ascending)
        {
            this.AddOrderBy(builder, order);
            return this;
        }

        /// <inheritdoc/>
        public IExecute<TResult> SelectAll<TResult>(Expression<Func<TResult, object>> expression)
        {
            base.AddAllColumns<TResult>(expression);
            return Execute<TResult, ISelectQueryContext>.Create(this.query, this.context);
        }

        /// <inheritdoc/>
        public IGroupByThenTyped<T1, T2> ThenBy(Expression<Func<T1, T2, object>> builder)
            => GroupBy(builder);

        /// <inheritdoc/>
        public IOrderByThenTyped<T1, T2> ThenBy(Expression<Func<T1, T2, object>> builder, SortOrder order = SortOrder.Ascending)
            => OrderBy(builder, order);

        /// <inheritdoc/>
        public IGroupByTyped<T1, T2> Where(Expression<Func<T1, T2, bool>> builder)
        {
            this.AddWhere(builder);
            return this;
        }

        /// <inheritdoc/>
        IExecuteDynamic ISelectColumnTyped<T1, T2>.Select(Expression<Func<T1, T2, dynamic>> expression)
        {
            this.AddColumns(expression);
            return this;
        }

        /// <inheritdoc/>
        IExecute<TResult> ISelectColumnTyped<T1, T2>.Select<TResult>(Expression<Func<T1, T2, TResult>> expression)
        {
            this.AddColumns(expression);
            return Execute<TResult, ISelectQueryContext>.Create(this.query, this.context);
        }
    }
}

