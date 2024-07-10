using decaf.common;
using decaf.common.Utilities;
using decaf.common.Utilities.Reflection;

namespace decaf.state.Utilities.Parsers
{
	internal class ParserHolder : IQueryParsers
	{
		private readonly IParser join;
		private readonly IParser where;
		private readonly IParser value;
		
		internal ParserHolder(
			IExpressionHelper expressionHelper,
			IReflectionHelper reflectionHelper,
			CallExpressionHelper callExpressionHelper)
		{
			value = new ValueParser(expressionHelper, callExpressionHelper, reflectionHelper);
            join = new JoinParser(expressionHelper, reflectionHelper);
            where = new WhereParser(expressionHelper, reflectionHelper, callExpressionHelper, join, value);
        }

		IParser IQueryParsers.Join => join;

		IParser IQueryParsers.Where => where;

		IParser IQueryParsers.Value => value;
    }
}

