using System;
using System.Linq.Expressions;
using System.Reflection;
using decaf.common;
using decaf.common.Utilities.Reflection;
using decaf.state.Conditionals;

namespace decaf.state.Utilities.Parsers;

internal class ValueParser(
    IExpressionHelper expressionHelper,
    CallExpressionHelper callExpressionHelper,
    IReflectionHelper reflectionHelper)
    : BaseParser(expressionHelper, reflectionHelper)
{
    public override IWhere Parse(Expression expression, IQueryContextExtended context)
    {
        var earlyResult = callExpressionHelper.ParseExpression(expression, context);
        if (earlyResult != null) return earlyResult;

        var valueResult = ParseMemberExpression(expression, context) ?? ParseBinaryExpression(expression, context);

        var col = Column.Create(valueResult.Field, valueResult.Table);

        var isNotEquals = valueResult.Operator == EqualityOperator.NotEquals;
        if (isNotEquals)
            valueResult.Operator = EqualityOperator.Equals;

        //add the model type for the type def of the repository
        var convertedValue = GetConvertedValue(valueResult.Value, valueResult.ValueType);
        var parameters = new[] { col, valueResult.Operator, convertedValue };
        var parameterTypes = new[] { col.GetType(), typeof(EqualityOperator), valueResult.ValueType };
        var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

        //get the generic type definition for the model
        var genericType = typeof(Column<>);
        Type[] genericTypeArguments = [valueResult.ValueType];
        var constructedType = genericType.MakeGenericType(genericTypeArguments);
        var ctor = constructedType.GetConstructor(bindingFlags, null, parameterTypes, null);

        object instance;

        if(ctor != null) instance = ctor.Invoke(parameters);
        else instance = Activator.CreateInstance(constructedType, parameters);

        if (isNotEquals)
            return Not.This((IWhere)instance);

        return (IWhere)instance;
    }

    private object GetConvertedValue(object val, Type valType)
    {
        if (valType.IsEnum) return Enum.ToObject(valType, val);
        if (val == null && valType == typeof(bool)) return true;
        if (val == null) return null;

        object convertedValue;
        try
        {
            convertedValue = Convert.ChangeType(val, valType);
        }
        catch
        {
            var underlyingType = reflectionHelper.GetUnderlyingType(valType);
            convertedValue = Convert.ChangeType(val, underlyingType);
        }

        return convertedValue;
    }

    private ValueResult ParseMemberExpression(Expression expression, IQueryContextExtended context)
    {
        var memberExpression = expression as MemberExpression;
        if (memberExpression == null) return null;

        var val = Parse(expression, context);
        var valType = expressionHelper.GetMemberType(expression);
        var field = expressionHelper.GetMemberName(expression);
        var op = EqualityOperator.Equals;

        var target = context.GetQueryTarget(memberExpression);
        if (target == null)
        {
            var table = reflectionHelper.GetTableName(memberExpression.Member.DeclaringType);
            var alias = expressionHelper.GetParameterName(expression);
            target = QueryTargets.TableTarget.Create(table, alias);
            context.AddQueryTarget(target);
        }

        return new ValueResult(field, target, val, valType, op);
    }

    private ValueResult ParseBinaryExpression(Expression expression, IQueryContextExtended context)
    {
        BinaryExpression operation;
        if (expression is LambdaExpression lambda) operation = lambda.Body as BinaryExpression;
        else operation = expression as BinaryExpression;

        if (operation is null) return null;
        
        var op = expressionHelper.ConvertExpressionTypeToEqualityOperator(operation.NodeType);
        Expression valueExpression = null;
        MemberExpression memberExpression = null;

        if(operation.Left is MemberExpression left)
        {
            memberExpression = left;
            valueExpression = operation.Right;
        }
        else if(operation.Right is MemberExpression right)
        {
            memberExpression = right;
            valueExpression = operation.Left;
        }

        if (valueExpression is null)
            return null;

        var field = expressionHelper.GetMemberName(memberExpression);
        var value = expressionHelper.GetValue(valueExpression);
        var valueType = expressionHelper.GetMemberType(valueExpression);

        var target = context.GetQueryTarget(memberExpression);
        if(target == null)
        {
            var alias = expressionHelper.GetParameterName(memberExpression);
            var table = reflectionHelper.GetTableName(memberExpression.Member.DeclaringType);
            target = QueryTargets.TableTarget.Create(table, alias);
            context.AddQueryTarget(target);
        }

        return new ValueResult(field, target, value, valueType, op);
    }

    private sealed class ValueResult(
        string field,
        IQueryTarget table,
        object value,
        Type valueType,
        EqualityOperator op)
    {
        public object Value { get; set; } = value;
        public Type ValueType { get; set; } = valueType;
        public string Field { get; set; } = field;
        public IQueryTarget Table { get; set; } = table;
        public EqualityOperator Operator { get; set; } = op;
    }
}