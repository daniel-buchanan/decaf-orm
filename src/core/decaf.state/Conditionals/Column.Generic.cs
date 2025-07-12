using System;
using decaf.common;
using Newtonsoft.Json;

namespace decaf.state.Conditionals;

public class Column<T> : Where, IColumn
{
	public Column(
		state.Column column,
		EqualityOperator op,
		T value)
	{
		ValueFunction = null;
		Details = column;
		EqualityOperator = op;
		Value = value;
		ValueType = typeof(T);
	}

	public Column(
		state.Column column,
		EqualityOperator op,
		IValueFunction? valueFunction,
		T value)
	{
		ValueFunction = valueFunction;
		Details = column;
		EqualityOperator = op;
		Value = value;
		ValueType = typeof(T);
	}

	public Type ValueType { get; private set; }

	public state.Column Details { get; private set; }

	public EqualityOperator EqualityOperator { get; private set; }

	public IValueFunction? ValueFunction { get; private set; }

	[JsonIgnore]
	public T Value { get; private set; }

	[JsonIgnore]
	object? IColumn.Value => Value;
}