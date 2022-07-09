using System;
using System.Collections.Generic;
using pdq.common;
using pdq.state.Utilities;

namespace pdq.state
{
	internal abstract class QueryContext : IQueryContext, IQueryContextInternal
	{
		private readonly IAliasManager aliasManager;
		private readonly IExpressionHelper expressionHelper;
		private readonly IReflectionHelper reflectionHelper;
		protected readonly List<IQueryTarget> queryTargets;

		protected QueryContext(
			IAliasManager aliasManager,
			QueryTypes kind)
		{
			Id = Guid.NewGuid();
			Kind = kind;
			this.aliasManager = aliasManager;
			this.queryTargets = new List<IQueryTarget>();
			this.reflectionHelper = new ReflectionHelper();
			this.expressionHelper = new ExpressionHelper(
				this.reflectionHelper,
				this.aliasManager,
				this);
		}

		/// <inheritdoc/>
        public Guid Id { get; private set; }

		/// <inheritdoc/>
		public QueryTypes Kind { get; private set; }

		/// <inheritdoc/>
		public IEnumerable<IQueryTarget> QueryTargets => this.queryTargets;

		/// <inheritdoc/>
		IExpressionHelper IQueryContextInternal.ExpressionHelper => this.expressionHelper;

		/// <inheritdoc/>
		IReflectionHelper IQueryContextInternal.ReflectionHelper => this.reflectionHelper;

		/// <inheritdoc/>
		void IQueryContextInternal.AddQueryTarget(IQueryTarget target)
			=> this.queryTargets.Add(target);

		/// <inheritdoc/>
		public abstract void Dispose();
    }
}

