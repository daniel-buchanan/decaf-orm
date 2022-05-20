using pdq.core.common;

namespace pdq.core.state.Conditionals
{
	public class Column : Where
	{
		private Column(state.Column left, EqualityOperator op, state.Column right)
			: base()
		{
			Left = left;
			Operator = op;
			Right = right;
		}

		public state.Column Left { get; private set; }

		public EqualityOperator Operator { get; private set; }

		public state.Column Right { get; private set; }

		public static Column Match(state.Column left, EqualityOperator op, state.Column right)
        {
			return new Column(left, op, right);
        }

		public static Column<T> ValueMatch<T>(state.Column column, EqualityOperator op, T value)
        {
			return new Column<T>(column, op, value);
        }

		public static Column<T> Equals<T>(state.Column column, T value)
        {
			return new Column<T>(column, EqualityOperator.Equals, value);
        }

		public static Column<T> NotEquals<T>(state.Column column, T value)
		{
			return new Column<T>(column, EqualityOperator.NotEquals, value);
		}

		public static Column<T> LessThan<T>(state.Column column, T value)
		{
			return new Column<T>(column, EqualityOperator.LessThan, value);
		}

		public static Column<T> LessThanOrEqualTo<T>(state.Column column, T value)
		{
			return new Column<T>(column, EqualityOperator.LessThanOrEqualTo, value);
		}

		public static Column<T> GreaterThan<T>(state.Column column, T value)
		{
			return new Column<T>(column, EqualityOperator.GreaterThan, value);
		}

		public static Column<T> GreaterThanOrEqualTo<T>(state.Column column, T value)
		{
			return new Column<T>(column, EqualityOperator.GreaterThanOrEqualTo, value);
		}
	}

	public class Column<T> : Where
    {
		internal Column(state.Column column, EqualityOperator op, T value)
			: base()
		{
			Details = column;
			Operator = op;
			Value = value;
			ValueType = typeof(T);
		}

		public Type ValueType { get; private set; }

		public state.Column Details { get; private set; }

		public EqualityOperator Operator { get; private set; }

		public T Value { get; private set; }
	}
}

