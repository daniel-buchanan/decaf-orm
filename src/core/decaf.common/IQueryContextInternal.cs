using System.Linq.Expressions;
using decaf.common.Utilities;
using decaf.common.Utilities.Reflection;
using decaf.common;

namespace decaf.common
{
	internal interface IQueryContextInternal : IQueryContext
	{
		/// <summary>
        /// The <see cref="IExpressionHelper"/> which provides helper methods for
        /// dealing with <see cref="System.Linq.Expression"/>.
        /// </summary>
		IExpressionHelper ExpressionHelper { get; }

        /// <summary>
        /// The <see cref="IDynamicExpressionHelper"/> which provides helper methods
        /// for dealing with <see cref="Expression"/> that
        /// involve dynamics
        /// </summary>
        IDynamicExpressionHelper DynamicExpressionHelper { get; }

		/// <summary>
        /// The <see cref="IReflectionHelper"/> which provides helper methods for
        /// dealing with actions which require reflection.
        /// </summary>
		IReflectionHelper ReflectionHelper { get; }

        /// <summary>
        /// The <see cref="IQueryParsers"/> that provide helper methods for dealing
        /// with joins, where clauses and value parsing.
        /// </summary>
        IQueryParsers Parsers { get; }

        /// <summary>
        /// The Alias Manager for the current query context.
        /// </summary>
        IAliasManager AliasManager { get; }

		/// <summary>
        /// Adds a <see cref="IQueryTarget"/> to this context.
        /// </summary>
        /// <param name="target">The <see cref="IQueryTarget"/> to add.</param>
		void AddQueryTarget(IQueryTarget target);

        /// <summary>
        /// Adds a <see cref="IQueryTarget"/> to this context based on the provided expression.
        /// </summary>
        /// <param name="expression">The expression which defines the target.</param>
        /// <returns>The <see cref="IQueryTarget"/> that has been added.</returns>
		IQueryTarget AddQueryTarget(Expression expression);

        /// <summary>
        /// Get a query target based off an expression.
        /// </summary>
        /// <param name="expression">The expression to use to find the query target.</param>
        /// <returns>A query target if found, otherwise null.</returns>
        IQueryTarget GetQueryTarget(Expression expression);

        /// <summary>
        /// Get a query target based off an alias.
        /// </summary>
        /// <param name="alias">The alias of the query target to find.</param>
        /// <returns>A query target if found, otherwise null.</returns>
        IQueryTarget GetQueryTarget(string alias);
    }

    internal interface IQueryParsers
    {
        IParser Join { get; }

        IParser Where { get; }

        IParser Value { get; }
    }
}

