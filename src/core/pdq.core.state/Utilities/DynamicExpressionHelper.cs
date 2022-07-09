using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace pdq.state.Utilities
{
	internal class DynamicExpressionHelper
	{
        private readonly IExpressionHelper expressionHelper;
        private readonly IReflectionHelper reflectionHelper;

        public DynamicExpressionHelper(
            IExpressionHelper expressionHelper,
            IReflectionHelper reflectionHelper)
        {
            this.expressionHelper = expressionHelper;
            this.reflectionHelper = reflectionHelper;
        }

		public IEnumerable<DynamicPropertyInfo> GetProperties(Expression expr)
        {
            var expression = (LambdaExpression)expr;

            if (expression.Body is MemberInitExpression)
                return GetPropertiesForMemberInit(expression);

            return GetPropertiesForNew(expression);
        }

        private List<DynamicPropertyInfo> GetPropertiesForMemberInit(LambdaExpression expression)
        {
            var initExpr = (MemberInitExpression)expression.Body;

            var countBindings = initExpr.Bindings.Count();
            var properties = new DynamicPropertyInfo[countBindings];

            var index = 0;
            foreach (var b in initExpr.Bindings)
            {
                var memberBinding = (MemberAssignment)b;
                var memberExpression = (MemberExpression)memberBinding.Expression;
                var parameterExpression = (ParameterExpression)memberExpression.Expression;
                var column = this.expressionHelper.GetName(memberExpression);
                var alias = this.expressionHelper.GetParameterName(memberExpression);
                var newName = memberBinding.Member.Name;

                properties[index] = DynamicPropertyInfo.Create(column, newName, type: parameterExpression.Type);

                index += 1;
            }

            return properties.ToList();
        }

        private List<DynamicPropertyInfo> GetPropertiesForNew(LambdaExpression expression)
        {
            var body = (NewExpression)expression.Body;

            var countArguments = body.Arguments.Count;
            var properties = new DynamicPropertyInfo[countArguments];

            var index = 0;
            foreach (var a in body.Arguments)
            {

                var memberExpression = (MemberExpression)a;
                var parameterExpression = (ParameterExpression)memberExpression.Expression;
                var table = this.reflectionHelper.GetTableName(parameterExpression.Type);
                var column = this.expressionHelper.GetName(memberExpression);
                var alias = this.expressionHelper.GetParameterName(a);

                properties[index] = DynamicPropertyInfo.Create(name: column, type: parameterExpression.Type);

                index += 1;
            }

            index = 0;
            foreach (var m in body.Members)
            {
                if (m.Name != properties[index].Name)
                {
                    properties[index].SetNewName(m.Name);
                }
                index += 1;
            }

            return properties.ToList();
        }
	}
}

