using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common;

namespace pdq.state
{
	internal class DeleteQueryContext :
		QueryContext,
		IDeleteQueryContext
	{
        private readonly List<Output> outputs;

		private DeleteQueryContext(IAliasManager aliasManager)
			: base(aliasManager, QueryTypes.Delete)
        {
			WhereClause = null;
            this.outputs = new List<Output>();
        }

		/// <inheritdoc/>
		public ITableTarget Table => QueryTargets.FirstOrDefault() as ITableTarget;

		/// <inheritdoc/>
		public IWhere WhereClause { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<Output> Outputs => this.outputs.AsReadOnly();

        /// <inheritdoc/>
        public IDeleteQueryContext From(ITableTarget target)
        {
            var item = this.QueryTargets.FirstOrDefault(t => t.IsEquivalentTo(target));
            if (item != null) return this;

            var internalContext = this as IQueryContextInternal;
			internalContext.AddQueryTarget(target);
			return this;
        }

		/// <inheritdoc/>
		public IDeleteQueryContext Where(IWhere where)
        {
			WhereClause = where;
			return this;
        }

        /// <inheritdoc/>
        public IDeleteQueryContext Output(Output output)
        {
            this.outputs.Add(output);
            return this;
        }

        /// <summary>
        /// Create a <see cref="IDeleteQueryContext"/> Context.
        /// </summary>
        /// <param name="aliasManager">The <see cref="IAliasManager"/> to use.</param>
        /// <returns>A new instance which implements <see cref="IDeleteQueryContext"/>.</returns>
        public static IDeleteQueryContext Create(IAliasManager aliasManager)
			=> new DeleteQueryContext(aliasManager);

		protected override void Dispose(bool disposing)
        {
			base.Dispose(disposing);
            WhereClause = null;
        }
    }
}

