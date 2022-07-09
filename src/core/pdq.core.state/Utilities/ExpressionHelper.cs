using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state.Conditionals;

namespace pdq.state.Utilities
{
    internal class ExpressionHelper : IExpressionHelper
    {
        private readonly ConstantAccess constantAccess;
        private readonly MemberAccess memberAccess;
        private readonly ConvertAccess convertAccess;
        private readonly ParameterAccess parameterAccess;
        private readonly LambdaAccess lambdaAccess;
        private readonly IReflectionHelper reflectionHelper;
        private readonly CallExpressionHelper callExpressionHelper;
        private readonly IAliasManager aliasManager;
        private readonly IQueryContextInternal context;

        public ExpressionHelper(
            IReflectionHelper reflectionHelper,
            IAliasManager aliasManager,
            IQueryContextInternal context)
        {
            // setup helpers
            this.aliasManager = aliasManager;
            this.context = context;
            this.constantAccess = new ConstantAccess();
            this.convertAccess = new ConvertAccess();
            this.parameterAccess = new ParameterAccess();
            this.lambdaAccess = new LambdaAccess();
            this.reflectionHelper = reflectionHelper;
            this.memberAccess = new MemberAccess(this.reflectionHelper);
            this.callExpressionHelper = new CallExpressionHelper(this);
        }

        /// <summary>
        /// Get the field name from the expression. i.e. p => p.ID (ID would be the field name). Note that this *should* be a SIMPLE expression, as in the previous example.
        /// </summary>
        /// <param name="expression">The expression to get the name from</param>
        /// <returns>The name of the field</returns>
        public string GetName(Expression expression)
        {
            // switch on node type
            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    return convertAccess.GetName(expression, this);
                case ExpressionType.MemberAccess:
                    return memberAccess.GetName(expression);
                case ExpressionType.Constant:
                    return memberAccess.GetName(expression);
                case ExpressionType.Call:
                    {
                        var call = (MethodCallExpression)expression;
                        if (call.Object == null)
                        {
                            return GetName(call.Arguments[0]);
                        }
                        return GetName(call.Object);
                    }
                case ExpressionType.Lambda:
                    var body = ((LambdaExpression)expression).Body;
                    return this.GetName(body);
            }

            // if not found, return null
            return null;
        }

        public string GetMethodName(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Lambda) expression = ((LambdaExpression)expression).Body;
            if (expression.NodeType == ExpressionType.Convert) expression = ((UnaryExpression)expression).Operand;

            if (!(expression is MethodCallExpression)) return null;

            var call = (MethodCallExpression)expression;
            return call.Method.Name;
        }

        /// <summary>
        /// Get the name of the parameter used in an expression. i.e. p => p.ID (p would be the parameter name). Note that this *should* be a SIMPLE expression, as in the previous example.
        /// </summary>
        /// <param name="expr">The expression to get the parameter name from</param>
        /// <returns>The paramter name</returns>
        public string GetParameterName(Expression expr)
        {
            // check the expression for null
            if (expr == null)
                throw new ArgumentNullException("propertyRefExpr", "propertyRefExpr is null.");

            // check for member expressions
            if (expr is MemberExpression)
            {
                // get member expression
                var memberExpression = (MemberExpression)expr;

                // check if we have a parameter expression
                if (memberExpression.Expression is ParameterExpression)
                {
                    // if so return name
                    return ((ParameterExpression)memberExpression.Expression).Name;
                }
            }

            if (expr is MethodCallExpression)
            {
                var call = (MethodCallExpression)expr;
                if (call.Object == null) return GetParameterName(call.Arguments[0]);

                return GetParameterName(call.Object);
            }

            // if the expression is not a lambda, return nothing
            if (!(expr is LambdaExpression)) return null;

            // get lambda and parameter
            var lambdaExpr = (LambdaExpression)expr;
            var param = (ParameterExpression)lambdaExpr.Parameters[0];

            // if no parameter, return null
            if (param == null)
                return null;

            // return parameter name
            return param.Name;
        }

        /// <summary>
        /// Get the table/type name
        /// </summary>
        /// <typeparam name="TObject">The type to get the name for</typeparam>
        /// <returns>The name of the type or table</returns>
        public string GetTypeName<TObject>()
        {
            // use the reflection helper to get the table name
            // NOTE: this will use the [TableName] attribute if present
            return reflectionHelper.GetTableName(typeof(TObject));
        }

        public EqualityOperator ConvertExpressionTypeToEqualityOperator(ExpressionType type)
        {
            // convert expression EqualityOperator into our EqualityOperator
            switch (type)
            {
                case ExpressionType.Equal:
                    return EqualityOperator.Equals;
                case ExpressionType.GreaterThan:
                    return EqualityOperator.GreaterThan;
                case ExpressionType.GreaterThanOrEqual:
                    return EqualityOperator.GreaterThanOrEqualTo;
                case ExpressionType.LessThan:
                    return EqualityOperator.LessThan;
                case ExpressionType.LessThanOrEqual:
                    return EqualityOperator.LessThanOrEqualTo;
                case ExpressionType.NotEqual:
                    return EqualityOperator.NotEquals;
            }

            // if nothing found, default to equals
            return EqualityOperator.Equals;
        }

        public IEnumerable<DynamicPropertyInfo> GetDynamicPropertyInformation(Expression expr)
        {
            DynamicPropertyInfo[] properties;
            var expression = (LambdaExpression)expr;

            if(expression.Body is MemberInitExpression)
            {
                var initExpr = (MemberInitExpression)expression.Body;

                var countBindings = initExpr.Bindings.Count();
                properties = new DynamicPropertyInfo[countBindings];

                var index = 0;
                foreach (var b in initExpr.Bindings)
                {
                    var memberBinding = (MemberAssignment)b;
                    var memberExpression = (MemberExpression)memberBinding.Expression;
                    var parameterExpression = (ParameterExpression)memberExpression.Expression;
                    var column = GetName(memberExpression);
                    var alias = GetParameterName(memberExpression);
                    var newName = memberBinding.Member.Name;

                    properties[index] = DynamicPropertyInfo.Create(column, newName, type: parameterExpression.Type);

                    index += 1;
                }
            }
            else
            {
                var body = (NewExpression)expression.Body;

                var countArguments = body.Arguments.Count;
                properties = new DynamicPropertyInfo[countArguments];

                var index = 0;
                foreach (var a in body.Arguments)
                {

                    var memberExpression = (MemberExpression)a;
                    var parameterExpression = (ParameterExpression)memberExpression.Expression;
                    var table = this.reflectionHelper.GetTableName(parameterExpression.Type);
                    var column = GetName(memberExpression);
                    var alias = GetParameterName(a);

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
            }

            return properties.ToList();
        }

        public object GetValue(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                try
                {
                    expression = convertAccess.GetExpression(expression);
                }
                catch
                {
                    return null;
                }
            }


            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return memberAccess.GetValue(expression);
            }
            else if (expression.NodeType == ExpressionType.Constant)
            {
                return constantAccess.GetValue(expression);
            }
            else if (expression.NodeType == ExpressionType.Lambda)
            {
                var body = ((LambdaExpression)expression).Body;
                return GetValue(body);
            }
            else if (expression.NodeType == ExpressionType.Call)
            {
                object result = null;
                try
                {
                    result = Expression.Lambda(expression).Compile().DynamicInvoke();
                }
                catch { }

                return result;
            }

            return null;
        }

        public Type GetType(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                return convertAccess.GetType(expression, this);
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return memberAccess.GetType(expression);
            }
            else if (expression.NodeType == ExpressionType.Constant)
            {
                return constantAccess.GetType(expression);
            }
            else if(expression.NodeType == ExpressionType.Parameter)
            {
                return parameterAccess.GetType(expression);
            }
            else if (expression.NodeType == ExpressionType.Lambda)
            {
                var body = ((LambdaExpression)expression).Body;
                return GetType(body);
            }

            return null;
        }

        public state.IWhere ParseWhereExpression(Expression expr)
        {
            // check to see if we have a normal where
            if (expr.NodeType != ExpressionType.OrElse && expr.NodeType != ExpressionType.AndAlso)
            {
                return ParseWhere(expr, false);
            }

            // check for and
            if (expr.NodeType == ExpressionType.AndAlso)
            {
                // get binary expression
                var binaryExpr = (BinaryExpression)expr;

                // get left and right
                var left = ParseWhereExpression(binaryExpr.Left);
                var right = ParseWhereExpression(binaryExpr.Right);

                // return and
                return And.Where(left, right);
            }
            // check for or
            else if (expr.NodeType == ExpressionType.OrElse)
            {
                // get binary expression
                var binaryExpr = (BinaryExpression)expr;

                // get left and right
                var left = ParseWhereExpression(binaryExpr.Left);
                var right = ParseWhereExpression(binaryExpr.Right);

                // return or
                return Or.Where(left, right);
            }

            // otherwise return nothing
            return null;
        }

        public state.IWhere ParseWhere(Expression expr, bool excludeAlias)
        {
            BinaryExpression binaryExpr = null;

            // check if we have a lambda
            if (expr.NodeType == ExpressionType.Lambda)
            {
                var lambda = (LambdaExpression)expr;

                // check for call expression on field
                if (lambda.Body.NodeType == ExpressionType.Call)
                    return callExpressionHelper.ParseCallExpressions(lambda.Body);

                binaryExpr = (BinaryExpression)lambda.Body;
            }
            // check if we have a method call
            else if (expr.NodeType == ExpressionType.Call)
            {
                // parse call expressions
                return callExpressionHelper.ParseCallExpressions(expr);
            }
            else
            {
                // otherwise make it a binary expression
                if (expr is not MemberExpression) binaryExpr = (BinaryExpression)expr;
            }

            if (binaryExpr == null) return ParseValue(expr, excludeAlias);

            IWhere left, right;

            if (binaryExpr.NodeType == ExpressionType.AndAlso)
            {
                // get left and right
                left = ParseWhere(binaryExpr.Left, excludeAlias);
                right = ParseWhere(binaryExpr.Right, excludeAlias);

                // return and
                return And.Where(left, right);
            }
            else if (binaryExpr.NodeType == ExpressionType.OrElse)
            {
                // get left and right
                left = ParseWhere(binaryExpr.Left, excludeAlias);
                right = ParseWhere(binaryExpr.Right, excludeAlias);

                // return or
                return Or.Where(left, right);
            }

            var operationTuple = UpdateNullableExpression(binaryExpr);
            var leftExpression = operationTuple.Item1;
            var rightExpression = operationTuple.Item2;

            var leftType = leftExpression.NodeType;
            var rightType = rightExpression.NodeType;

            var leftParam = this.GetParameterName(leftExpression);
            var rightParam = this.GetParameterName(rightExpression);
            var bothMemberAccess = leftType == ExpressionType.MemberAccess &&
                                    rightType == ExpressionType.MemberAccess;
            var bothHaveParam = !string.IsNullOrWhiteSpace(leftParam) &&
                                !string.IsNullOrWhiteSpace(rightParam);

            if (bothMemberAccess && bothHaveParam) return ParseJoin(expr);

            // failing that parse a value clause
            return ParseValue(expr, excludeAlias);
        }

        private state.IWhere ParseJoin(Expression expr)
        {
            BinaryExpression binaryExpr = null;
            LambdaExpression lambdaExpr = null;

            if (expr.NodeType == ExpressionType.Lambda)
            {
                lambdaExpr = (LambdaExpression)expr;
                binaryExpr = (BinaryExpression)lambdaExpr.Body;
            }
            else
            {
                if (expr is not MemberExpression) binaryExpr = (BinaryExpression)expr;
            }

            if (binaryExpr != null && binaryExpr.NodeType == ExpressionType.AndAlso)
            {
                var left = ParseJoin(binaryExpr.Left);
                var right = ParseJoin(binaryExpr.Right);

                return And.Where(left, right);
            }
            else if (binaryExpr != null && binaryExpr.NodeType == ExpressionType.OrElse)
            {
                var left = ParseJoin(binaryExpr.Left);
                var right = ParseJoin(binaryExpr.Right);

                return Or.Where(left, right);
            }

            EqualityOperator op;

            if (expr is BinaryExpression)
            {
                BinaryExpression operation = (BinaryExpression)expr;

                var operationTuple = UpdateNullableExpression(operation);
                var left = operationTuple.Item1;
                var right = operationTuple.Item2;

                //start with left
                var leftField = this.GetName(left);
                var rightField = this.GetName(right);

                //get operation
                op = this.ConvertExpressionTypeToEqualityOperator(operation.NodeType);

                // create column
                return Conditionals.Column.Match(
                    Column.Create(leftField, GetQueryTarget(left)),
                    op,
                    Column.Create(rightField, GetQueryTarget(right)));
            }
            else if (expr is LambdaExpression && lambdaExpr != null)
            {
                BinaryExpression operation = binaryExpr;
                MemberExpression left;
                MemberExpression right;

                if (operation.Left is UnaryExpression)
                {
                    var unaryLeft = (UnaryExpression)operation.Left;
                    left = unaryLeft.Operand as MemberExpression;
                }
                else
                {
                    left = (MemberExpression)operation.Left;
                }

                if (operation.Right is UnaryExpression)
                {
                    var unaryRight = (UnaryExpression)operation.Right;
                    right = unaryRight.Operand as MemberExpression;
                }
                else
                {
                    right = (MemberExpression)operation.Right;
                }

                var leftParam = (ParameterExpression)left.Expression;
                var rightParam = (ParameterExpression)right.Expression;

                return Conditionals.Column.Match(
                    Column.Create(
                        reflectionHelper.GetFieldName(left.Member),
                        GetQueryTarget(leftParam)),
                    this.ConvertExpressionTypeToEqualityOperator(operation.NodeType),
                    Column.Create(
                        reflectionHelper.GetFieldName(right.Member),
                        GetQueryTarget(rightParam)));
            }

            return null;
        }

        private IQueryTarget GetQueryTarget(Expression expression)
        {
            var alias = GetParameterName(expression);
            var table = this.reflectionHelper.GetTableName(expression.Type);
            var managedAlias = this.aliasManager.FindByAssociation(table)?.FirstOrDefault()?.Name ?? alias;
            managedAlias = this.aliasManager.Add(managedAlias, table);
            var existing = this.context.QueryTargets.FirstOrDefault(t => t.Alias == managedAlias);

            if (existing != null) return existing;

            existing = QueryTargets.TableTarget.Create(managedAlias, managedAlias);
            this.context.AddQueryTarget(existing);
            return existing;
        }


        /// <summary>
        /// This is used for the case where we are doing a column.Match<T1,T2>((t1,t2) => t1.someNullable == t2.nonNull)
        /// We check for the null-able and then get the underlying expression to join against
        /// </summary>
        /// <param name="operation">The operation to check for null ables</param>
        /// <returns>A Tuple of Left,Right Expression</returns>
        private static Tuple<Expression, Expression> UpdateNullableExpression(
            BinaryExpression operation)
        {
            var left = operation.Left;
            var right = operation.Right;
            if (left.NodeType == ExpressionType.Convert)
            {
                var temp = (UnaryExpression)left;
                left = temp.Operand;
            }

            if (right.NodeType == ExpressionType.Convert)
            {
                var temp = (UnaryExpression)right;
                right = temp.Operand;
            }

            return new Tuple<Expression, Expression>(left, right);
        }

        private state.IWhere ParseValue(
            Expression expr,
            bool excludeAlias)
        {
            object val;
            Type valType;
            string field, table;
            EqualityOperator op;

            if (expr is MemberExpression)
            {
                val = this.GetValue(expr);
                valType = this.GetType(expr);
                field = this.GetName(expr);
                table = this.GetParameterName(expr);
                op = EqualityOperator.Equals;
            }
            else
            {
                BinaryExpression operation;
                if (expr.NodeType == ExpressionType.Lambda)
                {
                    var lambda = (LambdaExpression)expr;
                    operation = (BinaryExpression)lambda.Body;
                }
                else
                {
                    operation = (BinaryExpression)expr;
                }

                var left = operation.Left;
                var right = operation.Right;

                if (left.NodeType == ExpressionType.Call ||
                    right.NodeType == ExpressionType.Call)
                {
                    return callExpressionHelper.ParseBinaryCallExpressions(operation);
                }

                //start with left
                val = this.GetValue(operation.Left);
                valType = this.GetType(operation.Left);
                field = this.GetName(operation.Left);
                table = this.GetParameterName(operation.Left);

                //then right
                if (val == null) val = this.GetValue(operation.Right);
                if (valType == null) valType = val == null ? this.GetType(operation.Right) : val.GetType();
                if (String.IsNullOrEmpty(field)) field = this.GetName(operation.Right);
                if (String.IsNullOrEmpty(table)) table = this.GetParameterName(operation.Right);

                //get table if not already present
                if (String.IsNullOrEmpty(table)) table = this.GetParameterName(expr);

                //get operation
                op = this.ConvertExpressionTypeToEqualityOperator(operation.NodeType);
            }

            var toCreate = typeof(Column<>);
            var col = state.Column.Create(field, state.QueryTargets.TableTarget.Create(table));

            //add the model type for the type def of the repository
            Type[] args = { valType };

            object convertedValue;

            if (valType.IsEnum)
            {
                convertedValue = Enum.ToObject(valType, val);
            }
            else
            {
                if (val == null && valType == typeof(bool))
                {
                    convertedValue = true;
                }
                else if (val == null)
                {
                    convertedValue = null;
                }
                else
                {
                    try
                    {
                        convertedValue = Convert.ChangeType(val, valType);
                    }
                    catch
                    {
                        var underlyingType = reflectionHelper.GetUnderlyingType(valType);
                        convertedValue = Convert.ChangeType(val, underlyingType);
                    }
                }
            }

            var parameters = new[] { col, op, convertedValue };

            //get the generic type definition for the model
            Type constructedField = toCreate.MakeGenericType(args);
            //create instance of the repository, typed for the model
            object instanceField = Activator.CreateInstance(constructedField, parameters);
            return (state.IWhere)instanceField;
        }
    }
}
