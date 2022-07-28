using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace pdq.state.Utilities
{
	internal class DynamicExpressionHelper
	{
        private readonly IExpressionHelper expressionHelper;

        public DynamicExpressionHelper(IExpressionHelper expressionHelper)
        {
            this.expressionHelper = expressionHelper;
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
                var memberBinding = b as MemberAssignment;
                if (!TryGetColumnDetails(memberBinding.Expression, out var column, out var alias, out var type))
                    continue;
                var newName = memberBinding.Member.Name;
                if (memberBinding.Member.MemberType == MemberTypes.Field)
                    type = (memberBinding.Member as FieldInfo).FieldType;

                if (memberBinding.Member.MemberType == MemberTypes.Property)
                    type = (memberBinding.Member as PropertyInfo).PropertyType;

                properties[index] = DynamicPropertyInfo.Create(column, newName, alias: alias, type: type);

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
                if (!TryGetColumnDetails(body.Arguments[i], out var column, out var alias, out var type))
                    continue;

                properties[i] = DynamicPropertyInfo.Create(name: column, alias: alias, type: type);
            }

            for (var i = 0; i < body.Members.Count; i += 1)
            {
                var m = body.Members[i];
                if (m.Name == properties[i].Name) continue;

                properties[i].SetNewName(m.Name);
            }

            return properties.ToList();
        }

        private bool TryGetColumnDetails(Expression expression, out string column, out string alias, out Type type)
        {
            column = null;
            alias = null;
            type = typeof(object);

            var methodCallExpression = expression as MethodCallExpression;
            if(methodCallExpression != null)
            {
                if (methodCallExpression.Method.DeclaringType != typeof(ISelectColumnBuilder))
                    return false;

                if(methodCallExpression.Arguments.Count == 1)
                {
                    var arg = methodCallExpression.Arguments[0] as ConstantExpression;
                    column = arg.Value as string;
                }
                else
                {
                    var arg = methodCallExpression.Arguments[0] as ConstantExpression;
                    column = arg.Value as string;

                    arg = methodCallExpression.Arguments[1] as ConstantExpression;
                    alias = arg.Value as string;
                }
                
                return true;
            }

            var memberExpression = expression as MemberExpression;
            if (memberExpression == null) return false;

            var parameterExpression = memberExpression.Expression as ParameterExpression;
            column = this.expressionHelper.GetName(memberExpression);
            alias = this.expressionHelper.GetParameterName(memberExpression);
            type = parameterExpression.Type;

            return true;
        }
	}
}

