using System.Linq;
using decaf.common.Templates;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.db.common.ANSISQL
{
    public abstract class DeleteBuilderPipeline : db.common.Builders.DeleteBuilderPipeline
    {
        protected readonly IQuotedIdentifierBuilder QuotedIdentifierBuilder;

        protected DeleteBuilderPipeline(
            DecafOptions options,
            IConstants constants,
            IParameterManager parameterManager,
            IWhereClauseBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder)
            : base(options, constants, parameterManager, whereBuilder)
        {
            QuotedIdentifierBuilder = quotedIdentifierBuilder;
        }

        protected override void AddOutput(IPipelineStageInput<IDeleteQueryContext> input)
        {
            if (!input.Context.Outputs.Any())
                return;

            input.Builder.AppendLine(Constants.Returning);
            input.Builder.IncreaseIndent();

            var index = 0;
            var noOutputs = input.Context.Outputs.Count - 1;
            foreach (var o in input.Context.Outputs)
            {
                var delimiter = string.Empty;
                if (index < noOutputs)
                    delimiter = Constants.Seperator;

                input.Builder.PrependIndent();
                QuotedIdentifierBuilder.AddOutput(o, input.Builder);

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
            QuotedIdentifierBuilder.AddFromTable(input.Context.Table, input.Builder);
            input.Builder.AppendLine();

            input.Builder.DecreaseIndent();
        }
    }
}

