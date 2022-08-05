using System;
using System.Collections.Generic;
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
        public ITableTarget Target { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<Column> Columns => this.columns.AsReadOnly();

        /// <inheritdoc/>
        public IInsertValuesSource Source { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<Output> Outputs => this.outputs.AsReadOnly();

        /// <inheritdoc/>
        public IInsertQueryContext Column(Column column)
        {
            this.columns.Add(column);
            return this;
        }

        /// <inheritdoc/>
        public IInsertQueryContext From(IInsertValuesSource source)
        {
            Source = source;
            return this;
        }

        /// <inheritdoc/>
        public IInsertQueryContext Into(ITableTarget target)
        {
            Target = target;
            return this;
        }

        /// <inheritdoc/>
        public IInsertQueryContext Output(Output output)
        {
            this.outputs.Add(output);
            return this;
        }

        /// <inheritdoc/>
        public IInsertQueryContext Value(object[] value)
        {
            var staticValueSource = Source as IInsertStaticValuesSource;
            if (staticValueSource == null) return this;

            staticValueSource.AddValue(value);
            return this;
        }
    }
}

