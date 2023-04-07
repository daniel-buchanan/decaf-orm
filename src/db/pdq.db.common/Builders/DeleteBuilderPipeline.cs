using System;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.state;

namespace pdq.db.common.Builders
{
	public abstract class DeleteBuilderPipeline : BuilderPipeline<IDeleteQueryContext>
	{
        private readonly IWhereBuilder whereBuilder;

        protected DeleteBuilderPipeline(
            PdqOptions options,
            IDatabaseOptions dbOptions,
            IHashProvider hashProvider,
            IWhereBuilder whereBuilder)
            : base(options, dbOptions, hashProvider)
        {
            this.whereBuilder = whereBuilder;

            Add(AddDelete, providesParameters: false);
            Add(AddTables, providesParameters: false);
            Add(AddWhere, providesParameters: true);
            Add(AddOutput, providesParameters: false);
        }

        private void AddDelete(IPipelineStageInput<IDeleteQueryContext> input)
            => input.Builder.AppendLine("{0} from", Constants.Delete);

        protected abstract void AddTables(IPipelineStageInput<IDeleteQueryContext> input);

        protected abstract void AddOutput(IPipelineStageInput<IDeleteQueryContext> input);

        private void AddWhere(IPipelineStageInput<IDeleteQueryContext> input)
            => this.whereBuilder.AddWhere(input.Context.WhereClause, input.Builder, input.Parameters);
    }
}

