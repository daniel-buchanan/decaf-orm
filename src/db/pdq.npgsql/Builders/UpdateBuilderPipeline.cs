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
        private readonly IValueParser valueParser;
        private readonly IBuilderPipeline<ISelectQueryContext> selectBuilder;

        public UpdateBuilderPipeline(
            PdqOptions options,
            NpgsqlOptions dbOptions,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            QuotedIdentifierBuilder quotedIdentifierBuilder,
            IValueParser valueParser,
            IBuilderPipeline<ISelectQueryContext> selectBuilder)
            : base(options, dbOptions, hashProvider)
        {
            this.whereBuilder = whereBuilder;
            this.quotedIdentifierBuilder = quotedIdentifierBuilder;
            this.valueParser = valueParser;
            this.selectBuilder = selectBuilder;
        }

        private void AppendItems<T>(ISqlBuilder sqlBuilder, T[] items, Action<ISqlBuilder, T> processMethod, bool appendNewLine = false)
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

            input.Builder.IncreaseIndent();
            foreach(var u in input.Context.Updates)
            {
                if(u is QueryValueSource qs)
                {
                    input.Builder.PrependIndent();
                    input.Builder.Append("{0} = ", qs.SourceColumn.Name);
                    quotedIdentifierBuilder.AddColumn(qs.DestinationColumn, input.Builder);
                    input.Builder.AppendLine();
                }
                else if(u is StaticValueSource ss)
                {
                    var p = input.Parameters.Add(ss.Column, ss.Value);
                    input.Builder.AppendLine("{0} = {1}", ss.Column.Name, p.Name);
                }
            }
            input.Builder.DecreaseIndent();
        }

        private void AddValuesFromStatic(IUpdateQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            throw new NotImplementedException();
            /*var source = context.Source as IInsertStaticValuesSource;
            var columns = context.Columns.ToArray();

            sqlBuilder.AppendLine(Constants.Values);
            var values = source.Values?.ToArray();

            sqlBuilder.IncreaseIndent();

            for(var i = 0; i < values.Length; i++)
            {
                sqlBuilder.PrependIndent();
                sqlBuilder.Append(Constants.OpeningParen);
                
                for(var j = 0; j < values[i].Length; j++)
                {
                    var itemNumber = j + 1;
                    var quoteCharacter = valueParser.ValueNeedsQuoting(columns[j]) ?
                        Constants.ValueQuote :
                        string.Empty;

                    var parameter = parameterManager.Add(columns[j], values[i][j]);
                    sqlBuilder.Append("{0}{1}{0}", quoteCharacter, parameter.Name);

                    if (itemNumber < values[i].Length)
                        sqlBuilder.Append(Constants.Seperator);
                }

                sqlBuilder.Append(Constants.ClosingParen);

                var valueNumber = i + 1;
                if (valueNumber < values.Length)
                    sqlBuilder.Append(Constants.Seperator);

                sqlBuilder.AppendLine();
            }

            sqlBuilder.DecreaseIndent();*/
        }

        private void AddValuesFromQuery(ISelectQueryTarget source, IPipelineStageInput<IUpdateQueryContext> input)
            => selectBuilder.Execute(source.Context, input);

        protected override void AddWhere(IPipelineStageInput<IUpdateQueryContext> input)
            => this.whereBuilder.AddWhere(input.Context.WhereClause, input.Builder, input.Parameters);
    }
}

