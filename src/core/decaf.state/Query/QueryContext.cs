using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using decaf.common;
using decaf.common.Utilities;
using decaf.common.Utilities.Reflection;
using decaf.common.Utilities.Reflection.Dynamic;
using decaf.state.Utilities;
using decaf.state.Utilities.Parsers;

namespace decaf.state
{
    public abstract class QueryContext : IQueryContextExtended
    {
        private readonly IExpressionHelper expressionHelper;
        private readonly IReflectionHelper reflectionHelper;
        private readonly IDynamicExpressionHelper dynamicExpressionHelper;
        private readonly IAliasManager aliasManager;
        private readonly IQueryParsers parserHolder;
        private readonly List<IQueryTarget> queryTargets;
        private readonly IHashProvider hashProvider;

        protected QueryContext(
            IAliasManager aliasManager,
            QueryTypes kind,
            IHashProvider hashProvider)
        {
            Id = Guid.NewGuid();
            Kind = kind;
            this.queryTargets = new List<IQueryTarget>();
            this.reflectionHelper = new ReflectionHelper();
            this.expressionHelper = new ExpressionHelper(this.reflectionHelper);
            var valueFunctionHelper = new ValueFunctionHelper(this.expressionHelper);
            this.dynamicExpressionHelper = new DynamicExpressionHelper(expressionHelper, valueFunctionHelper);
            this.aliasManager = aliasManager;
            var callExpressionHelper = new CallExpressionHelper(this.expressionHelper, valueFunctionHelper);
            this.parserHolder = new ParserHolder(expressionHelper, reflectionHelper, callExpressionHelper);
            this.hashProvider = hashProvider;
        }

        /// <inheritdoc/>
        public Guid Id { get; private set; }

        /// <inheritdoc/>
        public QueryTypes Kind { get; private set; }

        /// <inheritdoc/>
        public string GetHash() => this.hashProvider.GetHash(this);

        /// <inheritdoc/>
        public IReadOnlyCollection<IQueryTarget> QueryTargets => this.queryTargets;

        /// <inheritdoc/>
        IExpressionHelper IQueryContextExtended.ExpressionHelper => this.expressionHelper;

        /// <inheritdoc/>
        IReflectionHelper IQueryContextExtended.ReflectionHelper => this.reflectionHelper;

        /// <inheritdoc/>
        IQueryParsers IQueryContextExtended.Parsers => this.parserHolder;

        /// <inheritdoc/>
        IAliasManager IQueryContextExtended.AliasManager => this.aliasManager;

        /// <inheritdoc/>
        IDynamicExpressionHelper IQueryContextExtended.DynamicExpressionHelper => this.dynamicExpressionHelper;

        /// <inheritdoc/>
        void IQueryContextExtended.AddQueryTarget(IQueryTarget target)
            => this.queryTargets.Add(target);

        /// <inheritdoc/>
        IQueryTarget IQueryContextExtended.GetQueryTarget(Expression expression)
        {
            GetAliasAndTable(expression, out var alias, out var table);
            var managedAlias = this.aliasManager.FindByAssociation(table)?.FirstOrDefault()?.Name ?? alias;
            managedAlias = this.aliasManager.Add(managedAlias, table);

            return this.queryTargets.FirstOrDefault(t => t.Alias == managedAlias);
        }

        /// <inheritdoc/>
        IQueryTarget IQueryContextExtended.GetQueryTarget(string alias)
        {
            return this.queryTargets.FirstOrDefault(qt => qt.Alias == alias);
        }

        /// <inheritdoc/>
        IQueryTarget IQueryContextExtended.AddQueryTarget(Expression expression)
        {
            var self = this as IQueryContextExtended;
            var existing = self.GetQueryTarget(expression);
            if (existing != null) return existing;

            GetAliasAndTable(expression, out var alias, out var table);
            var target = state.QueryTargets.TableTarget.Create(table, alias);
            self.AddQueryTarget(target);
            return target;
        }

        private void GetAliasAndTable(Expression expression, out string alias, out string table)
        {
            alias = table = null;
            Expression exprAlias, exprTable;
            exprAlias = exprTable = expression;

            if (expression is LambdaExpression)
            {
                var lambda = expression as LambdaExpression;
                if (lambda.Parameters.Count > 1) return;
                exprAlias = lambda.Body;
                exprTable = lambda.Body;
            }

            if (exprAlias is MethodCallExpression)
            {
                var call = exprAlias as MethodCallExpression;
                var obj = call.Object as MemberExpression;
                exprAlias = obj;
                exprTable = obj.Expression;
            }

            if (expression is ParameterExpression)
            {
                var paramExpr = expression as ParameterExpression;
                alias = paramExpr.Name;
                table = this.reflectionHelper.GetTableName(paramExpr.Type);
                return;
            }

            if (expression is MemberExpression)
            {
                var memberExpr = expression as MemberExpression;
                alias = this.expressionHelper.GetParameterName(exprAlias);
                table = this.reflectionHelper.GetTableName(memberExpr.Member.DeclaringType);
                return;
            }

            alias = this.expressionHelper.GetParameterName(exprAlias);
            table = this.reflectionHelper.GetTableName(exprTable.Type);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
            => this.queryTargets.DisposeAll();
    }
}

