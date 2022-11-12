using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common;

namespace pdq.state
{
    internal class UpdateQueryContext : QueryContext, IUpdateQueryContext
    {
        private readonly List<IUpdateValueSource> sets;
        private readonly List<Output> outputs;

        private UpdateQueryContext(IAliasManager aliasManager)
            : base(aliasManager, QueryTypes.Update)
        {
            this.sets = new List<IUpdateValueSource>();
            this.outputs = new List<Output>();
        }

        /// <summary>
        /// Create a <see cref="IUpdateQueryContext"/> Context.
        /// </summary>
        /// <param name="aliasManager">The <see cref="IAliasManager"/> to use.</param>
        /// <returns>A new instance which implements <see cref="IUpdateQueryContext"/>.</returns>
		public static IUpdateQueryContext Create(IAliasManager aliasManager)
            => new UpdateQueryContext(aliasManager);

        /// <inheritdoc/>
        public void Update(ITableTarget target)
        {
            var self = this as IQueryContextInternal;
            var existingTarget = self.QueryTargets.FirstOrDefault(q => q.Alias == target.Alias);
            if (existingTarget != null) return;
            self.AddQueryTarget(target);
        }

        /// <inheritdoc/>
        public void From(IQueryTarget source) => Source = source;

        /// <inheritdoc/>
        public void Where(IWhere where) => WhereClause = where;

        /// <inheritdoc/>
        public void Output(Output output) => this.outputs.Add(output);

        /// <inheritdoc/>
        public void Set(IUpdateValueSource value) => this.sets.Add(value);

        /// <inheritdoc/>
        public ITableTarget Table => QueryTargets.FirstOrDefault() as ITableTarget;

        /// <inheritdoc/>
        public IReadOnlyCollection<IUpdateValueSource> Updates => this.sets.AsReadOnly();

        /// <inheritdoc/>
        public IQueryTarget Source { get; private set; }

        /// <inheritdoc/>
        public IWhere WhereClause { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<Output> Outputs => this.outputs.AsReadOnly();
    }
}

