using System;
using System.Linq.Expressions;

namespace pdq
{
	public interface IDelete : IBuilder
	{
        /// <summary>
        /// Sets the initial table, view or other schema item to perform the
        /// delete query on.
        /// </summary>
        /// <param name="name">The name of the table or view etc.</param>
        /// <param name="alias">(optional) The alias to give this source in the query.</param>
        /// <param name="schema">(optional) The schema to which this source belongs to.</param>
        /// <returns>(FluentApi) The ability to restrict the scope of the query and the execute it.</returns>
        /// <example>.From("users", "u")</example>
        /// <example>.From("users", "u", "transactional")</example>
        IDeleteFrom From(string name, string alias = null, string schema = null);

        /// <summary>
        /// Sets the initial table, view or other schema item to perform the
        /// delete query on.<br/>
        /// This is done from the type supplied to the method.
        /// </summary>
        /// <typeparam name="T">
        /// The type that defines the table, view or other schema item to use.
        /// </typeparam>
        /// <param name="aliasExpression">An expression which defines the alias for the table.</param>
        /// <returns>(FluentApi) The ability to restrict the scope of the query and execute it.</returns>
		IDeleteFrom<T> From<T>(Expression<Func<T, T>> aliasExpression);

        /// <summary>
        /// Sets the initial table, view or other schema item to perform the
        /// delete query on.<br/>
        /// This is done from the type supplied to the method.
        /// </summary>
        /// <typeparam name="T">
        /// The type that defines the table, view or other schema item to use.
        /// </typeparam>
        /// <returns>(FluentApi) The ability to restrict the scope of the query and execute it.</returns>
		IDeleteFrom<T> From<T>();
    }
}

