using System.Collections.Generic;
using decaf.common;

namespace decaf.state;

public interface IInsertQueryContext : IQueryContext
{
	/// <summary>
	/// Gets the <see cref="ITableTarget"/> that this insert targets.
	/// </summary>
	ITableTarget? Target { get; }

	/// <summary>
	/// Gets the set of columns being inserted.
	/// </summary>
	IReadOnlyCollection<Column> Columns { get; }

	/// <summary>
	/// Gets the <see cref="IInsertValuesSource"/> for this context, if it
	/// has been specified. Which is only when performing an update from a query.
	/// </summary>
	IInsertValuesSource? Source { get; }

	/// <summary>
	/// Gets the set of outputs for this context.
	/// </summary>
	IReadOnlyCollection<Output> Outputs { get; }

	/// <summary>
	/// Set the target for this context.
	/// </summary>
	/// <param name="target">The target to insert rows into.</param>
	void Into(ITableTarget target);

	/// <summary>
	/// Add a column to the set to be inserted.
	/// </summary>
	/// <param name="column">The column to add.</param>
	void Column(Column column);

	/// <summary>
	/// Set the source of the insert statement to a query rather than static values.
	/// </summary>
	/// <param name="source"></param>
	void From(IInsertValuesSource source);

	/// <summary>
	/// Add one or more static rows to be inserted.
	/// </summary>
	/// <param name="value">The rows to be inserted.</param>
	void Value(object[] value);

	/// <summary>
	/// Add a column to be output by the query.
	/// </summary>
	/// <param name="output">The column to be output.</param>
	void Output(Output output);
}