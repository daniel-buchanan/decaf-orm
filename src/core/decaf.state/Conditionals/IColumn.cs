using System;
using decaf.common;
using Newtonsoft.Json;

namespace decaf.state.Conditionals;

public interface IColumn : IWhere
{
	Type ValueType { get; }

	state.Column Details { get; }

	EqualityOperator EqualityOperator { get; }

	IValueFunction? ValueFunction { get; }

	[JsonIgnore]
	object? Value { get; }
}