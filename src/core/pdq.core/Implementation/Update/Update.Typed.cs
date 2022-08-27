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
        public IUpdateSetFromQuery<T, TSource> From<TSource>(Action<ISelectWithAlias> query)
        {
            base.FromQuery(query);
            return UpdateSetFromQueryTyped<T, TSource>.Create(this.query, this.context);
        }

        /// <inheritdoc/>
        public IUpdateSet<T> Output(Expression<Func<T, object>> column)
        {
            var columnToOutput = GetDestinationColumn(column);
            this.context.Output(state.Output.Create(columnToOutput, OutputSources.Updated));
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet<T> Set<TValue>(Expression<Func<T, TValue>> column, TValue value)
        {
            var col = GetDestinationColumn(column);
            var source = state.ValueSources.Update.StaticValueSource.Create<TValue>(col, value);
            this.context.Set(source);
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet<T> Set(T values)
        {
            base.SetValues(new[] { values });
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet<T> Set(dynamic values)
        {
            this.SetValues(new[] { values });
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet<T> Where(Expression<Func<T, bool>> expression)
        {
            var where = this.context.Helpers().ParseWhere(expression);
            this.context.Where(where);
            return this;
        }

        private state.Column GetDestinationColumn(Expression expression)
        {
            var internalContext = this.context as IQueryContextInternal;
            var columnName = internalContext.ExpressionHelper.GetName(expression);
            return state.Column.Create(columnName, this.context.Table);
        }
    }
}

