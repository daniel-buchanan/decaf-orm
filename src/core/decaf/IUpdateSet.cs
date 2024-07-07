using System;
using System.Linq.Expressions;
using decaf.common;

namespace decaf
{
    public interface IUpdateSet : IExecute
    {
        /// <summary>
        /// Set the value of a column to the provided value.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="column">The name of the column being set.</param>
        /// <param name="value">The value the column should be updated to.</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSet"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSet Set<T>(string column, T value);

        /// <summary>
        /// Sets the filtering criteria for the update.
        /// </summary>
        /// <param name="builder">
        /// An <see cref="IWhereBuilder"/> which provides for the filtering
        /// of the query.
        /// </param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSet"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSet Where(Action<IWhereBuilder> builder);

        /// <summary>
        /// Sets the filtering criteria for the update.
        /// </summary>
        /// <param name="clause">
        /// An <see cref="IWhere"/> clause which provides for the filtering
        /// of the query.
        /// </param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSet"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSet Where(IWhere clause);

        /// <summary>
        /// Add a column to be returned as output from the query.
        /// </summary>
        /// <param name="column">The name of the column to be returned.</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSet"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSet Output(string column);
    }

    public interface IUpdateSet<T> : IExecute
    {
        /// <summary>
        /// Set the value of a column to the provided value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to be set.</typeparam>
        /// <param name="column">An expression specifying the column being set.</param>
        /// <param name="value">The value the column should be updated to.</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSet{T}"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSet<T> Set<TValue>(Expression<Func<T, TValue>> column, TValue value);

        /// <summary>
        /// Sets the filtering criteria for the update.
        /// </summary>
        /// <param name="expression">
        /// An expression defining the filtering criteria for this update statement.
        /// </param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSet{T}"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSet<T> Where(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Sets the filtering criteria for the update.
        /// </summary>
        /// <param name="clause">
        /// An <see cref="IWhere"/> clause which provides for the filtering
        /// of the query.
        /// </param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSet"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSet<T> Where(IWhere clause);

        /// <summary>
        /// Add a column to be returned as output from the query.
        /// </summary>
        /// <param name="column">
        /// An expression defining the column to be output.
        /// </param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSet{T}"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSet<T> Output(Expression<Func<T, object>> column);
    }
}

