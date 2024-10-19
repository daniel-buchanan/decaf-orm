using decaf.common.Templates;
using decaf.state;

namespace decaf.db.common.Builders;

public abstract class DropTableBuilderPipeline : BuilderPipeline<IDropTableQueryContext>
{
    protected DropTableBuilderPipeline(
        DecafOptions options,
        IConstants constants,
        IParameterManager parameterManager) :
        base(options, constants, parameterManager)
    {
        Add(AddStartTable, providesParameters: false);
        Add(AddCascade, providesParameters: false);
        Add(AddEndTable, providesParameters: false);
    }

    protected abstract void AddStartTable(IPipelineStageInput<IDropTableQueryContext> input);

    protected abstract void AddEndTable(IPipelineStageInput<IDropTableQueryContext> input);
    
    protected abstract void AddCascade(IPipelineStageInput<IDropTableQueryContext> input);
}