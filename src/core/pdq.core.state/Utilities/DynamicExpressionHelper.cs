using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace pdq.state.Utilities
{
    internal class DynamicExpressionHelper : IDynamicExpressionHelper
    {
        private readonly IExpressionHelper expressionHelper;
        private readonly CallExpressionHelper callExpressionHelper;

        public DynamicExpressionHelper(
            IExpressionHelper expressionHelper,
            CallExpressionHelper callExpressionHelper)
        {
            this.expressionHelper = expressionHelper;
            this.callExpressionHelper = callExpressionHelper;
        }

        /// <inheritdoc/>
        public IEnumerable<DynamicPropertyInfo> GetProperties(Expression expr, IQueryContextInternal context)
        {
            var expression = (LambdaExpression)expr;

            if (expression.Body is MemberInitExpression)
                return GetPropertiesForMemberInit(expression, context);

            return GetPropertiesForNew(expression, context);
        }

        private List<DynamicPropertyInfo> GetPropertiesForMemberInit(
            LambdaExpression expression,
            IQueryContextInternal context)
        {
            var initExpr = (MemberInitExpression)expression.Body;

            var numBindings = initExpr.Bindings.Count;
            var properties = new DynamicPropertyInfo[numBindings];

            var index = 0;
            foreach (var b in initExpr.Bindings)
            {
                var memberBinding = b as MemberAssignment;
                if (!TryGetColumnDetails(memberBinding.Expression, context, out var info))
                    continue;

                var newName = memberBinding.Member.Name;
                info.SetNewName(newName);

                if (memberBinding.Member.MemberType == MemberTypes.Field)
                    info.SetType((memberBinding.Member as FieldInfo).FieldType);

                if (memberBinding.Member.MemberType == MemberTypes.Property)
                    info.SetType((memberBinding.Member as PropertyInfo).PropertyType);

                properties[index] = info;
                index += 1;
            }

            return properties.ToList();
        }

        private List<DynamicPropertyInfo> GetPropertiesForNew(
            LambdaExpression expression,
            IQueryContextInternal context)
        {
            var body = expression.Body as NewExpression;
            if (body == null) return new List<DynamicPropertyInfo>();

            var countArguments = body.Arguments.Count;
            var properties = new DynamicPropertyInfo[countArguments];

            for (var i = 0; i < countArguments; i += 1)
            {
                var a = body.Arguments[i];
                if (!TryGetColumnDetails(a, context, out var info))
                {
                    info = DynamicPropertyInfo.Empty();
                    var val = this.expressionHelper.GetValue(a);
                    info.SetValue(val);
                }

                properties[i] = info;
            }

            for (var i = 0; i < body.Members.Count; i += 1)
            {
                var m = body.Members[i];
                if (m.Name == properties[i].Name) continue;

                if (string.IsNullOrWhiteSpace(properties[i].Name))
                    properties[i].SetName(m.Name);
                else
                    properties[i].SetNewName(m.Name);
            }

            return properties.ToList();
        }

        private bool TryGetColumnDetails(
            Expression expression,
            IQueryContextInternal context,
            out DynamicPropertyInfo info)
        {
            if (TryGetColumnDetailsDynamic(expression, out info))
                return true;

            return TryGetColumnDetailsConcrete(expression, context, out info);
        }

        private bool TryGetColumnDetailsDynamic(
            Expression expression,
            out DynamicPropertyInfo info)
        {
            info = null;
            string column, alias = null;

            if (expression == null) return false;
            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null) return false;

            if (methodCallExpression.Method.DeclaringType == typeof(ISelectColumnBuilder))
                GetColumnAliasForSelectBuilder(methodCallExpression, out column, out alias);
            else if (methodCallExpression.Method.DeclaringType == typeof(IInsertColumnBuilder))
                GetColumnAliasForInsertBuilder(out column, out alias);
            else
                return false;

            if (methodCallExpression.Arguments.Count == 1)
            {
                var arg = methodCallExpression.Arguments[0] as ConstantExpression;
                column = arg.Value as string;
            }
            else if (methodCallExpression.Arguments.Count > 1)
            {
                var arg = methodCallExpression.Arguments[0] as ConstantExpression;
                column = arg.Value as string;

                arg = methodCallExpression.Arguments[1] as ConstantExpression;
                alias = arg.Value as string;
            }

            info = DynamicPropertyInfo.Create(name: column, alias: alias, type: typeof(object));

            return true;
        }

        private void GetColumnAliasForSelectBuilder(
            MethodCallExpression expression,
            out string column,
            out string alias)
        {
            column = null;
            alias = null;

            if (expression.Arguments.Count == 1)
            {
                var arg = expression.Arguments[0] as ConstantExpression;
                column = arg.Value as string;
            }
            else
            {
                var arg = expression.Arguments[0] as ConstantExpression;
                column = arg.Value as string;

                arg = expression.Arguments[1] as ConstantExpression;
                alias = arg.Value as string;
            }
        }

        private void GetColumnAliasForInsertBuilder(
            out string column,
            out string alias)
        {
            column = null;
            alias = null;
        }

        private bool TryGetColumnDetailsConcrete(
            Expression expression,
            IQueryContextInternal context,
            out DynamicPropertyInfo info)
        {
            info = null;

            var methodCallExpression = expression as MethodCallExpression;
            MemberExpression memberExpression = null;

            if (expression is UnaryExpression)
            {
                var unaryExpression = expression as UnaryExpression;
                methodCallExpression = unaryExpression.Operand as MethodCallExpression;
            }

            if (methodCallExpression?.Arguments.Count == 0)
                memberExpression = methodCallExpression.Object as MemberExpression;
            else if(methodCallExpression != null)
                memberExpression = methodCallExpression.Arguments[0] as MemberExpression;

            if (memberExpression == null)
                memberExpression = expression as MemberExpression;

            GetFunction(methodCallExpression, context, out var tempMemberExpression, out var function);
            memberExpression = tempMemberExpression ?? memberExpression;

            if (memberExpression == null) return false;

            var parameterExpression = memberExpression.Expression as ParameterExpression;
            var column = this.expressionHelper.GetName(memberExpression);
            var alias = this.expressionHelper.GetParameterName(memberExpression);
            var type = parameterExpression.Type;
            info = DynamicPropertyInfo.Create(name: column, alias: alias, type: type, function: function);

            return true;
        }

        private void GetFunction(
            MethodCallExpression methodCallExpression,
            IQueryContextInternal context,
            out MemberExpression memberExpression,
            out IValueFunction function)
        {
            function = null;
            memberExpression = null;
            Expression argument;

            if (methodCallExpression == null)
                return;

            if (methodCallExpression.Arguments.Count >= 1 &&
               methodCallExpression.Object == null)
                argument = methodCallExpression.Arguments[0];
            else
                argument = methodCallExpression.Object;

            memberExpression = argument as MemberExpression;

            var defaultValue = methodCallExpression.Method.ReturnType.IsValueType ?
                Activator.CreateInstance(methodCallExpression.Method.ReturnType) :
                null;
            var tempExpression = Expression.Equal(methodCallExpression, Expression.Constant(defaultValue));
            var parsed = this.callExpressionHelper.ParseExpression(tempExpression, context);

            if (!(parsed is Conditionals.IColumn)) return;

            var parsedColumn = parsed as Conditionals.IColumn;
            function = parsedColumn.ValueFunction;
        }
    }
}

