using System;
using System.Collections.Generic;
using System.Linq;
using decaf.common;
using decaf.common.Templates;
using decaf.common.Utilities;

namespace decaf.db.common.Builders
{
    public abstract partial class BuilderPipeline<T> : IBuilderPipeline<T>
		where T : IQueryContext
	{
        private readonly DecafOptions options;
        private readonly IConstants constants;
        private readonly IHashProvider hashProvider;
        private readonly List<PipelineStage<T>> stages;

		protected BuilderPipeline(
            DecafOptions options,
            IConstants constants,
            IHashProvider hashProvider)
		{
            this.options = options;
            this.constants = constants;
            this.hashProvider = hashProvider;
            this.stages = new List<PipelineStage<T>>();
		}

        protected void Add(Action<IPipelineStageInput<T>> delegateMethod, bool providesParameters, bool? condition = null)
        {
            if (condition != null && condition == false) return;

            this.stages.Add(PipelineStage<T>.Create(delegateMethod, providesParameters));
        }

        public SqlTemplate Execute(T context, IPipelineStageInput input)
        {
            var newInput = PipelineStageInput<T>.Create(input.Parameters, input.Builder, context);

            if (this.options.IncludeHeaderCommentsInSql)
            {
                input.Builder.AppendLine("{0} decaf :: query hash: {1}", this.constants.Comment, context.GetHash());
                input.Builder.AppendLine("{0} decaf :: generated at: {1}", this.constants.Comment, DateTime.Now.ToString());
            }

            foreach (var stage in this.stages)
            {
                stage.Delegate.Invoke(newInput);
            }

            var sql = input.Builder.GetSql();
            var parameters = input.Parameters.GetParameters();

            return SqlTemplate.Create(sql, parameters);
        }

        public SqlTemplate Execute(T context)
        {
            var input = PipelineStageInput.Create(this.hashProvider);
            return Execute(context, input);
        }

        public IDictionary<string, object> GetParameterValues(T context)
        {
            var input = PipelineStageInput<T>.CreateNoOp(this.hashProvider, context);
            var filteredStages = this.stages.Where(s => s.ProvidesParameters);

            foreach (var s in filteredStages)
            {
                s.Delegate.Invoke(input);
            }

            return input.Parameters.GetParameterValues();
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

