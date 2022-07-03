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

        public ISelectFromTyped<T> Column(Expression<Func<T, object>> expression)
        {
            this.AddColumn<T>(expression);
            return this;
        }

        public ISelectFromTyped<T> Columns(Expression<Func<T, dynamic>> selectExpression)
        {
            this.AddColumns(selectExpression);
            return this;
        }

        public void Dispose() { }

        public IGroupByThenTyped<T> GroupBy(Expression<Func<T, object>> builder)
        {
            this.AddGroupBy(builder);
            return this;
        }

        public ISelectFromTyped<T, TDestination> Join<TDestination>(Expression<Func<T, TDestination, bool>> joinExpression, JoinType type = JoinType.Default)
        {
            this.AddJoin<T, TDestination>(joinExpression, type);
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

        IGroupByThenTyped<T> IGroupByThenTyped<T>.ThenBy(Expression<Func<T, object>> builder)
            => GroupBy(builder);
    }
}

