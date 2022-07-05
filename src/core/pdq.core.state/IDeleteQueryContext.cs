using pdq.common;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq")]
namespace pdq.state
{
    public interface IDeleteQueryContext : IQueryContext
    {
		/// <summary>
        /// The <see cref="IQueryTarget"/> that this delete context targets.
        /// </summary>
		ITableTarget Table { get; }

		/// <summary>
        /// Any <see cref="IWhere"/> conditions for this context.
        /// </summary>
		IWhere WhereClause { get; }

		/// <summary>
        /// The <see cref="IQueryTarget"/> to delete from.
        /// </summary>
        /// <param name="target"></param>
        /// <returns>This <see cref="IDeleteQueryContext"/>.</returns>
		IDeleteQueryContext From(ITableTarget target);

        /// <summary>
        /// Filter the rows to be deleted.
        /// </summary>
        /// <param name="where">The <see cref="IWhere"/> conditions to add.</param>
        /// <returns>This <see cref="IDeleteQueryContext"/>.</returns>
		IDeleteQueryContext Where(IWhere where);
	}
}

