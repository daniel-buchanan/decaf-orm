using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state.Conditionals;

namespace pdq.state.Utilities
{
    internal class ExpressionHelper : IExpressionHelper
    {
        private readonly ConstantAccess _constantAccess;
        private readonly MemberAccess _memberAccess;
        private readonly ConvertAccess _convertAccess;
        private readonly IReflectionHelper _reflectionHelper;
        private readonly CallExpressionHelper _callExpressionHelper;

        public ExpressionHelper(IReflectionHelper reflectionHelper)
        {
            // setup helpers
            _constantAccess = new ConstantAccess();
            _convertAccess = new ConvertAccess();
            _reflectionHelper = reflectionHelper;
            _memberAccess = new MemberAccess(_reflectionHelper);
            _callExpressionHelper = new CallExpressionHelper(this);
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
                    return _convertAccess.GetName(expression, this);
                case ExpressionType.MemberAccess:
                    return _memberAccess.GetName(expression);
                case ExpressionType.Constant:
                    return _memberAccess.GetName(expression);
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
            return _reflectionHelper.GetTableName(typeof(TObject));
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

        public object GetValue(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                try
                {
                    expression = _convertAccess.GetExpression(expression);
                }
                catch
                {
                    return null;
                }
            }


            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return _memberAccess.GetValue(expression);
            }
            else if (expression.NodeType == ExpressionType.Constant)
            {
                return _constantAccess.GetValue(expression);
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
                return _convertAccess.GetType(expression, this);
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return _memberAccess.GetType(expression);
            }
            else if (expression.NodeType == ExpressionType.Constant)
            {
                return _constantAccess.GetType(expression);
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
                    return _callExpressionHelper.ParseCallExpressions(lambda.Body);

                binaryExpr = (BinaryExpression)lambda.Body;
            }
            // check if we have a method call
            else if (expr.NodeType == ExpressionType.Call)
            {
                // parse call expressions
                return _callExpressionHelper.ParseCallExpressions(expr);
            }
            else
            {
                // otherwise make it a binary expression
                if (!(expr is MemberExpression)) binaryExpr = (BinaryExpression)expr;
            }

            // check for and
            if (binaryExpr != null && binaryExpr.NodeType == ExpressionType.AndAlso)
            {
                // get left and right
                var left = ParseWhere(binaryExpr.Left, excludeAlias);
                var right = ParseWhere(binaryExpr.Right, excludeAlias);

                // return and
                return And.Where(left, right);
            }
            // check for or
            else if (binaryExpr != null && binaryExpr.NodeType == ExpressionType.OrElse)
            {
                // get left and right
                var left = ParseWhere(binaryExpr.Left, excludeAlias);
                var right = ParseWhere(binaryExpr.Right, excludeAlias);

                // return or
                return Or.Where(left, right);
            }

            if (binaryExpr != null)
            {
                var operationTuple = UpdateNullableExpression(binaryExpr);
                var left = operationTuple.Item1;
                var right = operationTuple.Item2;

                var leftType = left.NodeType;
                var rightType = right.NodeType;

                var leftParam = this.GetParameterName(left);
                var rightParam = this.GetParameterName(right);
                var bothMemberAccess = leftType == ExpressionType.MemberAccess &&
                                       rightType == ExpressionType.MemberAccess;
                var bothHaveParam = !string.IsNullOrWhiteSpace(leftParam) &&
                                    !string.IsNullOrWhiteSpace(rightParam);

                if (bothMemberAccess && bothHaveParam)
                {
                    return ParseJoin(expr);
                }
            }

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
                if (!(expr is MemberExpression)) binaryExpr = (BinaryExpression)expr;
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
            else
            {
                string leftField, leftTable, rightField, rightTable;
                leftField = rightField = String.Empty;
                leftTable = rightTable = String.Empty;

                EqualityOperator op;

                if (expr is BinaryExpression)
                {
                    BinaryExpression operation = (BinaryExpression)expr;

                    var operationTuple = UpdateNullableExpression(operation);
                    var left = operationTuple.Item1;
                    var right = operationTuple.Item2;

                    //start with left
                    leftField = this.GetName(left);
                    leftTable = this.GetParameterName(left);

                    //then right
                    rightField = this.GetName(right);
                    rightTable = this.GetParameterName(right);

                    //get table if not already present
                    if (String.IsNullOrEmpty(leftTable)) leftTable = this.GetParameterName(operation.Left);
                    if (string.IsNullOrEmpty(rightTable))
                        rightTable = this.GetParameterName(operation.Right);

                    //get operation
                    op = this.ConvertExpressionTypeToEqualityOperator(operation.NodeType);

                    // create column
                    return Conditionals.Column.Match(
                        Column.Create(leftField, QueryTargets.TableTarget.Create(leftTable)),
                        op,
                        Column.Create(rightField, QueryTargets.TableTarget.Create(rightTable)));
                }
                else if (expr is LambdaExpression)
                {
                    BinaryExpression operation = (BinaryExpression)lambdaExpr.Body;
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

                    var leftParam = ((ParameterExpression)left.Expression);
                    var rightParam = ((ParameterExpression)right.Expression);

                    return Conditionals.Column.Match(
                        Column.Create(
                            _reflectionHelper.GetFieldName(left.Member),
                            QueryTargets.TableTarget.Create(leftParam.Name)),
                        this.ConvertExpressionTypeToEqualityOperator(operation.NodeType),
                        Column.Create(
                            _reflectionHelper.GetFieldName(right.Member),
                            QueryTargets.TableTarget.Create(rightParam.Name)));
                }

                return null;
            }
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
            BinaryExpression binaryExpr = null;

            if (expr.NodeType == ExpressionType.Lambda)
            {
                var lambda = (LambdaExpression)expr;
                binaryExpr = (BinaryExpression)lambda.Body;
            }
            else
            {
                if (!(expr is MemberExpression)) binaryExpr = (BinaryExpression)expr;
            }

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
                BinaryExpression operation = binaryExpr;

                var left = operation.Left;
                var right = operation.Right;

                if (left.NodeType == ExpressionType.Call ||
                    right.NodeType == ExpressionType.Call)
                {
                    return _callExpressionHelper.ParseBinaryCallExpressions(operation);
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
                        var underlyingType = _reflectionHelper.GetUnderlyingType(valType);
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
