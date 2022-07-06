using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
    internal class Select<T>
        : SelectTypedBase,
        ISelectFromTyped<T>,
        ISelectColumnTyped<T>,
        IGroupByTyped<T>,
        IGroupByThenTyped<T>,
        IOrderByThenTyped<T>
	{
        private Select(
            ISelectQueryContext context,
            IQuery query)
            : base(context, query)
        {
        }

        public static Select<T> Create(
            ISelectQueryContext context,
            IQuery query)
            => new Select<T>(context, query);

        public void Dispose() { }

        public IGroupByThenTyped<T> GroupBy(Expression<Func<T, object>> builder)
        {
            this.AddGroupBy(builder);
            return this;
        }

        public ISelectFromTyped<T, TDestination> Join<TDestination>(
            Expression<Func<T, TDestination, bool>> joinExpression,
            JoinType type = JoinType.Default)
        {
            this.AddJoin<T, TDestination>(joinExpression, type);
            return Select<T, TDestination>.Create(this.context, this.query);
        }

        public ISelectFromTyped<T, TDestination> Join<TDestination>(
            Action<ISelectWithAlias> query,
            Expression<Func<T, TDestination, bool>> joinExpression,
            JoinType type = JoinType.Default)
        {
            this.AddJoin<T, TDestination>(query, joinExpression, type);
            return Select<T, TDestination>.Create(this.context, this.query);
        }

        public IOrderByThenTyped<T> OrderBy(Expression<Func<T, object>> builder, SortOrder order = SortOrder.Ascending)
        {
            this.AddOrderBy(builder, order);
            return this;
        }

        public IOrderByThenTyped<T> ThenBy(Expression<Func<T, object>> builder, SortOrder order = SortOrder.Ascending)
            => OrderBy(builder, order);

        public IGroupByTyped<T> Where(Expression<Func<T, bool>> builder)
        {
            this.AddWhere(builder);
            return this;
        }

        IExecuteDynamic ISelectColumnTyped<T>.Select(Expression<Func<T, dynamic>> expression)
        {
            this.AddColumns(expression);
            return ExecuteDynamic.Create(this.query);
        }

        IExecute<TResult> ISelectColumnTyped<T>.Select<TResult>(Expression<Func<T, TResult>> expression)
        {
            this.AddColumns(expression);
            return Execute<TResult>.Create(this.query);
        }

        IGroupByThenTyped<T> IGroupByThenTyped<T>.ThenBy(Expression<Func<T, object>> builder)
            => GroupBy(builder);
    }
}

