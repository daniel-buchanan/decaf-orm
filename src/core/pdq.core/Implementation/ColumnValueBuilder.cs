using System;
using pdq.state.Conditionals;

namespace pdq.Implementation
{
	public class ColumnValueBuilder : IColumnValueBuilder
	{
        private readonly IWhereBuilder builder;
        private readonly state.Column column;
        private readonly bool notEquals;

		private ColumnValueBuilder(
            IWhereBuilder builder,
            state.Column column,
            bool notEquals)
		{
            this.builder = builder;
            this.column = column;
            this.notEquals = notEquals;
		}

        internal static IColumnValueBuilder Create(
            IWhereBuilder builder,
            state.Column column,
            bool notEquals)
            => new ColumnValueBuilder(builder, column, notEquals);

        /// <inheritdoc />
        public void EndsWith<T>(T value) => WrapValue(common.EqualityOperator.EndsWith, value);

        /// <inheritdoc />
        public void EqualTo<T>(T value) => WrapValue(common.EqualityOperator.Equals, value);

        /// <inheritdoc />
        public void GreaterThan(int value) => WrapValue(common.EqualityOperator.GreaterThan, value);

        /// <inheritdoc />
        public void GreaterThan(uint value) => WrapValue(common.EqualityOperator.GreaterThan, value);

        /// <inheritdoc />
        public void GreaterThan(short value) => WrapValue(common.EqualityOperator.GreaterThan, value);

        /// <inheritdoc />
        public void GreaterThan(double value) => WrapValue(common.EqualityOperator.GreaterThan, value);

        /// <inheritdoc />
        public void GreaterThan(long value) => WrapValue(common.EqualityOperator.GreaterThan, value);

        /// <inheritdoc />
        public void GreaterThan(DateTime value) => WrapValue(common.EqualityOperator.GreaterThan, value);

        /// <inheritdoc />
        public void GreaterThanOrEqualTo(int value) => WrapValue(common.EqualityOperator.GreaterThanOrEqualTo, value);

        /// <inheritdoc />
        public void GreaterThanOrEqualTo(uint value) => WrapValue(common.EqualityOperator.GreaterThanOrEqualTo, value);

        /// <inheritdoc />
        public void GreaterThanOrEqualTo(short value) => WrapValue(common.EqualityOperator.GreaterThanOrEqualTo, value);

        /// <inheritdoc />
        public void GreaterThanOrEqualTo(double value) => WrapValue(common.EqualityOperator.GreaterThanOrEqualTo, value);

        /// <inheritdoc />
        public void GreaterThanOrEqualTo(long value) => WrapValue(common.EqualityOperator.GreaterThanOrEqualTo, value);

        /// <inheritdoc />
        public void GreaterThanOrEqualTo(DateTime value) => WrapValue(common.EqualityOperator.GreaterThanOrEqualTo, value);

        /// <inheritdoc />
        public void IsBetween<T>(T start, T end)
        {
            var between = Between.Values(this.column, start, end);
            if (this.notEquals) this.builder.AddClause(Not.This(between));
            else this.builder.AddClause(between);
        }

        /// <inheritdoc />
        public void LessThan(int value) => WrapValue(common.EqualityOperator.LessThan, value);

        /// <inheritdoc />
        public void LessThan(uint value) => WrapValue(common.EqualityOperator.LessThan, value);

        /// <inheritdoc />
        public void LessThan(short value) => WrapValue(common.EqualityOperator.LessThan, value);

        /// <inheritdoc />
        public void LessThan(double value) => WrapValue(common.EqualityOperator.LessThan, value);

        /// <inheritdoc />
        public void LessThan(long value) => WrapValue(common.EqualityOperator.LessThan, value);

        /// <inheritdoc />
        public void LessThan(DateTime value) => WrapValue(common.EqualityOperator.LessThan, value);

        /// <inheritdoc />
        public void LessThanOrEqualTo(int value) => WrapValue(common.EqualityOperator.LessThanOrEqualTo, value);

        /// <inheritdoc />
        public void LessThanOrEqualTo(uint value) => WrapValue(common.EqualityOperator.LessThanOrEqualTo, value);

        /// <inheritdoc />
        public void LessThanOrEqualTo(short value) => WrapValue(common.EqualityOperator.LessThanOrEqualTo, value);

        /// <inheritdoc />
        public void LessThanOrEqualTo(double value) => WrapValue(common.EqualityOperator.LessThanOrEqualTo, value);

        /// <inheritdoc />
        public void LessThanOrEqualTo(long value) => WrapValue(common.EqualityOperator.LessThanOrEqualTo, value);

        /// <inheritdoc />
        public void LessThanOrEqualTo(DateTime value) => WrapValue(common.EqualityOperator.LessThanOrEqualTo, value);

        /// <inheritdoc />
        public void Like<T>(T value) => WrapValue(common.EqualityOperator.Like, value);

        /// <inheritdoc />
        public void StartsWith<T>(T value) => WrapValue(common.EqualityOperator.StartsWith, value);

        private void WrapValue<T>(common.EqualityOperator op, T value)
        {
            var col = new Column<T>(this.column, op, value);
            if (this.notEquals) this.builder.AddClause(Not.This(col));
            else this.builder.AddClause(col);
        }
    }
}

