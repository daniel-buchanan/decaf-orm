using System;
using System.Collections.Generic;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.state;

namespace pdq.db.common.Builders
{
	public abstract class SelectBuilderPipeline : BuilderPipeline<ISelectQueryContext>
	{
        private readonly IWhereBuilder whereBuilder;

        protected SelectBuilderPipeline(
            PdqOptions options,
            IDatabaseOptions dbOptions,
            IHashProvider hashProvider,
            IWhereBuilder whereBuilder)
            : base(options, dbOptions, hashProvider)
        {
            this.whereBuilder = whereBuilder;

            Add(AddSelect, providesParameters: false);
            Add(AddColumns, providesParameters: false);
            Add(AddTables, providesParameters: false);
            Add(AddJoins, providesParameters: true);
            Add(AddWhere, providesParameters: true);
            Add(AddOrderBy, providesParameters: false);
            Add(AddGroupBy, providesParameters: false);
        }

        private void AddSelect(IPipelineStageInput<ISelectQueryContext> input)
            => input.Builder.AppendLine(Constants.Select);

		protected abstract void AddColumns(IPipelineStageInput<ISelectQueryContext> input);

		protected abstract void AddTables(IPipelineStageInput<ISelectQueryContext> input);

		protected abstract void AddJoins(IPipelineStageInput<ISelectQueryContext> input);

		protected abstract void AddOrderBy(IPipelineStageInput<ISelectQueryContext> input);

		protected abstract void AddGroupBy(IPipelineStageInput<ISelectQueryContext> input);

        private void AddWhere(IPipelineStageInput<ISelectQueryContext> input)
            => this.whereBuilder.AddWhere(input.Context.WhereClause, input.Builder, input.Parameters);
    }
}

