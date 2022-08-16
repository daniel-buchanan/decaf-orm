using System;
using System.Linq.Expressions;

namespace pdq
{
	public interface IDeleteFrom : IExecute
	{
        /// <summary>
        /// Restrict the rows that are deleted by this query.
        /// </summary>
        /// <param name="builder">
        /// An <see cref="IWhereBuilder"/> that allows for a "where" clause to be
        /// built up.
        /// </param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IDeleteFrom"/> which provides the
        /// ability to filter the result set, and set additional columns as output.
        /// </returns>
        IDeleteFrom Where(Action<IWhereBuilder> builder);

        /// <summary>
        /// Add a column to be returned as output from the query.
        /// </summary>
        /// <param name="column">The name of the column to be returned.</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IDeleteFrom"/> which provides the
        /// ability to filter the result set, and set additional columns as output.
        /// </returns>
        IDeleteFrom Output(string column);
	}

	public interface IDeleteFrom<T> : IExecute
    {
        /// <summary>
        /// Restrict the rows that are deleted by this query.
        /// </summary>
        /// <param name="whereExpression">An expression that defines the filter for the query.</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IDeleteFrom{T}"/> which provides the
        /// ability to filter the result set, and set additional columns as output.
        /// </returns>
        IDeleteFrom<T> Where(Expression<Func<T, bool>> whereExpression);

        /// <summary>
        /// Add a column to be returned as output from the query.
        /// </summary>
        /// <param name="column">An expression specifying the column to be returned.</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IDeleteFrom"/> which provides the
        /// ability to filter the result set, and set additional columns as output.
        /// </returns>
        IDeleteFrom<T> Output(Expression<Func<T, object>> column);
    }
}

