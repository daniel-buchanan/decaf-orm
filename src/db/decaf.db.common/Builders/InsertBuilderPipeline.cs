using decaf.common.Templates;
using decaf.state;

namespace decaf.db.common.Builders;

public abstract class InsertBuilderPipeline : BuilderPipeline<IInsertQueryContext>
{
    protected InsertBuilderPipeline(
        DecafOptions decafOptions,
        IConstants constants,
        IParameterManager parameterManager)
        : base(decafOptions, constants, parameterManager)
    {
        Add(AddInsert, providesParameters: false);
        Add(AddTable, providesParameters: false);
        Add(AddColumns, providesParameters: false);
        Add(AddValues, providesParameters: true);
        Add(AddOutput, providesParameters: false);
    }

    private static void AddInsert(IPipelineStageInput<IInsertQueryContext> input)
        => input.Builder.AppendLine("{0} into", Builders.Constants.Insert);

    protected abstract void AddTable(IPipelineStageInput<IInsertQueryContext> input);

    protected abstract void AddColumns(IPipelineStageInput<IInsertQueryContext> input);

    protected abstract void AddValues(IPipelineStageInput<IInsertQueryContext> input);

    protected abstract void AddOutput(IPipelineStageInput<IInsertQueryContext> input);
}