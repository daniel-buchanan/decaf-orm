using System.Collections.Generic;
using pdq.common;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq")]
namespace pdq.state
{
    public interface IDeleteQueryContext : IQueryContext
    {
		/// <summary>
        /// Get the <see cref="IQueryTarget"/> that this delete context targets.
        /// </summary>
		ITableTarget Table { get; }

		/// <summary>
        /// Get the <see cref="IWhere"/> conditions for this context.
        /// </summary>
		IWhere WhereClause { get; }

        /// <summary>
        /// Get the set of outputs for the query.
        /// </summary>
		IReadOnlyCollection<Output> Outputs { get; }

        /// <summary>
        /// The <see cref="IQueryTarget"/> to delete from.
        /// </summary>
        /// <param name="target">The target for the query.</param>
        void From(ITableTarget target);

        /// <summary>
        /// Filter the rows to be deleted.
        /// </summary>
        /// <param name="where">The <see cref="IWhere"/> conditions to add.</param>
		void Where(IWhere where);

        /// <summary>
        /// Add a column to be output as part of the query.
        /// </summary>
        /// <param name="output">The column to be output.</param>
		void Output(Output output);
    }
}

