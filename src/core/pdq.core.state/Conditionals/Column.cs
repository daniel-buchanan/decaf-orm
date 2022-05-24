using System;
using pdq.core.common;

namespace pdq.core.state.Conditionals
{
	public class Column : Where
	{
		private Column(state.Column left, EqualityOperator op, state.Column right)
			: base()
		{
			Left = left;
			EqualityOperator = op;
			Right = right;
		}

		private Column(state.Column left, EqualityOperator op, IValueFunction valueFunction, state.Column right)
			: this(left, op, right)
        {
			RightFunction = valueFunction;
        }

		public state.Column Left { get; private set; }

		public EqualityOperator EqualityOperator { get; private set; }

		public IValueFunction? RightFunction { get; private set; }

		public state.Column Right { get; private set; }

        public static Column Match(state.Column left, EqualityOperator op, state.Column right)
			=> new Column(left, op, right);

		public static Column Match(state.Column left, EqualityOperator op, IValueFunction valueFunction, state.Column right)
			=> new Column(left, op, valueFunction, right);

        public static Column<T> ValueMatch<T>(state.Column column, EqualityOperator op, T value, IValueFunction valueFunction = null)
			=> new Column<T>(column, op, valueFunction, value);

        public static Column<T> Equals<T>(state.Column column, T value, IValueFunction valueFunction = null)
			=> new Column<T>(column, EqualityOperator.Equals, valueFunction, value);

        public static Column<T> NotEquals<T>(state.Column column, T value, IValueFunction valueFunction = null)
			=> new Column<T>(column, EqualityOperator.NotEquals, valueFunction, value);

        public static Column<T> LessThan<T>(state.Column column, T value, IValueFunction valueFunction = null)
			=> new Column<T>(column, EqualityOperator.LessThan, valueFunction, value);

        public static Column<T> LessThanOrEqualTo<T>(state.Column column, T value, IValueFunction valueFunction = null)
			=> new Column<T>(column, EqualityOperator.LessThanOrEqualTo, valueFunction, value);

        public static Column<T> GreaterThan<T>(state.Column column, T value, IValueFunction valueFunction = null)
			=> new Column<T>(column, EqualityOperator.GreaterThan, valueFunction, value);

        public static Column<T> GreaterThanOrEqualTo<T>(state.Column column, T value, IValueFunction valueFunction = null)
			=> new Column<T>(column, EqualityOperator.GreaterThanOrEqualTo, valueFunction, value);

		public static Column<T> Like<T>(state.Column column, T value, IValueFunction valueFunction = null)
			=> new Column<T>(column, EqualityOperator.Like, valueFunction, value);

		public static Column<T> NotLike<T>(state.Column column, T value, IValueFunction valueFunction = null)
			=> new Column<T>(column, EqualityOperator.NotLike, valueFunction, value);
	}
}

