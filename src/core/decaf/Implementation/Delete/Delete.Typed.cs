﻿using System;
using System.Linq.Expressions;
using decaf.common;
using decaf.state;

namespace decaf.Implementation
{
	internal class Delete<T> :
        Execute<IDeleteQueryContext>,
        IDeleteFrom<T>
	{
        private Delete(
            IDeleteQueryContext context,
            IQueryContainerInternal query)
            : base(query, context)
        {
            this.context = context;
        }

        public static IDeleteFrom<T> Create(
            IDeleteQueryContext context,
            IQueryContainerInternal query)
            => new Delete<T>(context, query);

        /// <inheritdoc/>
        public IDeleteFrom<T> Output(Expression<Func<T, object>> column)
        {
            var internalContext = this.context as IQueryContextInternal;
            var columnName = internalContext.ExpressionHelper.GetMemberName(column);
            var col = state.Column.Create(columnName, this.context.Table);
            this.context.Output(state.Output.Create(col, OutputSources.Deleted));
            return this;
        }

        /// <inheritdoc />
        public IDeleteFrom<T> Where(Expression<Func<T, bool>> whereExpression)
        {
            var clause = this.context.Helpers().ParseWhere(whereExpression);
            this.context.Where(clause);
            return this;
        }
    }
}

