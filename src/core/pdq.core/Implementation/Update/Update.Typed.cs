using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
    internal class UpdateTyped<T> :
        UpdateBase,
        IUpdateTable<T>,
        IUpdateSet<T>
    {
        private UpdateTyped(
            IUpdateQueryContext context,
            IQueryInternal query)
            : base(query, context)
        {
        }

        public static UpdateTyped<T> Create(
            IUpdateQueryContext context,
            IQueryInternal query)
            => new UpdateTyped<T>(context, query);

        /// <inheritdoc/>
        public IUpdateSet<T> From(Action<ISelectWithAlias> query)
        {
            base.FromQuery(query);
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet<T> Output(Expression<Func<T, object>> column)
        {
            var internalContext = this.context as IQueryContextInternal;
            var columnName = internalContext.ExpressionHelper.GetName(column);
            this.context.AddOutput(state.Output.Create(columnName, OutputSources.Updated));
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet<T> Set<TValue>(Expression<Func<T, TValue>> column, TValue value)
        {
            var internalContext = this.context as IQueryContextInternal;
            var columnName = internalContext.ExpressionHelper.GetName(column);
            var col = Column.Create(columnName, this.context.Table);
            var source = state.ValueSources.Update.StaticValueSource.Create<TValue>(col, value);
            this.context.Set(source);
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet<T> Set(T values)
        {
            base.SetValues(values);
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet<T> Where(Expression<Func<T, bool>> expression)
        {
            var where = this.context.Helpers().ParseWhere(expression);
            this.context.Where(where);
            return this;
        }
    }
}

