using System.Linq.Expressions;

namespace decaf.common.Utilities;

public interface IParser
{
	/// <summary>
	/// Parse a provided expression into a where clause.
	/// </summary>
	/// <param name="expression">The expression to parse.</param>
	/// <param name="context">The current query context</param>
	/// <returns>The parsed where clause.</returns>
	IWhere? Parse(Expression expression, IQueryContextExtended context);
}