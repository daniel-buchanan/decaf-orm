using System;

namespace decaf.state.ValueSources.Update;

public class StaticValueSource : IUpdateValueSource
{
	private StaticValueSource(
		Column column,
		Type type,
		object? value)
	{
		Column = column;
		ValueType = type;
		Value = value;
	}

	public Type ValueType { get; private set; }

	public Column Column { get; private set; }

	public object? Value { get; private set; }

	public T? GetValue<T>()
	{
		if (Value is null) return default;

		var convertedValue = Convert.ChangeType(Value, typeof(T));
		return (T)convertedValue;
	}

	public static StaticValueSource Create(
		Column column,
		Type type,
		object value)
		=> new(column, type, value);

	public static StaticValueSource Create<T>(Column column, T value)
		=> new(column, typeof(T), value);
}