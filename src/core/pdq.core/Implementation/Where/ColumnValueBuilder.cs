using System;
using System.Collections.Generic;
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
        public void EndsWith<T>(T value) => AddClause(common.EqualityOperator.EndsWith, value);

        /// <inheritdoc />
        public void EqualTo<T>(T value) => AddClause(common.EqualityOperator.Equals, value);

        /// <inheritdoc />
        public void GreaterThan(int value) => AddClause(common.EqualityOperator.GreaterThan, value);

        /// <inheritdoc />
        public void GreaterThan(uint value) => AddClause(common.EqualityOperator.GreaterThan, value);

        /// <inheritdoc />
        public void GreaterThan(short value) => AddClause(common.EqualityOperator.GreaterThan, value);

        /// <inheritdoc />
        public void GreaterThan(double value) => AddClause(common.EqualityOperator.GreaterThan, value);

        /// <inheritdoc />
        public void GreaterThan(long value) => AddClause(common.EqualityOperator.GreaterThan, value);

        /// <inheritdoc />
        public void GreaterThan(DateTime value) => AddClause(common.EqualityOperator.GreaterThan, value);

        /// <inheritdoc />
        public void GreaterThanOrEqualTo(int value) => AddClause(common.EqualityOperator.GreaterThanOrEqualTo, value);

        /// <inheritdoc />
        public void GreaterThanOrEqualTo(uint value) => AddClause(common.EqualityOperator.GreaterThanOrEqualTo, value);

        /// <inheritdoc />
        public void GreaterThanOrEqualTo(short value) => AddClause(common.EqualityOperator.GreaterThanOrEqualTo, value);

        /// <inheritdoc />
        public void GreaterThanOrEqualTo(double value) => AddClause(common.EqualityOperator.GreaterThanOrEqualTo, value);

        /// <inheritdoc />
        public void GreaterThanOrEqualTo(long value) => AddClause(common.EqualityOperator.GreaterThanOrEqualTo, value);

        /// <inheritdoc />
        public void GreaterThanOrEqualTo(DateTime value) => AddClause(common.EqualityOperator.GreaterThanOrEqualTo, value);

        /// <inheritdoc />
        public void In<T>(params T[] values) => AddClause(values);

        /// <inheritdoc />
        public void In<T>(IEnumerable<T> values) => AddClause(values);

        /// <inheritdoc />
        public void IsBetween<T>(T start, T end)
        {
            var between = Between.Values(this.column, start, end);
            if (this.notEquals) this.builder.AddClause(Not.This(between));
            else this.builder.AddClause(between);
        }

        /// <inheritdoc />
        public void LessThan(int value) => AddClause(common.EqualityOperator.LessThan, value);

        /// <inheritdoc />
        public void LessThan(uint value) => AddClause(common.EqualityOperator.LessThan, value);

        /// <inheritdoc />
        public void LessThan(short value) => AddClause(common.EqualityOperator.LessThan, value);

        /// <inheritdoc />
        public void LessThan(double value) => AddClause(common.EqualityOperator.LessThan, value);

        /// <inheritdoc />
        public void LessThan(long value) => AddClause(common.EqualityOperator.LessThan, value);

        /// <inheritdoc />
        public void LessThan(DateTime value) => AddClause(common.EqualityOperator.LessThan, value);

        /// <inheritdoc />
        public void LessThanOrEqualTo(int value) => AddClause(common.EqualityOperator.LessThanOrEqualTo, value);

        /// <inheritdoc />
        public void LessThanOrEqualTo(uint value) => AddClause(common.EqualityOperator.LessThanOrEqualTo, value);

        /// <inheritdoc />
        public void LessThanOrEqualTo(short value) => AddClause(common.EqualityOperator.LessThanOrEqualTo, value);

        /// <inheritdoc />
        public void LessThanOrEqualTo(double value) => AddClause(common.EqualityOperator.LessThanOrEqualTo, value);

        /// <inheritdoc />
        public void LessThanOrEqualTo(long value) => AddClause(common.EqualityOperator.LessThanOrEqualTo, value);

        /// <inheritdoc />
        public void LessThanOrEqualTo(DateTime value) => AddClause(common.EqualityOperator.LessThanOrEqualTo, value);

        /// <inheritdoc />
        public void Like<T>(T value) => AddClause(common.EqualityOperator.Like, value);

        /// <inheritdoc />
        public void StartsWith<T>(T value) => AddClause(common.EqualityOperator.StartsWith, value);

        private void AddClause<T>(common.EqualityOperator op, T value)
        {
            var col = new Column<T>(this.column, op, value);
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

