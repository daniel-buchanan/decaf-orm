using System;
using System.Linq;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.db.common.Builders;
using pdq.state;
using pdq.state.QueryTargets;

namespace pdq.npgsql.Builders
{
    public class DeleteBuilder : db.common.Builders.DeleteBuilder
    {
        private readonly QuotedIdentifierBuilder quotedIdentifierBuilder;

        public DeleteBuilder(
            db.common.Builders.IWhereBuilder whereBuilder,
            IHashProvider hashProvider,
            QuotedIdentifierBuilder quotedIdentifierBuilder,
            PdqOptions pdqOptions)
            : base(whereBuilder, hashProvider, pdqOptions)
        {
            this.quotedIdentifierBuilder = quotedIdentifierBuilder;
        }

        protected override string CommentCharacter => Constants.Comment;

        protected override void AddOutput(IDeleteQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            if (!context.Outputs.Any())
                return;

            sqlBuilder.AppendLine(Constants.Returning);
            sqlBuilder.IncreaseIndent();

            var index = 0;
            var noOutputs = context.Outputs.Count - 1;
            foreach (var o in context.Outputs)
            {
                var delimiter = string.Empty;
                if (index < noOutputs)
                    delimiter = Constants.Seperator;

                sqlBuilder.PrependIndent();
                this.quotedIdentifierBuilder.AddOutput(o, sqlBuilder);

                if (delimiter.Length > 0)
                    sqlBuilder.Append(delimiter);

                sqlBuilder.AppendLine();

                index += 1;
            }

            sqlBuilder.DecreaseIndent();
        }

        protected override void AddTables(IDeleteQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.IncreaseIndent();

            sqlBuilder.PrependIndent();
            this.quotedIdentifierBuilder.AddFromTable(context.Table, sqlBuilder);
            sqlBuilder.AppendLine();

            sqlBuilder.DecreaseIndent();
        }
    }
}

