using System;
using System.Linq;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.db.common;
using pdq.db.common.Builders;
using pdq.state;
using pdq.state.QueryTargets;

namespace pdq.db.common.ANSISQL
{
    public abstract class DeleteBuilderPipeline : db.common.Builders.DeleteBuilderPipeline
    {
        private readonly IQuotedIdentifierBuilder quotedIdentifierBuilder;
        private readonly IConstants constants;

        public DeleteBuilderPipeline(
            PdqOptions options,
            IDatabaseOptions dbOptions,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IConstants constants)
            : base(options, dbOptions, hashProvider, whereBuilder)
        {
            this.quotedIdentifierBuilder = quotedIdentifierBuilder;
            this.constants = constants;
        }

        protected override void AddOutput(IPipelineStageInput<IDeleteQueryContext> input)
        {
            if (!input.Context.Outputs.Any())
                return;

            input.Builder.AppendLine(constants.Returning);
            input.Builder.IncreaseIndent();

            var index = 0;
            var noOutputs = input.Context.Outputs.Count - 1;
            foreach (var o in input.Context.Outputs)
            {
                var delimiter = string.Empty;
                if (index < noOutputs)
                    delimiter = constants.Seperator;

                input.Builder.PrependIndent();
                this.quotedIdentifierBuilder.AddOutput(o, input.Builder);

                if (delimiter.Length > 0)
                    input.Builder.Append(delimiter);

                input.Builder.AppendLine();

                index += 1;
            }

            input.Builder.DecreaseIndent();
        }

        protected override void AddTables(IPipelineStageInput<IDeleteQueryContext> input)
        {
            input.Builder.IncreaseIndent();

            input.Builder.PrependIndent();
            this.quotedIdentifierBuilder.AddFromTable(input.Context.Table, input.Builder);
            input.Builder.AppendLine();

            input.Builder.DecreaseIndent();
        }
    }
}

