using decaf.common.Templates;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.sqlite.Builders;

public class DropTableBuilderPipeline : db.common.Builders.DropTableBuilderPipeline
{
    public DropTableBuilderPipeline(
        DecafOptions options, 
        IConstants constants, 
        IParameterManager parameterManager) : 
        base(options, constants, parameterManager) { }

    protected override void AddStartTable(IPipelineStageInput<IDropTableQueryContext> input)
        => input.Builder.Append("drop table {0}", input.Context.Name);

    protected override void AddEndTable(IPipelineStageInput<IDropTableQueryContext> input) 
        => input.Builder.AppendLine();

    protected override void AddCascade(IPipelineStageInput<IDropTableQueryContext> input)
    {
        if (!input.Context.Cascade) return;
        input.Builder.Append(" cascade");
    }
}