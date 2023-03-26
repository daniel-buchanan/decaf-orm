using System;
using System.Collections.Generic;
using pdq.common.Templates;
using pdq.db.common.Builders;
using pdq.state;

namespace pdq.npgsql
{
    public class NpgsqlSqlFactory : SqlFactory
    {
        private readonly IBuilderPipeline<ISelectQueryContext> selectBuilder;
        private readonly IBuilderPipeline<IInsertQueryContext> insertBuilder;
        private readonly IBuilderPipeline<IDeleteQueryContext> deleteBuilder;

        public NpgsqlSqlFactory(
            IBuilderPipeline<ISelectQueryContext> selectBuilder,
            IBuilderPipeline<IDeleteQueryContext> deleteBuilder,
            IBuilderPipeline<IInsertQueryContext> insertBuilder)
        {
            this.selectBuilder = selectBuilder;
            this.deleteBuilder = deleteBuilder;
            this.insertBuilder = insertBuilder;
        }

        /// <inheritdoc/>
        protected override IDictionary<string, object> ParseParameters(IDeleteQueryContext context)
            => this.deleteBuilder.GetParameterValues(context);

        /// <inheritdoc/>
        protected override SqlTemplate ParseQuery(IDeleteQueryContext context)
            => this.deleteBuilder.Execute(context);

        /// <inheritdoc/>
        protected override IDictionary<string, object> ParseParameters(IInsertQueryContext context)
            => this.insertBuilder.GetParameterValues(context);

        /// <inheritdoc/>
        protected override SqlTemplate ParseQuery(IInsertQueryContext context)
            => this.insertBuilder.Execute(context);

        /// <inheritdoc/>
        protected override IDictionary<string, object> ParseParameters(ISelectQueryContext context)
            => this.selectBuilder.GetParameterValues(context);

        /// <inheritdoc/>
        protected override SqlTemplate ParseQuery(ISelectQueryContext context)
            => this.selectBuilder.Execute(context);

        /// <inheritdoc/>
        protected override IDictionary<string, object> ParseParameters(IUpdateQueryContext context)
            => throw new NotImplementedException("Update is not yet implemented");

        /// <inheritdoc/>
        protected override SqlTemplate ParseQuery(IUpdateQueryContext context)
            => throw new NotImplementedException("Update is not yet implemented");
    }
}

