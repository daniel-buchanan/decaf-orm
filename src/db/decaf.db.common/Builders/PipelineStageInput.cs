using decaf.common;
using decaf.common.Templates;

namespace decaf.db.common.Builders;

public class PipelineStageInput : IPipelineStageInput
{
    protected PipelineStageInput(ISqlBuilder builder, IParameterManager parameters)
    {
        Parameters = parameters;
        Builder = builder;
    }

    public IParameterManager Parameters { get; }

    public ISqlBuilder Builder { get; }

    public static IPipelineStageInput Create(IParameterManager parameterManager)
        => new PipelineStageInput(SqlBuilder.Create(), parameterManager);

    public static IPipelineStageInput Create(ISqlBuilder builder, IParameterManager parameters)
        => new PipelineStageInput(builder, parameters);
}

public class PipelineStageInput<TContext> :
    PipelineStageInput,
    IPipelineStageInput<TContext>
    where TContext : IQueryContext
{
    private PipelineStageInput(
        IParameterManager parameters,
        ISqlBuilder builder,
        TContext context)
        : base(builder, parameters) 
    {
        Context = context;
    }

    public TContext Context { get; }

    public static IPipelineStageInput<TContext> CreateNoOp(IParameterManager parameters, TContext context)
        => Create(parameters, SqlBuilder.CreateNoOp(), context);

    public static IPipelineStageInput<TContext> Create(IParameterManager parameters, ISqlBuilder builder, TContext context)
        => new PipelineStageInput<TContext>(parameters, builder, context);
}