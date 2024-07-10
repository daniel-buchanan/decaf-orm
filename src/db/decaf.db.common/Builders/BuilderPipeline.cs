using System;
using System.Collections.Generic;
using System.Linq;
using decaf.common;
using decaf.common.Templates;

namespace decaf.db.common.Builders
{
    public abstract partial class BuilderPipeline<T> : IBuilderPipeline<T>
		where T : IQueryContext
	{
        private readonly DecafOptions options;
        protected readonly IConstants Constants;
        private readonly IParameterManager parameterManager;
        private readonly List<PipelineStage<T>> stages;

		protected BuilderPipeline(
            DecafOptions options,
            IConstants constants,
            IParameterManager parameterManager)
		{
            this.options = options;
            Constants = constants;
            this.parameterManager = parameterManager;
            stages = new List<PipelineStage<T>>();
		}

        protected void Add(Action<IPipelineStageInput<T>> delegateMethod, bool providesParameters, bool? condition = null)
        {
            if (condition != null && condition == false) return;

            stages.Add(PipelineStage<T>.Create(delegateMethod, providesParameters));
        }

        public SqlTemplate Execute(T context, IPipelineStageInput input)
        {
            var newInput = PipelineStageInput<T>.Create(input.Parameters, input.Builder, context);

            if (options.IncludeHeaderCommentsInSql)
            {
                input.Builder.AppendLine("{0} decaf :: query hash: {1}", Constants.Comment, context.GetHash());
                input.Builder.AppendLine("{0} decaf :: generated at: {1}", Constants.Comment, DateTime.Now.ToString());
            }

            foreach (var stage in stages)
            {
                stage.Delegate.Invoke(newInput);
            }

            var sql = input.Builder.GetSql();
            var parameters = input.Parameters.GetParameters();

            return SqlTemplate.Create(sql, parameters);
        }

        public SqlTemplate Execute(T context)
        {
            var input = PipelineStageInput.Create(parameterManager);
            return Execute(context, input);
        }

        public IDictionary<string, object> GetParameterValues(T context, bool includePrefix)
        {
            var input = PipelineStageInput<T>.CreateNoOp(parameterManager, context);
            var filteredStages = stages.Where(s => s.ProvidesParameters);

            foreach (var s in filteredStages)
            {
                s.Delegate.Invoke(input);
            }

            return input.Parameters.GetParameterValues(includePrefix);
        }

        private sealed class PipelineStage<TContext>
            where TContext : IQueryContext
        {
            private PipelineStage(
                Action<IPipelineStageInput<TContext>> delegateMethod,
                bool providesParameters)
            {
                Delegate = delegateMethod;
                ProvidesParameters = providesParameters;
            }

            public Action<IPipelineStageInput<TContext>> Delegate { get; }

            public bool ProvidesParameters { get; }

            public static PipelineStage<TContext> Create(
                Action<IPipelineStageInput<TContext>> delegateMethod,
                bool providesParameters)
                => new PipelineStage<TContext>(delegateMethod, providesParameters);
        }
    }
}

