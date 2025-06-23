using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace decaf;

public interface IInsertValues<T> : IExecute
{
    /// <summary>
    /// Add a row to be inserted.
    /// </summary>
    /// <param name="value">
    /// The row to be inserted, provided as a concrete object
    /// of type <see cref="T"/>.
    /// </param>
    /// <returns>
    /// (FluentApi) Returns an <see cref="IInsertValues{T}"/> which provices the
    /// ability to continue adding rows, or to execute the query.
    /// </returns>
    IInsertValues<T> Value(T value);

    /// <summary>
    /// Add a set of rows to be inserted.
    /// </summary>
    /// <param name="values">
    /// The rows to be inserted, provided as an enumeration of concrete objects
    /// of type <see cref="T"/>.
    /// </param>
    /// <returns>
    /// (FluentApi) Returns an <see cref="IInsertValues{T}"/> which provices the
    /// ability to continue adding rows, or to execute the query.
    /// </returns>
    IInsertValues<T> Values(IEnumerable<T> values);

    /// <summary>
    /// Provide rows to be inserted from a sub-query.
    /// </summary>
    /// <param name="query">The query that defines the rows to be inserted.</param>
    /// <returns>
    /// (FluentApi) Returns an <see cref="IInsertValues{T}"/> which provices the
    /// ability to continue adding rows, or to execute the query.
    /// </returns>
    IInsertValues<T> From(Action<ISelect> query);

    /// <summary>
    /// Add a column to be returned as output from the query.
    /// </summary>
    /// <param name="column">
    /// An expression defining the column to be output.
    /// </param>
    /// <returns>
    /// (FluentApi) Returns an <see cref="IInsertValues{T}"/> which provides the
    /// ability to continue setting column values, perform filtering or
    /// execute the query.
    /// </returns>
    IInsertValues<T> Output(Expression<Func<T, object>> column);
}