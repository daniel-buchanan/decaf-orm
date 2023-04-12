using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using pdq.Attributes;
using pdq.common;

namespace pdq.common.Utilities.Reflection.Dynamic
{
    internal class DynamicExpressionHelper : IDynamicExpressionHelper
    {
        private readonly IExpressionHelper expressionHelper;
        private readonly ValueFunctionHelper callExpressionHelper;

        public DynamicExpressionHelper(
            IExpressionHelper expressionHelper,
            ValueFunctionHelper callExpressionHelper)
        {
            this.expressionHelper = expressionHelper;
            this.callExpressionHelper = callExpressionHelper;
        }

        /// <inheritdoc/>
        public IEnumerable<DynamicColumnInfo> GetProperties(Expression expr, IQueryContextInternal context)
        {
            var expression = (LambdaExpression)expr;

            if (expression.Body is MemberInitExpression)
                return GetPropertiesForMemberInit(expression, context);

            if (expression.Body is ConstantExpression)
                return GetPropertiesForConstant(expression, context);

            return GetPropertiesForNew(expression, context);
        }

        private List<DynamicColumnInfo> GetPropertiesForMemberInit(
            LambdaExpression expression,
            IQueryContextInternal context)
        {
            var initExpr = (MemberInitExpression)expression.Body;

            var numBindings = initExpr.Bindings.Count;
            var properties = new DynamicColumnInfo[numBindings];

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

        private List<DynamicColumnInfo> GetPropertiesForConstant(
            LambdaExpression expression,
            IQueryContextInternal context)
        {
            var results = new List<DynamicColumnInfo>();
            var constExpression = expression.Body as ConstantExpression;
            var obj = constExpression.Value;

            // get object type and properties
            var objType = obj.GetType();
            var properties = context.ReflectionHelper.GetMemberDetails(obj, context.Kind);

            // iternate through properties
            foreach (var p in properties)
            {
                // get field name
                var value = context.ReflectionHelper.GetPropertyValue(obj, p.NewName);

                // add to set
                var info = DynamicColumnInfo.Create(
                    name: p.Name,
                    newName: p.NewName,
                    value: value,
                    valueType: value?.GetType(),
                    type: objType);
                results.Add(info);
            }

            return results;
        }

        private List<DynamicColumnInfo> GetPropertiesForNew(
            LambdaExpression expression,
            IQueryContextInternal context)
        {
            var body = expression.Body as NewExpression;
            if (body == null) return new List<DynamicColumnInfo>();

            var countArguments = body.Arguments.Count;
            var properties = new DynamicColumnInfo[countArguments];

            for (var i = 0; i < countArguments; i += 1)
            {
                var a = body.Arguments[i];
                if (!TryGetColumnDetails(a, context, out var info))
                {
                    info = DynamicColumnInfo.Empty();
                    var val = this.expressionHelper.GetValue(a);
                    var valueType = this.expressionHelper.GetMemberType(a);
                    info.SetValue(val);
                    info.SetValueType(valueType);
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
            out DynamicColumnInfo info)
        {
            if (TryGetColumnDetailsDynamic(expression, out info))
                return true;

            return TryGetColumnDetailsConcrete(expression, context, out info);
        }

        private bool TryGetColumnDetailsDynamic(
            Expression expression,
            out DynamicColumnInfo info)
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

            info = DynamicColumnInfo.Create(name: column, alias: alias, type: typeof(object));

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
            out DynamicColumnInfo info)
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
            var column = this.expressionHelper.GetMemberName(memberExpression);
            var alias = this.expressionHelper.GetParameterName(memberExpression);
            var type = parameterExpression.Type;
            info = DynamicColumnInfo.Create(name: column, alias: alias, type: type, function: function);

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

            var defaultValue = DefaultValueHelper.Get(methodCallExpression.Method.ReturnType);
            var tempExpression = Expression.Equal(methodCallExpression, Expression.Constant(defaultValue));
            var parsed = this.callExpressionHelper.ParseFunction(methodCallExpression);

            if (parsed == null) return;

            function = parsed;
        }
    }
}

