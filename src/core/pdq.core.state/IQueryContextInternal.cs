using pdq.common;
using pdq.state.Utilities;

namespace pdq.state
{
	public interface IQueryContextInternal : IQueryContext
	{
		/// <summary>
        /// The <see cref="IExpressionHelper"/> which provides helper methods for
        /// dealing with <see cref="System.Linq.Expression"/>.
        /// </summary>
		internal IExpressionHelper ExpressionHelper { get; }

		/// <summary>
        /// The <see cref="IReflectionHelper"/> which provides helper methods for
        /// dealing with actions which require reflection.
        /// </summary>
		internal IReflectionHelper ReflectionHelper { get; }

		/// <summary>
        /// Adds a <see cref="IQueryTarget"/> to this context.
        /// </summary>
        /// <param name="target">The <see cref="IQueryTarget"/> to add.</param>
		internal void AddQueryTarget(IQueryTarget target);
    }
}

