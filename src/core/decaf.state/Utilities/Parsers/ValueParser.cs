using System;
using System.Linq.Expressions;
using System.Reflection;
using decaf.common;
using decaf.common.Utilities;
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

        var col = Column.Create(valueResult!.Field, valueResult!.Table);

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

    private object? GetConvertedValue(object? val, Type valType)
    {
        if (val is not null && valType.IsEnum) return Enum.ToObject(valType, val);
        if (val is null && valType == typeof(bool)) return true;
        if (val is null) return null;

        object convertedValue;
        try
        {
            convertedValue = Convert.ChangeType(val, valType);
        }
        catch
        {
            var underlyingType = ReflectionHelper.GetUnderlyingType(valType);
            convertedValue = Convert.ChangeType(val, underlyingType);
        }

        return convertedValue;
    }

    private ValueResult? ParseMemberExpression(Expression expression, IQueryContextExtended context)
    {
        var memberExpression = expression.CastAs<MemberExpression>();
        if (memberExpression is null) return null;

        var val = Parse(expression, context);
        var valType = ExpressionHelper.GetMemberType(expression);
        var field = ExpressionHelper.GetMemberName(expression);
        var op = EqualityOperator.Equals;

        var target = context.GetQueryTarget(memberExpression);
        if (target is null)
        {
            var table = ReflectionHelper.GetTableName(memberExpression.Member.DeclaringType!);
            var alias = ExpressionHelper.GetParameterName(expression);
            target = QueryTargets.TableTarget.Create(table, alias);
            context.AddQueryTarget(target);
        }

        return new ValueResult(field!, target, val, valType!, op);
    }

    private ValueResult? ParseBinaryExpression(Expression expression, IQueryContextExtended context)
    {
        BinaryExpression? operation;
        if (expression is LambdaExpression lambda) operation = lambda.Body.CastAs<BinaryExpression>();
        else operation = expression.CastAs<BinaryExpression>();

        if (operation is null) return null;
        
        var op = ExpressionHelper.ConvertExpressionTypeToEqualityOperator(operation.NodeType);
        Expression? valueExpression = null;
        MemberExpression? memberExpression = null;

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

        var field = ExpressionHelper.GetMemberName(memberExpression!);
        var value = ExpressionHelper.GetValue(valueExpression);
        var valueType = ExpressionHelper.GetMemberType(valueExpression);

        var target = context.GetQueryTarget(memberExpression!);
        if(target == null)
        {
            var alias = ExpressionHelper.GetParameterName(memberExpression!);
            var table = ReflectionHelper.GetTableName(memberExpression!.Member.DeclaringType!);
            target = QueryTargets.TableTarget.Create(table, alias);
            context.AddQueryTarget(target);
        }

        return new ValueResult(field!, target, value!, valueType!, op);
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