using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
    internal class SelectTyped<T1, T2, T3, T4, T5>
        : SelectTyped,
        ISelectFromTyped<T1, T2, T3, T4, T5>,
        IGroupByTyped<T1, T2, T3, T4, T5>,
        IGroupByThenTyped<T1, T2, T3, T4, T5>,
        IOrderByThenTyped<T1, T2, T3, T4, T5>
    {
        private SelectTyped(
            ISelectQueryContext context,
            IQueryInternal query)
            : base(context, query)
        {
        }

        public static SelectTyped<T1, T2, T3, T4, T5> Create(
            ISelectQueryContext context,
            IQueryInternal query)
            => new SelectTyped<T1, T2, T3, T4, T5>(context, query);

        /// <inheritdoc/>
        public IGroupByThenTyped<T1, T2, T3, T4, T5> GroupBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder)
        {
            this.AddGroupBy(builder);
            return this;
        }

        /// <inheritdoc/>
        public IOrderByThenTyped<T1, T2, T3, T4, T5> OrderBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder, SortOrder order = SortOrder.Ascending)
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
        public IGroupByThenTyped<T1, T2, T3, T4, T5> ThenBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder)
            => GroupBy(builder);

        /// <inheritdoc/>
        public IOrderByThenTyped<T1, T2, T3, T4, T5> ThenBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder, SortOrder order = SortOrder.Ascending)
            => OrderBy(builder, order);

        /// <inheritdoc/>
        public IGroupByTyped<T1, T2, T3, T4, T5> Where(Expression<Func<T1, T2, T3, T4, T5, bool>> builder)
        {
            this.AddWhere(builder);
            return this;
        }

        /// <inheritdoc/>
        IExecuteDynamic ISelectColumnTyped<T1, T2, T3, T4, T5>.Select(Expression<Func<T1, T2, T3, T4, T5, dynamic>> expression)
        {
            this.AddColumns(expression);
            return this;
        }

        /// <inheritdoc/>
        IExecute<TResult> ISelectColumnTyped<T1, T2, T3, T4, T5>.Select<TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> expression)
        {
            this.AddColumns(expression);
            return Execute<TResult, ISelectQueryContext>.Create(this.query, this.context);
        }
    }
}

