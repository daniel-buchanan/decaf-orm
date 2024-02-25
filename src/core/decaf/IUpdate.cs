using System;
using System.Linq.Expressions;

namespace decaf
{
    public interface IUpdate
    {
        /// <summary>
        /// Sets the table to be updated.
        /// </summary>
        /// <param name="table">The name of the table to be updated.</param>
        /// <param name="alias">The alias for the table. (Optional)</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateTable"/> with the ability
        /// to either specify a source for the query, or set column values.
        /// </returns>
        IUpdateTable Table(string table, string alias = null);

        /// <summary>
        /// Sets the table to be updated.
        /// </summary>
        /// <typeparam name="T">The Type representing the table to be updated.</typeparam>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateTable{T}"/> with the ability
        /// to either specify a source for the query, or set column values.
        /// </returns>
        IUpdateTable<T> Table<T>();

        /// <summary>
        /// Sets the table to be updated.
        /// </summary>
        /// <typeparam name="T">The Type representing the table to be updated.</typeparam>
        /// <param name="expression">An expression specifying the alias for the table.</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateTable{T}"/> with the ability
        /// to either specify a source for the query, or set column values.
        /// </returns>
        IUpdateTable<T> Table<T>(Expression<Func<T, object>> expression);

        /// <summary>
        /// Sets the table to be updated.
        /// </summary>
        /// <typeparam name="T">The Type representing the table to be updated.</typeparam>
        /// <param name="alias">The alias for the table.</param>
        /// <returns>
        /// (FluentApi) Returns an <see cref="IUpdateTable{T}"/> with the ability
        /// to either specify a source for the query, or set column values.
        /// </returns>
        IUpdateTable<T> Table<T>(string alias);
    }
}

