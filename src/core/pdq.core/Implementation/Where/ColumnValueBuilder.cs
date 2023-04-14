using System.Collections.Generic;
using pdq.common;
using pdq.common.ValueFunctions;
using pdq.state;
using pdq.state.Conditionals;

namespace pdq.Implementation
{
	public class ColumnValueBuilder : IColumnValueBuilder
	{
        private readonly IQueryContext queryContext;
        private readonly IWhereBuilderInternal builder;
        private readonly state.Column column;
        private readonly bool notEquals;

		private ColumnValueBuilder(
            IQueryContext queryContext,
            IWhereBuilder builder,
            state.Column column,
            bool notEquals)
		{
            this.queryContext = queryContext;
            this.builder = builder as IWhereBuilderInternal;
            this.column = column;
            this.notEquals = notEquals;
		}

        internal static IColumnValueBuilder Create(
            IQueryContext queryContext,
            IWhereBuilder builder,
            state.Column column,
            bool notEquals)
            => new ColumnValueBuilder(queryContext, builder, column, notEquals);

        /// <inheritdoc />
        public void EndsWith<T>(T value)
            => AddClause(common.EqualityOperator.EndsWith, StringEndsWith.Create(value as string), value);

        /// <inheritdoc />
        public IColumnMatchBuilder EqualTo()
            => ColumnMatchBuilder.Create(queryContext, builder, column, notEquals, EqualityOperator.Equals);

        /// <inheritdoc />
        public void EqualTo<T>(T value)
            => AddClause(common.EqualityOperator.Equals, null, value);

        /// <inheritdoc />
        public IColumnMatchBuilder GreaterThan()
            => ColumnMatchBuilder.Create(queryContext, builder, column, notEquals, EqualityOperator.GreaterThan);

        /// <inheritdoc />
        public void GreaterThan<T>(T value)
            => AddClause(common.EqualityOperator.GreaterThan, null, value);

        /// <inheritdoc />
        public IColumnMatchBuilder GreaterThanOrEqualTo()
            => ColumnMatchBuilder.Create(queryContext, builder, column, notEquals, EqualityOperator.GreaterThanOrEqualTo);

        /// <inheritdoc />
        public void GreaterThanOrEqualTo<T>(T value)
            => AddClause(common.EqualityOperator.GreaterThanOrEqualTo, null, value);

        /// <inheritdoc />
        public void In<T>(params T[] values)
            => AddClause(values);

        /// <inheritdoc />
        public void In<T>(IEnumerable<T> values)
            => AddClause(values);

        /// <inheritdoc />
        public void Between<T>(T start, T end)
        {
            var between = state.Conditionals.Between.Values(this.column, start, end);
            if (this.notEquals) this.builder.AddClause(Not.This(between));
            else this.builder.AddClause(between);
        }

        /// <inheritdoc />
        public IColumnMatchBuilder LessThan()
            => ColumnMatchBuilder.Create(queryContext, builder, column, notEquals, EqualityOperator.LessThan);

        /// <inheritdoc />
        public void LessThan<T>(T value)
            => AddClause(common.EqualityOperator.LessThan, null, value);

        /// <inheritdoc />
        public IColumnMatchBuilder LessThanOrEqualTo()
            => ColumnMatchBuilder.Create(queryContext, builder, column, notEquals, EqualityOperator.LessThanOrEqualTo);

        /// <inheritdoc />
        public void LessThanOrEqualTo<T>(T value)
            => AddClause(common.EqualityOperator.LessThanOrEqualTo, null, value);

        /// <inheritdoc />
        public void Like<T>(T value)
            => AddClause(common.EqualityOperator.Like, StringContains.Create(value as string), value);

        /// <inheritdoc />
        public void Null() => AddClause<object>(EqualityOperator.Equals, null, null);

        /// <inheritdoc />
        public void NullOrWhitespace()
        {
            var nullClause = new Column<object>(this.column, EqualityOperator.Equals, null);
            var whitespaceClause = new Column<string>(this.column, EqualityOperator.Equals, Trim.Create(), string.Empty);
            var orClause = Or.Where(nullClause, whitespaceClause);
            if (this.notEquals) this.builder.AddClause(Not.This(orClause));
            else this.builder.AddClause(orClause);
        }

        /// <inheritdoc />
        public void StartsWith<T>(T value)
            => AddClause(common.EqualityOperator.StartsWith, StringStartsWith.Create(value as string), value);

        private void AddClause<T>(common.EqualityOperator op, IValueFunction function, T value)
        {
            var col = new Column<T>(this.column, op, function, value);
            if (this.notEquals) this.builder.AddClause(Not.This(col));
            else this.builder.AddClause(col);
        }

        private void AddClause<T>(IEnumerable<T> values)
        {
            var col = Values.In(this.column, values);
            if (this.notEquals) this.builder.AddClause(Not.This(col));
            else this.builder.AddClause(col);
        }
    }
}

