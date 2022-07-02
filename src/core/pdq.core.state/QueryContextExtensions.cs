using System.Linq.Expressions;
using pdq.common;

namespace pdq.state
{
	public static class QueryContextExtensions
	{
		public static IHelperExtensions Helpers(this IQueryContext context)
        {
            var internalContext = (IQueryContextInternal)context;
			return new HelperExtensions(internalContext);
        }

		public static string GetTableName(
			this IHelperExtensions self,
			Expression expression)
        {
			var tableType = self.Context.ExpressionHelper.GetType(expression);
			return self.Context.ReflectionHelper.GetTableName(tableType);
        }

        public static string GetTableName<T>(
            this IHelperExtensions self)
        {
            return self.Context.ReflectionHelper.GetTableName<T>();
        }

        public static string GetTableAlias(
            this IHelperExtensions self,
            Expression expression)
        {
            return self.Context.ExpressionHelper.GetParameterName(expression);
        }

        public static string GetColumnName(
            this IHelperExtensions self,
            Expression expression)
        {
            return self.Context.ExpressionHelper.GetName(expression);
        }

        public static IWhere ParseWhere(
            this IHelperExtensions self,
            Expression expression)
            => self.Context.ExpressionHelper.ParseWhereExpression(expression);
	}
}

