using System;
using System.Linq;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.db.common;
using pdq.db.common.Builders;
using pdq.Exceptions;
using pdq.state;
using pdq.state.ValueSources.Update;

namespace pdq.npgsql.Builders
{
	public class UpdateBuilderPipeline : db.common.Builders.UpdateBuilderPipeline
	{
        private readonly db.common.Builders.IWhereBuilder whereBuilder;
        private readonly QuotedIdentifierBuilder quotedIdentifierBuilder;
        private readonly IBuilderPipeline<ISelectQueryContext> selectBuilder;

        public UpdateBuilderPipeline(
            PdqOptions options,
            NpgsqlOptions dbOptions,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            QuotedIdentifierBuilder quotedIdentifierBuilder,
            IBuilderPipeline<ISelectQueryContext> selectBuilder)
            : base(options, dbOptions, hashProvider)
        {
            this.whereBuilder = whereBuilder;
            this.quotedIdentifierBuilder = quotedIdentifierBuilder;
            this.selectBuilder = selectBuilder;
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
                if (i < lastItemIndex) delimiter = Constants.Seperator;
                processMethod(sqlBuilder, items[i]);
                sqlBuilder.Append(delimiter);

                if(appendNewLine) sqlBuilder.AppendLine();
            }
        }

        protected override void AddOutput(IPipelineStageInput<IUpdateQueryContext> input)
        {
            if (!input.Context.Outputs.Any())
                return;

            input.Builder.AppendLine(Constants.Returning);
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
                input.Builder.AppendLine(Constants.From);
                input.Builder.AppendLine(Constants.OpeningParen);
                input.Builder.IncreaseIndent();
                AddValuesFromQuery(input.Context.Source as ISelectQueryTarget, input);
                input.Builder.DecreaseIndent();
                input.Builder.AppendLine("{0} as x", Constants.ClosingParen);
            }
        }

        private void AddValuesFromQuery(ISelectQueryTarget source, IPipelineStageInput<IUpdateQueryContext> input)
            => selectBuilder.Execute(source.Context, input);

        protected override void AddWhere(IPipelineStageInput<IUpdateQueryContext> input)
            => this.whereBuilder.AddWhere(input.Context.WhereClause, input.Builder, input.Parameters);
    }
}

