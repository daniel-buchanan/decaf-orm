using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;
using pdq.state.Utilities;

namespace pdq.Implementation
{
    internal class Select<T1, T2, T3>
        : SelectTypedBase,
        ISelectFromTyped<T1, T2, T3>,
        ISelectColumnTyped<T1, T2, T3>,
        IGroupByTyped<T1, T2, T3>,
        IGroupByThenTyped<T1, T2, T3>,
        IOrderByThenTyped<T1, T2, T3>
	{
        private Select(
            ISelectQueryContext context,
            IQuery query)
            : base(context, query)
        {
        }

        public static Select<T1, T2, T3> Create(
            ISelectQueryContext context,
            IQuery query)
            => new Select<T1, T2, T3>(context, query);

        public void Dispose() { }

        public IGroupByThenTyped<T1, T2, T3> GroupBy(Expression<Func<T1, T2, T3, object>> builder)
        {
            this.AddGroupBy(builder);
            return this;
        }

        public ISelectFromTyped<T1, T2, T3, TDestination> Join<T, TDestination>(Expression<Func<T, TDestination, bool>> joinExpression, JoinType type = JoinType.Default) where T : T1
        {
            this.AddJoin<T, TDestination>(joinExpression, type);
            return Select<T1, T2, T3, TDestination>.Create(this.context, this.query);
        }

        public ISelectFromTyped<T1, T2, T3, TDestination> Join<TDestination>(Expression<Func<T3, TDestination, bool>> joinExpression, JoinType type = JoinType.Default)
        {
            this.AddJoin<T3, TDestination>(joinExpression, type);
            return Select<T1, T2, T3, TDestination>.Create(this.context, this.query);
        }

        public IOrderByThenTyped<T1, T2, T3> OrderBy(Expression<Func<T1, T2, T3, object>> builder, SortOrder order = SortOrder.Ascending)
        {
            this.AddOrderBy(builder, order);
            return this;
        }

        public IGroupByThenTyped<T1, T2, T3> ThenBy(Expression<Func<T1, T2, T3, object>> builder)
            => GroupBy(builder);

        public IOrderByThenTyped<T1, T2, T3> ThenBy(Expression<Func<T1, T2, T3, object>> builder, SortOrder order = SortOrder.Ascending)
            => OrderBy(builder, order);

        public IGroupByTyped<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> builder)
        {
            this.AddWhere(builder);
            return this;
        }

        IExecuteDynamic ISelectColumnTyped<T1, T2, T3>.Select(Expression<Func<T1, T2, T3, dynamic>> expression)
        {
            this.AddColumns(expression);
            return ExecuteDynamic.Create(this.query);
        }

        IExecute<TResult> ISelectColumnTyped<T1, T2, T3>.Select<TResult>(Expression<Func<T1, T2, T3, TResult>> expression)
        {
            this.AddColumns(expression);
            return Execute<TResult>.Create(this.query);
        }
    }
}

