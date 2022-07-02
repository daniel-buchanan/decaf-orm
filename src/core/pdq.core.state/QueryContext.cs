using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;
using pdq.state.Utilities;

namespace pdq.state
{
	public abstract class QueryContext : IQueryContext, IQueryContextInternal
	{
		private readonly IAliasManager aliasManager;
		private readonly IExpressionHelper expressionHelper;
		private readonly IReflectionHelper reflectionHelper;
		protected readonly List<IQueryTarget> queryTargets;

		protected QueryContext(
			IAliasManager aliasManager,
			QueryType kind)
		{
			Id = Guid.NewGuid();
			Kind = kind;
			this.aliasManager = aliasManager;
			this.queryTargets = new List<IQueryTarget>();
			this.reflectionHelper = new ReflectionHelper();
			this.expressionHelper = new ExpressionHelper(this.reflectionHelper);
		}

        public Guid Id { get; private set; }

        public QueryType Kind { get; private set; }

        public IEnumerable<IQueryTarget> QueryTargets => this.queryTargets;

        IExpressionHelper IQueryContextInternal.ExpressionHelper => this.expressionHelper;

        IReflectionHelper IQueryContextInternal.ReflectionHelper => this.reflectionHelper;

        public abstract void Dispose();
    }
}

