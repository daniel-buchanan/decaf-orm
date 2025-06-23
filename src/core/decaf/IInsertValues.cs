using System;
using System.Collections.Generic;

namespace decaf;

public interface IInsertValues : IExecute
{
    /// <summary>
    /// Add a row to be inserted.
    /// </summary>
    /// <param name="value">The row to be inserted, provided as a dynamic/anonymous object.</param>
    /// <returns>
    /// (FluentApi) Returns an <see cref="IInsertValues"/> which provices the
    /// ability to continue adding rows, or to execute the query.
    /// </returns>
    IInsertValues Value(dynamic value);

    /// <summary>
    /// Add a set of rows to be inserted.
    /// </summary>
    /// <param name="values">
    /// The set of rows to be inserted, provided as a
    /// dynamic/anonymous object enumeration.
    /// </param>
    /// <returns>
    /// (FluentApi) Returns an <see cref="IInsertValues"/> which provices the
    /// ability to continue adding rows, or to execute the query.
    /// </returns>
    IInsertValues Values(IEnumerable<dynamic> values);

    /// <summary>
    /// Provide rows to be inserted from a sub-query.
    /// </summary>
    /// <param name="query">The query that defines the rows to be inserted.</param>
    /// <returns>
    /// (FluentApi) Returns an <see cref="IInsertValues"/> which provices the
    /// ability to continue adding rows, or to execute the query.
    /// </returns>
    IInsertValues From(Action<ISelect> query);

    /// <summary>
    /// Add a column to be returned as output from the query.
    /// </summary>
    /// <param name="column">The name of the column to be returned.</param>
    /// <returns>
    /// (FluentApi) Returns an <see cref="IInsertValues"/> which provides the
    /// ability to continue adding rows, or to execute the query.
    /// </returns>
    IInsertValues Output(string column);
}