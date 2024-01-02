using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
    internal abstract class InsertBase : Execute<IInsertQueryContext>
    {
        protected InsertBase(IQueryContainerInternal query, IInsertQueryContext context)
            : base(query, context)
        {
            this.query.SetContext(this.context);
        }

        protected void FromQuery(Action<ISelect> queryBuilder)
        {
            var context = SelectQueryContext.Create(this.query.AliasManager, this.query.HashProvider);
            var query = this.query.UnitOfWork.Query() as IQueryContainerInternal;
            var select = Select.Create(context, query);

            queryBuilder(select);
            var source = state.ValueSources.Insert.QueryValuesSource.Create(context);
            this.context.From(source);
        }

        protected void AddValues<T>(T value)
        {
            var internalContext = this.context as IQueryContextInternal;
            var properties = internalContext.ReflectionHelper.GetMemberDetails(value);
            var values = properties.Select(p => internalContext.ReflectionHelper.GetPropertyValue(value, p.NewName));
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
                var target = this.context.QueryTargets.FirstOrDefault(t => t.Alias == p.Alias);
                this.context.Column(state.Column.Create(p.Name, target, p.NewName));
            }
        }
    }
}

