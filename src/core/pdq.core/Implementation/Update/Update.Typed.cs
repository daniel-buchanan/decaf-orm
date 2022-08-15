using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
    internal class UpdateTyped<T> :
        UpdateBase,
        IUpdateTable<T>,
        IUpdateSet<T>,
        IUpdateSetFromQuery<T>
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
        public IUpdateSetFromQuery<T> From(Action<ISelectWithAlias> query)
        {
            base.FromQuery(query);
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet<T> Output(Expression<Func<T, object>> column)
        {
            var columnToOutput = GetDestinationColumn(column);
            this.context.AddOutput(state.Output.Create(columnToOutput, OutputSources.Updated));
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
        public IUpdateSetFromQuery<T> Set(Expression<Func<T, object>> columnToUpdate, string sourceColumnName)
        {
            var destinationColumn = GetDestinationColumn(columnToUpdate);
            var querySource = this.context.Source;
            var sourceColumn = state.Column.Create(sourceColumnName, querySource);
            var source = state.ValueSources.Update.QueryValueSource.Create(destinationColumn, sourceColumn, querySource);
            this.context.Set(source);
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet<T> Where(Expression<Func<T, bool>> expression)
        {
            var where = this.context.Helpers().ParseWhere(expression);
            this.context.Where(where);
            return this;
        }

        /// <inheritdoc/>
        IUpdateSetFromQuery<T> IUpdateSetFromQuery<T>.Output(Expression<Func<T, object>> column)
        {
            this.Output(column);
            return this;
        }

        /// <inheritdoc/>
        IUpdateSetFromQuery<T> IUpdateSetFromQuery<T>.Where(Expression<Func<T, bool>> expression)
        {
            this.Where(expression);
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

