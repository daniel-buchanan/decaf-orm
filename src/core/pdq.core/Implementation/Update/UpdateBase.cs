using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;
using pdq.state.Utilities;

namespace pdq.Implementation
{
    internal abstract class UpdateBase : Execute<IUpdateQueryContext>
    {
        protected UpdateBase(IQueryInternal query, IUpdateQueryContext context)
            : base(query, context)
        {
            this.query.SetContext(this.context);
        }

        protected void FromQuery(Action<ISelectWithAlias> queryBuilder)
        {
            var context = SelectQueryContext.Create(this.query.AliasManager);
            var query = this.query.Transient.Query() as IQueryInternal;
            var select = Select.Create(context, query) as ISelectWithAlias;

            queryBuilder(select);
            var source = state.QueryTargets.SelectQueryTarget.Create(context, select.Alias);
            this.context.From(source);
        }

        protected void SetValues<T>(IEnumerable<T> values)
        {
            foreach(var v in values) SetValues(v);
        }

        protected void SetValues<T>(T value)
        {
            var internalContext = this.context as IQueryContextInternal;
            var paramExpression = Expression.Parameter(typeof(T), "p");
            var constExpression = Expression.Constant(value);
            var valueExpression = Expression.Lambda(constExpression, paramExpression);

            var props = internalContext.DynamicExpressionHelper
                .GetProperties(valueExpression, internalContext)
                .ToList();
            
            for(var i = 0; i < props.Count; i += 1)
            {
                var p = props[i];
                
                var column = Column.Create(p.Name, this.context.Table);
                var v = p.Value;
                var valueType = p.ValueType;
                var defaultValue = DefaultValueHelper.Get(valueType);
                if (v?.Equals(defaultValue) == true ||
                    (v == null && defaultValue == null))
                    continue;

                var source = state.ValueSources.Update.StaticValueSource.Create(column, valueType, v);
                this.context.Set(source);
            }
        }
    }
}

