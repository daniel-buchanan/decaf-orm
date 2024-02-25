using System;
using System.Linq;
using decaf.common.Utilities;
using decaf.db.common.Builders;
using decaf.state;
using decaf.state.ValueSources.Update;
using decaf.common.Templates;
using decaf.db.common;
using decaf.Exceptions;

namespace decaf.db.common.ANSISQL
{
	public class UpdateBuilderPipeline : db.common.Builders.UpdateBuilderPipeline
	{
        protected readonly db.common.Builders.IWhereBuilder whereBuilder;
        protected readonly IQuotedIdentifierBuilder quotedIdentifierBuilder;
        protected readonly IBuilderPipeline<ISelectQueryContext> selectBuilder;
        protected readonly IConstants constants;

        protected UpdateBuilderPipeline(
            DecafOptions options,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IBuilderPipeline<ISelectQueryContext> selectBuilder,
            IConstants constants)
            : base(options, constants, hashProvider)
        {
            this.whereBuilder = whereBuilder;
            this.quotedIdentifierBuilder = quotedIdentifierBuilder;
            this.selectBuilder = selectBuilder;
            this.constants = constants;
        }

        private void AppendItems<T>(
            ISqlBuilder sqlBuilder,
            T[] items,
            Action<ISqlBuilder, T> processMethod,
            bool appendNewLine = false)
        {
            var lastItemIndex = items.Length - 1;
            for (var i = 0; i < items.Length; i++)
            {
                var delimiter = string.Empty;
                if (i < lastItemIndex) delimiter = constants.Seperator;
                processMethod(sqlBuilder, items[i]);
                sqlBuilder.Append(delimiter);

                if(appendNewLine) sqlBuilder.AppendLine();
            }
        }

        protected override void AddOutput(IPipelineStageInput<IUpdateQueryContext> input)
        {
            if (!input.Context.Outputs.Any())
                return;

            input.Builder.AppendLine(constants.Returning);
            input.Builder.IncreaseIndent();
            var outputs = input.Context.Outputs.ToArray();

            AppendItems(input.Builder, outputs, (b, o) =>
            {
                input.Builder.PrependIndent();
                this.quotedIdentifierBuilder.AddOutput(o, input.Builder);
            });

            input.Builder.DecreaseIndent();
            input.Builder.AppendLine();
        }

        protected override void AddTable(IPipelineStageInput<IUpdateQueryContext> input)
        {
            input.Builder.IncreaseIndent();

            input.Builder.PrependIndent();
            this.quotedIdentifierBuilder.AddFromTable(input.Context.Table.Name, input.Builder);
            input.Builder.AppendLine();

            input.Builder.DecreaseIndent();
        }

        protected override void AddValues(IPipelineStageInput<IUpdateQueryContext> input)
        {
            input.Builder.AppendLine("set");

            var first = input.Context.Updates?.FirstOrDefault();
            if (first == null || input.Context.Updates?.Any() == false)
                return;

            input.Builder.IncreaseIndent();
            if(first is StaticValueSource)
            {
                var items = input.Context.Updates.Select(i => i as StaticValueSource).ToArray();
                AppendItems(input.Builder, items, (b, i) => {
                    var p = input.Parameters.Add(i.Column, i.Value);
                    b.PrependIndent();
                    b.Append("{0} = {1}", i.Column.Name, p.Name);
                }, true);
            }
            else if (first is QueryValueSource)
            {
                var items = input.Context.Updates.Select(i => i as QueryValueSource).ToArray();
                AppendItems(input.Builder, items, (b, i) => {
                    b.PrependIndent();
                    quotedIdentifierBuilder.AddColumn(i.DestinationColumn, b);
                    b.Append(" = x.");
                    quotedIdentifierBuilder.AddColumn(i.SourceColumn, b);
                }, true);
            }
            input.Builder.DecreaseIndent();

            if (first is QueryValueSource)
            {
                input.Builder.AppendLine(constants.From);
                input.Builder.AppendLine(constants.OpeningParen);
                input.Builder.IncreaseIndent();
                AddValuesFromQuery(input.Context.Source as ISelectQueryTarget, input);
                input.Builder.DecreaseIndent();
                this.quotedIdentifierBuilder.AddClosingFromQuery("x", input.Builder);
            }
        }

        private void AddValuesFromQuery(ISelectQueryTarget source, IPipelineStageInput<IUpdateQueryContext> input)
            => selectBuilder.Execute(source.Context, input);

        protected override void AddWhere(IPipelineStageInput<IUpdateQueryContext> input)
            => this.whereBuilder.AddWhere(input.Context.WhereClause, input.Builder, input.Parameters);
    }
}

