using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
	internal class Delete<T> : Execute, IDeleteFrom<T>
	{
        private readonly IDeleteQueryContext context;

        private Delete(
            IDeleteQueryContext context,
            IQueryInternal query) : base(query)
        {
            this.context = context;
        }

        public static IDeleteFrom<T> Create(IDeleteQueryContext context, IQueryInternal query)
            => new Delete<T>(context, query);

        /// <inheritdoc />
        public IDeleteFrom<T> Where(Expression<Func<T, bool>> whereExpression)
        {
            var clause = this.context.Helpers().ParseWhere(whereExpression);
            this.context.Where(clause);
            return this;
        }
    }
}

