using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.Exceptions;
using pdq.state;

namespace pdq.Implementation
{
    internal abstract class InsertBase : Execute<IInsertQueryContext>
    {
        protected InsertBase(IQueryInternal query, IInsertQueryContext context)
            : base(query, context)
        {
            this.query.SetContext(this.context);
        }

        protected void FromQuery(Action<ISelect> query)
        {
            var context = SelectQueryContext.Create(this.query.AliasManager);
            var select = Select.Create(context, this.query);

            query(select);
            var source = state.ValueSources.Insert.QueryValuesSource.Create(context);
            this.context.From(source);
        }

        protected void AddValues<T>(T value)
        {
            var internalContext = this.context as IQueryContextInternal;
            var properties = internalContext.ReflectionHelper.GetMemberNames(value);
            var values = properties.Select(p => internalContext.ReflectionHelper.GetPropertyValue(value, p));
            var row = values.ToArray();
            this.context.Value(row);
        }

        protected void AddValues<T>(IEnumerable<T> values)
        {
            foreach (var v in values) AddValues(v);
        }

        protected void AddColumns(Expression expression)
        {
            var properties = this.context.Helpers().GetPropertyInformation(expression);
            foreach (var p in properties)
            {
                var target = p.Type is object ?
                    this.context.QueryTargets.FirstOrDefault(t => t.Alias == p.Alias) :
                    GetQueryTarget(p.Type);
                this.context.Column(state.Column.Create(p.Name, target, p.NewName));
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
            if (alias == null) throw new TableNotFoundException(alias.Name, table);

            var target = this.context.QueryTargets.FirstOrDefault(t => t.Alias == alias.Name);
            if (target == null) throw new TableNotFoundException(alias.Name, table);
            return target;
        }
    }
}

