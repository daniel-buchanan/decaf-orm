using pdq.common;
using pdq.state.Utilities;

namespace pdq.state
{
	public interface IQueryContextInternal : IQueryContext
	{
		internal IExpressionHelper ExpressionHelper { get; }
		internal IReflectionHelper ReflectionHelper { get; }
    }
}

