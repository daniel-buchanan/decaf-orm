using pdq.common.Utilities;
using pdq.state;

namespace pdq.db.common.Builders
{
	public abstract class UpdateBuilderPipeline : BuilderPipeline<IUpdateQueryContext>
	{
        protected UpdateBuilderPipeline(
            PdqOptions pdqOptions,
            IDatabaseOptions dbOptions,
            IHashProvider hashProvider)
            : base(pdqOptions, dbOptions, hashProvider)
        {
            Add(AddUpdate, providesParameters: false);
            Add(AddTable, providesParameters: false);
            Add(AddValues, providesParameters: true);
            Add(AddWhere, providesParameters: true);
            Add(AddOutput, providesParameters: false);
        }

        private void AddUpdate(IPipelineStageInput<IUpdateQueryContext> input)
            => input.Builder.AppendLine("{0}", Constants.Update);

        protected abstract void AddTable(IPipelineStageInput<IUpdateQueryContext> input);

        protected abstract void AddValues(IPipelineStageInput<IUpdateQueryContext> input);

        protected abstract void AddWhere(IPipelineStageInput<IUpdateQueryContext> input);

        protected abstract void AddOutput(IPipelineStageInput<IUpdateQueryContext> input);
    }
}

