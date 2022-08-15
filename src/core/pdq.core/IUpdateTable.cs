using System;
using System.Linq.Expressions;

namespace pdq
{
    public interface IUpdateTable
    {
        /// <summary>
        /// Set the source of the udpate to be from a query.
        /// For more information, see <see cref="ISelectWithAlias"/>.
        /// </summary>
        /// <param name="query">The query that defines the source for this update.</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSetFromQuery"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSetFromQuery From(Action<ISelectWithAlias> query);

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
        /// Set multiple columns and their values at once by providing a
        /// dynamic object which represents them.
        /// </summary>
        /// <param name="values">
        /// A <see cref="dynamic"/> object which represents all of the columns
        /// and their values to be updated.
        /// </param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSet"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSet Set(dynamic values);
    }

    public interface IUpdateTable<T>
    {
        /// <summary>
        /// Set the source of the udpate to be from a query.
        /// For more information, see <see cref="ISelectWithAlias"/>.
        /// </summary>
        /// <param name="query">The query that defines the source for this update.</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSet{T}"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSetFromQuery<T> From(Action<ISelectWithAlias> query);

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
        /// Set multiple columns and their values at once by providing a
        /// object which represents them.
        /// </summary>
        /// <param name="values">
        /// An object of the type being updated, providing
        /// the values to be updated.
        /// </param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateSet{T}"/> which provides the
        /// ability to continue setting column values, perform filtering or
        /// execute the query.
        /// </returns>
        IUpdateSet<T> Set(T values);
    }
}

