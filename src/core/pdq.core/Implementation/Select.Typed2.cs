using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
    internal class Select<T1, T2>
        : SelectTypedBase,
        ISelectFromTyped<T1, T2>,
        ISelectColumnTyped<T1, T2>,
        IGroupByTyped<T1, T2>,
        IGroupByThenTyped<T1, T2>,
        IOrderByThenTyped<T1, T2>
	{
        private Select(
            ISelectQueryContext context,
            IQuery query)
            : base(context, query)
        {
        }

        public static Select<T1, T2> Create(
            ISelectQueryContext context,
            IQuery query)
            => new Select<T1, T2>(context, query);

        public ISelectFromTyped<T1, T2> Column(Expression<Func<T2, object>> selectExpression)
        {
            this.AddColumn<T2>(selectExpression);
            return this;
        }

        public ISelectFromTyped<T1, T2> Columns(Expression<Func<T1, T2, dynamic>> selectExpression)
        {
            this.AddColumns(selectExpression);
            return this;
        }

        public void Dispose() { }

        public IGroupByThenTyped<T1, T2> GroupBy(Expression<Func<T1, T2, object>> builder)
        {
            this.GroupBy(builder);
            return this;
        }

        public ISelectFromTyped<T1, T2, TDestination> Join<T, TDestination>(Expression<Func<T, TDestination, bool>> joinExpression, JoinType type = JoinType.Default) where T : T1
        {
            this.AddJoin<T, TDestination>(joinExpression, type);
            return Select<T1, T2, TDestination>.Create(this.context, this.query);
        }

        public ISelectFromTyped<T1, T2, TDestination> Join<TDestination>(Expression<Func<T2, TDestination, bool>> joinExpression, JoinType type = JoinType.Default)
        {
            this.AddJoin<T2, TDestination>(joinExpression, type);
            return Select<T1, T2, TDestination>.Create(this.context, this.query);
        }

        public IOrderByThenTyped<T1, T2> OrderBy(Expression<Func<T1, T2, object>> builder, SortOrder order = SortOrder.Ascending)
        {
            this.AddOrderBy(builder, order);
            return this;
        }

        public IGroupByThenTyped<T1, T2> ThenBy(Expression<Func<T1, T2, object>> builder)
            => GroupBy(builder);


        public IOrderByThenTyped<T1, T2> ThenBy(Expression<Func<T1, T2, object>> builder, SortOrder order = SortOrder.Ascending)
            => OrderBy(builder, order);


        public IGroupByTyped<T1, T2> Where(Expression<Func<T1, T2, bool>> builder)
        {
            this.AddWhere(builder);
            return this;
        }
    }
}

