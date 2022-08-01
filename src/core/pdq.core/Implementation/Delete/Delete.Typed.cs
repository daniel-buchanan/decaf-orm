using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
	internal class Delete<T> : Execute<IDeleteQueryContext>, IDeleteFrom<T>
	{
        private Delete(
            IDeleteQueryContext context,
            IQueryInternal query,
            ISqlFactory sqlFactory)
            : base(query, context, sqlFactory)
        {
            this.context = context;
        }

        public static IDeleteFrom<T> Create(
            IDeleteQueryContext context,
            IQueryInternal query,
            ISqlFactory sqlFactory)
            => new Delete<T>(context, query, sqlFactory);

        /// <inheritdoc />
        public IDeleteFrom<T> Where(Expression<Func<T, bool>> whereExpression)
        {
            var clause = this.context.Helpers().ParseWhere(whereExpression);
            this.context.Where(clause);
            return this;
        }
    }
}

