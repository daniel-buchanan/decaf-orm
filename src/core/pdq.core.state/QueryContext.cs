using System;
using System.Collections.Generic;
using pdq.common;
using pdq.state.Utilities;
using pdq.state.Utilities.Parsers;

namespace pdq.state
{
	internal abstract class QueryContext : IQueryContextInternal
	{
		private readonly IExpressionHelper expressionHelper;
		private readonly IReflectionHelper reflectionHelper;
		private readonly ParserHolder parserHolder;
		private readonly List<IQueryTarget> queryTargets;

		protected QueryContext(
			IAliasManager aliasManager,
			QueryTypes kind)
		{
			Id = Guid.NewGuid();
			Kind = kind;
			this.queryTargets = new List<IQueryTarget>();
			this.reflectionHelper = new ReflectionHelper();
			this.expressionHelper = new ExpressionHelper(this.reflectionHelper);
			this.parserHolder = new ParserHolder(expressionHelper, reflectionHelper, aliasManager, this, new CallExpressionHelper(expressionHelper, reflectionHelper, aliasManager));
		}

		/// <inheritdoc/>
        public Guid Id { get; private set; }

		/// <inheritdoc/>
		public QueryTypes Kind { get; private set; }

		/// <inheritdoc/>
		public IReadOnlyCollection<IQueryTarget> QueryTargets => this.queryTargets;

		/// <inheritdoc/>
		IExpressionHelper IQueryContextInternal.ExpressionHelper => this.expressionHelper;

		/// <inheritdoc/>
		IReflectionHelper IQueryContextInternal.ReflectionHelper => this.reflectionHelper;

        /// <inheritdoc/>
        IQueryParsers IQueryContextInternal.Parsers => this.parserHolder;

        /// <inheritdoc/>
        void IQueryContextInternal.AddQueryTarget(IQueryTarget target)
			=> this.queryTargets.Add(target);

		/// <inheritdoc/>
		public void Dispose()
        {
			Dispose(true);
			GC.SuppressFinalize(this);
        }

		protected virtual void Dispose(bool disposing)
        {
			this.queryTargets.DisposeAll();
        }
    }
}

