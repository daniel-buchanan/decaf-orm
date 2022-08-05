using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
	internal class Insert<T> :
        InsertBase,
		IInsertInto<T>,
		IInsertValues<T>
	{
        private Insert(IQueryInternal query, IInsertQueryContext context)
            : base(query, context) { }

        public static Insert<T> Create(IInsertQueryContext context, IQueryInternal query)
            => new Insert<T>(query, context);

        public IInsertValues<T> Columns(Expression<Func<T, dynamic>> columns)
        {
            base.AddColumns(columns);
            return this;
        }

        public IInsertValues<T> From(Action<ISelect> query)
        {
            base.FromQuery(query);
            return this;
        }

        public IInsertValues<T> Value(T value)
        {
            base.AddValues<T>(new[] { value });
            return this;
        }

        public IInsertValues<T> Values(IEnumerable<T> values)
        {
            base.AddValues<T>(values);
            return this;
        }
    }
}

