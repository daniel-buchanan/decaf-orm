using System;
using System.Collections.Generic;
using pdq.common;
using pdq.state.Utilities;
using pdq.state.Utilities.Parsers;

namespace pdq.state
{
	internal abstract class QueryContext : IDisposable, IQueryContextInternal
	{
		private readonly IExpressionHelper expressionHelper;
		private readonly IReflectionHelper reflectionHelper;
		private readonly ParserHolder parserHolder;
		protected readonly List<IQueryTarget> queryTargets;

		protected QueryContext(
			IAliasManager aliasManager,
			QueryTypes kind)
		{
			Id = Guid.NewGuid();
			Kind = kind;
			this.queryTargets = new List<IQueryTarget>();
			this.reflectionHelper = new ReflectionHelper();
			this.expressionHelper = new ExpressionHelper(
				this.reflectionHelper,
				aliasManager,
				this);
			this.parserHolder = new ParserHolder(expressionHelper, reflectionHelper, aliasManager, this, new CallExpressionHelper(expressionHelper));
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

		IQueryParsers IQueryContextInternal.Parsers => this.parserHolder;

        Guid IQueryContext.Id => throw new NotImplementedException();

        QueryTypes IQueryContext.Kind => throw new NotImplementedException();

        IEnumerable<IQueryTarget> IQueryContext.QueryTargets => throw new NotImplementedException();

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
			// nothing to do here
        }
    }
}

