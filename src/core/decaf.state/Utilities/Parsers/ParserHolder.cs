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
			this.value = new ValueParser(expressionHelper, callExpressionHelper, reflectionHelper);
            this.join = new JoinParser(expressionHelper, reflectionHelper);
            this.where = new WhereParser(expressionHelper, reflectionHelper, callExpressionHelper, this.join, this.value);
        }

		IParser IQueryParsers.Join => this.join;

		IParser IQueryParsers.Where => this.where;

		IParser IQueryParsers.Value => this.value;
    }
}

