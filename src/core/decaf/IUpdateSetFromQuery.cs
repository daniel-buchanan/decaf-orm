using System;
using System.Linq.Expressions;

namespace decaf
{
    public interface IUpdateSetFromQuery : IExecute
    {
        /// <summary>
        /// Set the value of a column to the provided value.
        /// </summary>
        /// <param name="columnToUpdate">The name of the column being set.</param>
        /// <param name="sourceColumn">The column from the query containing the value to update.</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSetFromQuery"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSetFromQuery Set(string columnToUpdate, string sourceColumn);

        /// <summary>
        /// Sets the filtering criteria for the update.
        /// </summary>
        /// <param name="builder">
        /// An <see cref="IWhereBuilder"/> which provides for the filtering
        /// of the query.
        /// </param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSetFromQuery"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSetFromQuery Where(Action<IWhereBuilder> builder);

        /// <summary>
        /// Add a column to be returned as output from the query.
        /// </summary>
        /// <param name="column">The name of the column to be returned.</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSetFromQuery"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSetFromQuery Output(string column);
    }

    public interface IUpdateSetFromQuery<TDestination, TSource> : IExecute
    {
        /// <summary>
        /// Set the value of a column to the provided value.
        /// </summary>
        /// <param name="columnToUpdate">An expression specifying the column being set.</param>
        /// <param name="sourceColumn">The column from the query containing the value to update.</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSetFromQuery{T}"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSetFromQuery<TDestination, TSource> Set(
            Expression<Func<TDestination, object>> columnToUpdate,
            Expression<Func<TSource, object>> sourceColumn);

        /// <summary>
        /// Sets the filtering criteria for the update.
        /// </summary>
        /// <param name="expression">
        /// An expression defining the filtering criteria for this update statement.
        /// </param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSetFromQuery{T}"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSetFromQuery<TDestination, TSource> Where(Expression<Func<TDestination, TSource, bool>> expression);

        /// <summary>
        /// Add a column to be returned as output from the query.
        /// </summary>
        /// <param name="column">
        /// An expression defining the column to be output.
        /// </param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSetFromQuery{T}"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSetFromQuery<TDestination, TSource> Output(Expression<Func<TDestination, object>> column);
    }
}

