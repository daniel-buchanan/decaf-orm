using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
    internal class SelectTyped<T>
        : SelectTyped,
        ISelectFromTyped<T>,
        IGroupByTyped<T>,
        IGroupByThenTyped<T>,
        IOrderByThenTyped<T>
	{
        private SelectTyped(
            ISelectQueryContext context,
            IQueryContainerInternal query)
            : base(context, query)
        {
        }

        public static SelectTyped<T> Create(
            ISelectQueryContext context,
            IQueryContainerInternal query)
            => new SelectTyped<T>(context, query);

        /// <inheritdoc/>
        public ISelectFromTyped<T, T1> From<T1>()
        {
            AddFrom<T1>();
            return SelectTyped<T, T1>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T, T1> From<T1>(Expression<Func<T1, T1>> expression)
        {
            AddFrom<T1>(expression);
            return SelectTyped<T, T1>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public IGroupByThenTyped<T> GroupBy(Expression<Func<T, object>> builder)
        {
            this.AddGroupBy(builder);
            return this;
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T, TDestination> Join<TDestination>(
            Expression<Func<T, TDestination, bool>> joinExpression,
            JoinType type = JoinType.Default)
        {
            this.AddJoin<T, TDestination>(joinExpression, type);
            return SelectTyped<T, TDestination>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public ISelectFromTyped<T, TDestination> Join<TDestination>(
            Action<ISelectWithAlias> query,
            Expression<Func<T, TDestination, bool>> joinExpression,
            JoinType type = JoinType.Default)
        {
            this.AddJoin<T, TDestination>(query, joinExpression, type);
            return SelectTyped<T, TDestination>.Create(this.context, this.query);
        }

        public ISelectFromTyped<T, TDestination> Join<TDestination>(Func<ISelectWithAlias, Expression<Func<T, TDestination, bool>>> query, JoinType type = JoinType.Default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IOrderByThenTyped<T> OrderBy(Expression<Func<T, object>> builder, SortOrder order = SortOrder.Ascending)
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
        public IOrderByThenTyped<T> ThenBy(Expression<Func<T, object>> builder, SortOrder order = SortOrder.Ascending)
            => OrderBy(builder, order);

        /// <inheritdoc/>
        public IGroupByTyped<T> Where(Expression<Func<T, bool>> builder)
        {
            this.AddWhere(builder);
            return this;
        }

        /// <inheritdoc/>
        IExecuteDynamic ISelectColumnTyped<T>.Select(Expression<Func<T, dynamic>> expression)
        {
            this.AddColumns(expression);
            return this;
        }

        /// <inheritdoc/>
        IExecute<TResult> ISelectColumnTyped<T>.Select<TResult>(Expression<Func<T, TResult>> expression)
        {
            this.AddColumns(expression);
            return Execute<TResult, ISelectQueryContext>.Create(this.query, this.context);
        }

        /// <inheritdoc/>
        IGroupByThenTyped<T> IGroupByThenTyped<T>.ThenBy(Expression<Func<T, object>> builder)
            => GroupBy(builder);
    }
}

