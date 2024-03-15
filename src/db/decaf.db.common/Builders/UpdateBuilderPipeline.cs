using decaf.common.Utilities;
using decaf.state;

namespace decaf.db.common.Builders
{
	public abstract class UpdateBuilderPipeline : BuilderPipeline<IUpdateQueryContext>
	{
        protected UpdateBuilderPipeline(
            DecafOptions decafOptions,
            IConstants constants,
            IHashProvider hashProvider)
            : base(decafOptions, constants, hashProvider)
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

