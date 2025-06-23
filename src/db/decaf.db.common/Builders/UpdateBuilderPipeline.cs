using decaf.common.Templates;
using decaf.state;

namespace decaf.db.common.Builders;

public abstract class UpdateBuilderPipeline : BuilderPipeline<IUpdateQueryContext>
{
    protected UpdateBuilderPipeline(
        DecafOptions decafOptions,
        IConstants constants,
        IParameterManager parameterManager)
        : base(decafOptions, constants, parameterManager)
    {
        Add(AddUpdate, providesParameters: false);
        Add(AddTable, providesParameters: false);
        Add(AddValues, providesParameters: true);
        Add(AddWhere, providesParameters: true);
        Add(AddOutput, providesParameters: false);
    }

    private static void AddUpdate(IPipelineStageInput<IUpdateQueryContext> input)
        => input.Builder.AppendLine("{0}", Builders.Constants.Update);

    protected abstract void AddTable(IPipelineStageInput<IUpdateQueryContext> input);

    protected abstract void AddValues(IPipelineStageInput<IUpdateQueryContext> input);

    protected abstract void AddWhere(IPipelineStageInput<IUpdateQueryContext> input);

    protected abstract void AddOutput(IPipelineStageInput<IUpdateQueryContext> input);
}