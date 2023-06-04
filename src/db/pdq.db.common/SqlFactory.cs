using System;
using System.Collections.Generic;
using pdq.common.Templates;
using pdq.db.common.Builders;
using pdq.state;

namespace pdq.db.common
{
	public class SqlFactory : pdq.state.SqlFactory
	{
        private readonly IBuilderPipeline<ISelectQueryContext> selectBuilder;
        private readonly IBuilderPipeline<IInsertQueryContext> insertBuilder;
        private readonly IBuilderPipeline<IDeleteQueryContext> deleteBuilder;
        private readonly IBuilderPipeline<IUpdateQueryContext> updateBuilder;

        public SqlFactory(
            IBuilderPipeline<ISelectQueryContext> selectBuilder,
            IBuilderPipeline<IDeleteQueryContext> deleteBuilder,
            IBuilderPipeline<IInsertQueryContext> insertBuilder,
            IBuilderPipeline<IUpdateQueryContext> updateBuilder)
        {
            this.selectBuilder = selectBuilder;
            this.deleteBuilder = deleteBuilder;
            this.insertBuilder = insertBuilder;
            this.updateBuilder = updateBuilder;
        }

        /// <inheritdoc/>
        protected override IDictionary<string, object> ParseParameters(IInsertQueryContext context)
            => this.insertBuilder.GetParameterValues(context);

        /// <inheritdoc/>
        protected override IDictionary<string, object> ParseParameters(IDeleteQueryContext context)
            => this.deleteBuilder.GetParameterValues(context);

        /// <inheritdoc/>
        protected override IDictionary<string, object> ParseParameters(IUpdateQueryContext context)
            => this.updateBuilder.GetParameterValues(context);

        /// <inheritdoc/>
        protected override IDictionary<string, object> ParseParameters(ISelectQueryContext context)
            => this.selectBuilder.GetParameterValues(context);

        /// <inheritdoc/>
        protected override SqlTemplate ParseQuery(IInsertQueryContext context)
            => this.insertBuilder.Execute(context);

        /// <inheritdoc/>
        protected override SqlTemplate ParseQuery(IDeleteQueryContext context)
            => this.deleteBuilder.Execute(context);

        /// <inheritdoc/>
        protected override SqlTemplate ParseQuery(IUpdateQueryContext context)
            => this.updateBuilder.Execute(context);

        /// <inheritdoc/>
        protected override SqlTemplate ParseQuery(ISelectQueryContext context)
            => this.selectBuilder.Execute(context);
    }
}

