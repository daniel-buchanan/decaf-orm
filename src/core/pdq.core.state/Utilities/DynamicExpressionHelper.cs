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

            var numBindings = initExpr.Bindings.Count;
            var properties = new DynamicPropertyInfo[numBindings];

            var index = 0;
            foreach (var b in initExpr.Bindings)
            {
                var memberBinding = (MemberAssignment)b;
                var memberExpression = (MemberExpression)memberBinding.Expression;
                var parameterExpression = (ParameterExpression)memberExpression.Expression;
                var column = this.expressionHelper.GetName(memberExpression);
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

            for(var i = 0; i < countArguments; i += 1)
            {
                var memberExpression = (MemberExpression)body.Arguments[i];
                var parameterExpression = (ParameterExpression)memberExpression.Expression;
                var column = this.expressionHelper.GetName(memberExpression);

                properties[i] = DynamicPropertyInfo.Create(name: column, type: parameterExpression.Type);
            }

            for (var i = 0; i < body.Members.Count; i += 1)
            {
                var m = body.Members[i];
                if (m.Name == properties[i].Name) continue;

                properties[i].SetNewName(m.Name);
            }

            return properties.ToList();
        }
	}
}

