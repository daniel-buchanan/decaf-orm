using decaf.common.Templates;
using decaf.state;

namespace decaf.db.common.Builders;

public abstract class CreateTableBuilderPipeline : BuilderPipeline<ICreateTableQueryContext>
{
    protected CreateTableBuilderPipeline(
        DecafOptions options,
        IConstants constants,
        IParameterManager parameterManager) :
        base(options, constants, parameterManager)
    {
        Add(AddCreateTableInternal, providesParameters: false);
        Add(AddColumns, providesParameters: false);
        Add(AddEndTable, providesParameters: false);
        Add(AddIndicies, providesParameters: false);
        Add(AddPrimaryKey, providesParameters: false);
    }

    private void AddCreateTableInternal(IPipelineStageInput<ICreateTableQueryContext> input)
    {
        AddCreateTable(input);
        input.Builder.IncreaseIndent();
    }

    protected abstract void AddCreateTable(IPipelineStageInput<ICreateTableQueryContext> input);

    protected abstract void AddColumns(IPipelineStageInput<ICreateTableQueryContext> input);
    
    protected abstract void AddIndicies(IPipelineStageInput<ICreateTableQueryContext> input);

    protected abstract void AddEndTable(IPipelineStageInput<ICreateTableQueryContext> input);
    
    protected abstract void AddPrimaryKey(IPipelineStageInput<ICreateTableQueryContext> input);
}