using decaf.common;
using decaf.common.Templates;
using decaf.common.Utilities;

namespace decaf.db.common.Builders
{
    public class PipelineStageInput : IPipelineStageInput
    {
        protected PipelineStageInput(ISqlBuilder builder, IParameterManager parameters)
        {
            Parameters = parameters;
            Builder = builder;
        }

        public IParameterManager Parameters { get; }

        public ISqlBuilder Builder { get; }

        public static IPipelineStageInput Create(IHashProvider hashProvider)
            => new PipelineStageInput(SqlBuilder.Create(), ParameterManager.Create(hashProvider));

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

        public static IPipelineStageInput<TContext> CreateNoOp(IHashProvider hashProvider, TContext context)
            => PipelineStageInput<TContext>.Create(hashProvider, SqlBuilder.CreateNoOp(), context);

        public static IPipelineStageInput<TContext> Create(IHashProvider hashProvider, TContext context)
            => PipelineStageInput<TContext>.Create(hashProvider, SqlBuilder.Create(), context);

        public static IPipelineStageInput<TContext> Create(IHashProvider hashProvider, ISqlBuilder builder, TContext context)
            => new PipelineStageInput<TContext>(ParameterManager.Create(hashProvider), builder, context);

        public static IPipelineStageInput<TContext> Create(IParameterManager parameters, ISqlBuilder builder, TContext context)
            => new PipelineStageInput<TContext>(parameters, builder, context);
    }
}

