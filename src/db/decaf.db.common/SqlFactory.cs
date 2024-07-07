using System.Collections.Generic;
using decaf.common.Templates;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.db.common
{
	public class SqlFactory : state.SqlFactory
	{
        private readonly IBuilderPipeline<ISelectQueryContext> selectBuilder;
        private readonly IBuilderPipeline<IInsertQueryContext> insertBuilder;
        private readonly IBuilderPipeline<IDeleteQueryContext> deleteBuilder;
        private readonly IBuilderPipeline<IUpdateQueryContext> updateBuilder;
        private readonly IBuilderPipeline<ICreateTableQueryContext> createTableBuilder;

        public SqlFactory(
            IBuilderPipeline<ISelectQueryContext> selectBuilder,
            IBuilderPipeline<IDeleteQueryContext> deleteBuilder,
            IBuilderPipeline<IInsertQueryContext> insertBuilder,
            IBuilderPipeline<IUpdateQueryContext> updateBuilder,
            IBuilderPipeline<ICreateTableQueryContext> createTableBuilder = null)
        {
            this.selectBuilder = selectBuilder;
            this.deleteBuilder = deleteBuilder;
            this.insertBuilder = insertBuilder;
            this.updateBuilder = updateBuilder;
            this.createTableBuilder = createTableBuilder;
        }

        protected virtual bool IncludeParameterPrefix => false;

        /// <inheritdoc/>
        protected override IDictionary<string, object> ParseParameters(IInsertQueryContext context, bool includePrefix)
            => this.insertBuilder.GetParameterValues(context, includePrefix);

        /// <inheritdoc/>
        protected override IDictionary<string, object> ParseParameters(IDeleteQueryContext context, bool includePrefix)
            => this.deleteBuilder.GetParameterValues(context, includePrefix);

        /// <inheritdoc/>
        protected override IDictionary<string, object> ParseParameters(IUpdateQueryContext context, bool includePrefix)
            => this.updateBuilder.GetParameterValues(context, includePrefix);

        /// <inheritdoc/>
        protected override IDictionary<string, object> ParseParameters(ISelectQueryContext context, bool includePrefix)
            => this.selectBuilder.GetParameterValues(context, includePrefix);

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

        protected override SqlTemplate ParseQuery(ICreateTableQueryContext context)
            => this.createTableBuilder.Execute(context);
    }
}

