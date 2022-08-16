﻿using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common;

namespace pdq.state
{
    internal class InsertQueryContext : QueryContext, IInsertQueryContext
    {
        private readonly List<Output> outputs;
        private readonly List<Column> columns;

        private InsertQueryContext(IAliasManager aliasManager, QueryTypes kind)
            : base(aliasManager, kind)
        {
            outputs = new List<Output>();
            columns = new List<Column>();
        }

        public static IInsertQueryContext Create(IAliasManager aliasManager)
            => new InsertQueryContext(aliasManager, QueryTypes.Insert);

        /// <inheritdoc/>
        public ITableTarget Target
            => this.QueryTargets.FirstOrDefault() as ITableTarget;

        /// <inheritdoc/>
        public IReadOnlyCollection<Column> Columns => this.columns.AsReadOnly();

        /// <inheritdoc/>
        public IInsertValuesSource Source { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<Output> Outputs => this.outputs.AsReadOnly();

        /// <inheritdoc/>
        public void Column(Column column)
        {
            var item = this.columns.FirstOrDefault(c => c.IsEquivalentTo(column));
            if (item != null) return;

            this.columns.Add(column);
        }

        /// <inheritdoc/>
        public void From(IInsertValuesSource source) => Source = source;

        /// <inheritdoc/>
        public void Into(ITableTarget target)
        {
            var item = this.QueryTargets.FirstOrDefault(t => t.IsEquivalentTo(target));
            if (item != null) return;

            var internalContext = this as IQueryContextInternal;
            internalContext.AddQueryTarget(target);
        }

        /// <inheritdoc/>
        public void Output(Output output) => this.outputs.Add(output);

        /// <inheritdoc/>
        public void Value(object[] value)
        {
            var staticValueSource = Source as IInsertStaticValuesSource;

            if(Source != null && staticValueSource == null) return;

            if (staticValueSource == null)
            {
                staticValueSource = state.ValueSources.Insert.StaticValuesSource.Create(value.Length);
                Source = staticValueSource;
            }

            staticValueSource.AddValue(value);
        }
    }
}

