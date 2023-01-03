using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using pdq.common;
using pdq.Exceptions;
using pdq.state;
using pdq.state.Utilities;

namespace pdq.Implementation
{
	internal abstract class SelectBase : Execute<ISelectQueryContext>
	{
        protected readonly PdqOptions options;
        protected new readonly ISelectQueryContext context;

        protected SelectBase(
            ISelectQueryContext context,
            IQuery query)
            : base((IQueryInternal)query, context)
        {
            this.options = (query as IQueryInternal).Options;
            this.context = context;
            this.query.SetContext(this.context);
        }

        protected void AddColumns(Expression expression)
        {
            var properties = this.context.Helpers().GetPropertyInformation(expression);
            foreach (var p in properties)
            {
                var target = p.Type is object ?
                    this.context.QueryTargets.FirstOrDefault(t => t.Alias == p.Alias) :
                    GetQueryTarget(p.Type);
                this.context.Select(state.Column.Create(p.Name, target, p.NewName));
            }
        }

        protected IQueryTarget GetQueryTarget(Type type)
        {
            var table = this.context.Helpers().GetTableName(type);
            return GetQueryTarget(table);
        }

        protected IQueryTarget GetQueryTarget(Expression expression)
        {
            var table = this.context.Helpers().GetTableName(expression);
            return GetQueryTarget(table);
        }

        protected IQueryTarget GetQueryTarget(string table)
        {
            var alias = this.query.AliasManager.FindByAssociation(table).FirstOrDefault();
            if (alias == null) throw new TableNotFoundException(table);

            var target = this.context.QueryTargets.FirstOrDefault(t => t.Alias == alias.Name);
            if (target == null) throw new TableNotFoundException(alias.Name, table);
            return target;
        }

        protected void AddAllColumns<T>(Expression<Func<T, object>> expression)
        {
            var param = this.context.Helpers().GetTableAlias(expression);
            AddAllColumns<T>(param);
        }

        protected void AddAllColumns<T>(string alias)
        {
            var entityType = typeof(T);
            var internalContext = this.context as IQueryContextInternal;
            
            var members = new List<PropertyInfo>();
            var arguments = new List<Expression>();
            var parameterExpression = Expression.Parameter(typeof(ISelectColumnBuilder), "b");
            var properties = internalContext.ReflectionHelper.GetMemberDetails(entityType, QueryTypes.Select);
            var emptyCtor = new DynamicConstructorInfo(properties, typeof(T));
            var isMethod = typeof(ISelectColumnBuilder).GetMethods().FirstOrDefault(m => m.IsGenericMethod && m.GetParameters().Count() == 2);

            if(isMethod == null)
            {
                throw new ShouldNeverOccurException("The method .Is<T>(column, alias) could not be found on `ISelectColumnBuilder`, this should never happen!");
            }    

            foreach (var p in properties)
            {
                var typedIsMethod = isMethod.MakeGenericMethod(p.ValueType);
                var nameExpression = Expression.Constant(p.NewName);
                var aliasExpression = Expression.Constant(alias);
                var methodCallExpression = Expression.Call(parameterExpression, typedIsMethod, nameExpression, aliasExpression);
                var member = new DynamicPropertyInfo(p.Name, p.ValueType, typeof(T));
                members.Add(member);
                arguments.Add(methodCallExpression);
            }

            var bodyExpression = Expression.New(emptyCtor, arguments, members);
            var lambdaExpression = (Expression<Func<ISelectColumnBuilder, T>>)Expression.Lambda(bodyExpression, parameterExpression);
            AddColumns(lambdaExpression);
        }
    }
}

