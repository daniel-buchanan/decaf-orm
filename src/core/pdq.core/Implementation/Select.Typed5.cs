﻿using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
    internal class Select<T1, T2, T3, T4, T5>
        : SelectTypedBase,
        ISelectFromTyped<T1, T2, T3, T4, T5>,
        ISelectColumnTyped<T1, T2, T3, T4, T5>,
        IGroupByTyped<T1, T2, T3, T4, T5>,
        IGroupByThenTyped<T1, T2, T3, T4, T5>,
        IOrderByThenTyped<T1, T2, T3, T4, T5>
    {
        private Select(
            ISelectQueryContext context,
            IQuery query)
            : base(context, query)
        {
        }

        public static Select<T1, T2, T3, T4, T5> Create(
            ISelectQueryContext context,
            IQuery query)
            => new Select<T1, T2, T3, T4, T5>(context, query);

        public void Dispose() { }

        public IGroupByThenTyped<T1, T2, T3, T4, T5> GroupBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder)
        {
            this.AddGroupBy(builder);
            return this;
        }

        public IOrderByThenTyped<T1, T2, T3, T4, T5> OrderBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder, SortOrder order = SortOrder.Ascending)
        {
            this.AddOrderBy(builder, order);
            return this;
        }

        public IGroupByThenTyped<T1, T2, T3, T4, T5> ThenBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder)
            => GroupBy(builder);

        public IOrderByThenTyped<T1, T2, T3, T4, T5> ThenBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder, SortOrder order = SortOrder.Ascending)
            => OrderBy(builder, order);

        public IGroupByTyped<T1, T2, T3, T4, T5> Where(Expression<Func<T1, T2, T3, T4, T5, bool>> builder)
        {
            this.AddWhere(builder);
            return this;
        }

        IExecuteDynamic ISelectColumnTyped<T1, T2, T3, T4, T5>.Select(Expression<Func<T1, T2, T3, T4, T5, dynamic>> expression)
        {
            this.AddColumns(expression);
            return ExecuteDynamic.Create(this.query);
        }

        IExecute<TResult> ISelectColumnTyped<T1, T2, T3, T4, T5>.Select<TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> expression)
        {
            this.AddColumns(expression);
            return Execute<TResult>.Create(this.query);
        }
    }
}
