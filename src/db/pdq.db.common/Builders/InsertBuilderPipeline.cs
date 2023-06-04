using System;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.state;

namespace pdq.db.common.Builders
{
	public abstract class InsertBuilderPipeline : BuilderPipeline<IInsertQueryContext>
	{
        protected InsertBuilderPipeline(
            PdqOptions pdqOptions,
            IConstants constants,
            IHashProvider hashProvider)
            : base(pdqOptions, constants, hashProvider)
        {
            Add(AddInsert, providesParameters: false);
            Add(AddTable, providesParameters: false);
            Add(AddColumns, providesParameters: false);
            Add(AddValues, providesParameters: true);
            Add(AddOutput, providesParameters: false);
        }

        private void AddInsert(IPipelineStageInput<IInsertQueryContext> input)
            => input.Builder.AppendLine("{0} into", Constants.Insert);

        protected abstract void AddTable(IPipelineStageInput<IInsertQueryContext> input);

        protected abstract void AddColumns(IPipelineStageInput<IInsertQueryContext> input);

        protected abstract void AddValues(IPipelineStageInput<IInsertQueryContext> input);

        protected abstract void AddOutput(IPipelineStageInput<IInsertQueryContext> input);
    }
}

