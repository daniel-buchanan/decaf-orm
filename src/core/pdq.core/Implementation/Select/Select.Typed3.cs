using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
    internal class SelectTyped<T1, T2, T3>
        : SelectTyped,
        ISelectFromTyped<T1, T2, T3>,
        IGroupByTyped<T1, T2, T3>,
        IGroupByThenTyped<T1, T2, T3>,
        IOrderByThenTyped<T1, T2, T3>
	{
        private SelectTyped(
            ISelectQueryContext context,
            IQueryContainerInternal query)
            : base(context, query)
        {
        }

        public static SelectTyped<T1, T2, T3> Create(
            ISelectQueryContext context,
            IQueryContainerInternal query)
            => new SelectTyped<T1, T2, T3>(context, query);

        /// <inheritdoc/>
        public ISelectFromTyped<T1, T2, T3, T4> From<T4>()
        {
            AddFrom<T4>();
            return SelectTyped<T1, T2, T3, T4>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T1, T2, T3, T4> From<T4>(Expression<Func<T4, T4>> expression)
        {
            AddFrom<T4>(expression);
            return SelectTyped<T1, T2, T3, T4>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public IGroupByThenTyped<T1, T2, T3> GroupBy(Expression<Func<T1, T2, T3, object>> builder)
        {
            this.AddGroupBy(builder);
            return this;
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T1, T2, T3, TDestination> Join<T, TDestination>(Expression<Func<T, TDestination, bool>> joinExpression, JoinType type = JoinType.Default) where T : T1
        {
            this.AddJoin<T, TDestination>(joinExpression, type);
            return SelectTyped<T1, T2, T3, TDestination>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T1, T2, T3, TDestination> Join<TDestination>(Expression<Func<T3, TDestination, bool>> joinExpression, JoinType type = JoinType.Default)
        {
            this.AddJoin<T3, TDestination>(joinExpression, type);
            return SelectTyped<T1, T2, T3, TDestination>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T1, T2, T3, TDestination> Join<TDestination>(Action<ISelectWithAlias> query, Expression<Func<T3, TDestination, bool>> joinExpression, JoinType type = JoinType.Default)
        {
            this.AddJoin<T3, TDestination>(query, joinExpression, type);
            return SelectTyped<T1, T2, T3, TDestination>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public IOrderByThenTyped<T1, T2, T3> OrderBy(Expression<Func<T1, T2, T3, object>> builder, SortOrder order = SortOrder.Ascending)
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
        public IGroupByThenTyped<T1, T2, T3> ThenBy(Expression<Func<T1, T2, T3, object>> builder)
            => GroupBy(builder);

        /// <inheritdoc/>
        public IOrderByThenTyped<T1, T2, T3> ThenBy(Expression<Func<T1, T2, T3, object>> builder, SortOrder order = SortOrder.Ascending)
            => OrderBy(builder, order);

        /// <inheritdoc/>
        public IGroupByTyped<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> builder)
        {
            this.AddWhere(builder);
            return this;
        }

        /// <inheritdoc/>
        IExecuteDynamic ISelectColumnTyped<T1, T2, T3>.Select(Expression<Func<T1, T2, T3, dynamic>> expression)
        {
            this.AddColumns(expression);
            return this;
        }

        /// <inheritdoc/>
        IExecute<TResult> ISelectColumnTyped<T1, T2, T3>.Select<TResult>(Expression<Func<T1, T2, T3, TResult>> expression)
        {
            this.AddColumns(expression);
            return Execute<TResult, ISelectQueryContext>.Create(this.query, this.context);
        }
    }
}

