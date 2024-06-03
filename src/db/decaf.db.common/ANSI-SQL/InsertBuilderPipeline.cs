using System;
using System.Linq;
using decaf.common.Exceptions;
using decaf.common.Templates;
using decaf.common.Utilities;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.db.common.ANSISQL
{
    public abstract class InsertBuilderPipeline : db.common.Builders.InsertBuilderPipeline
    {
        protected readonly IQuotedIdentifierBuilder QuotedIdentifierBuilder;
        protected readonly IValueParser ValueParser;
        protected readonly IBuilderPipeline<ISelectQueryContext> SelectBuilder;

        protected InsertBuilderPipeline(
            DecafOptions options,
            IConstants constants,
            IParameterManager parameterManager,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IValueParser valueParser,
            IBuilderPipeline<ISelectQueryContext> selectBuilder)
            : base(options, constants, parameterManager)
        {
            QuotedIdentifierBuilder = quotedIdentifierBuilder;
            ValueParser = valueParser;
            SelectBuilder = selectBuilder;
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

                if (appendNewLine) sqlBuilder.AppendLine();
            }
        }

        protected override void AddColumns(IPipelineStageInput<IInsertQueryContext> input)
        {
            input.Builder.IncreaseIndent();
            var columns = input.Context.Columns.ToArray();

            input.Builder.PrependIndent();
            input.Builder.Append(Constants.OpeningParen);

            AppendItems(input.Builder, columns, (b, i) => this.QuotedIdentifierBuilder.AddSelect(i, b));

            input.Builder.Append(Constants.ClosingParen);
            input.Builder.DecreaseIndent();
            input.Builder.AppendLine();
        }

        protected override void AddOutput(IPipelineStageInput<IInsertQueryContext> input)
        {
            if (!input.Context.Outputs.Any())
                return;

            input.Builder.AppendLine(Constants.Returning);
            input.Builder.IncreaseIndent();
            var outputs = input.Context.Outputs.ToArray();

            AppendItems(input.Builder, outputs, (_, o) =>
            {
                input.Builder.PrependIndent();
                QuotedIdentifierBuilder.AddOutput(o, input.Builder);
            });

            input.Builder.DecreaseIndent();
            input.Builder.AppendLine();
        }

        protected override void AddTable(IPipelineStageInput<IInsertQueryContext> input)
        {
            input.Builder.IncreaseIndent();

            input.Builder.PrependIndent();
            this.QuotedIdentifierBuilder.AddFromTable(input.Context.Target.Name, input.Builder);
            input.Builder.AppendLine();

            input.Builder.DecreaseIndent();
        }

        protected override void AddValues(IPipelineStageInput<IInsertQueryContext> input)
        {
            if (input.Context.Source is IInsertStaticValuesSource)
                AddValuesFromStatic(input.Context, input.Builder, input.Parameters);
            else if (input.Context.Source is IInsertQueryValuesSource queryValuesSource)
                AddValuesFromQuery(queryValuesSource, input);
            else
                throw new ShouldNeverOccurException($"Insert Values should be one of {nameof(IInsertStaticValuesSource)} or {nameof(IInsertQueryValuesSource)}");
        }

        private void AddValuesFromStatic(IInsertQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            var source = context.Source as IInsertStaticValuesSource;
            var columns = context.Columns.ToArray();

            sqlBuilder.AppendLine(Constants.Values);
            var values = source?.Values?.ToArray();

            if (values == null) return;

            sqlBuilder.IncreaseIndent();

            for (var i = 0; i < values.Length; i++)
                BuildValueClause(values, i, columns, sqlBuilder, parameterManager);

            sqlBuilder.DecreaseIndent();
        }

        private void BuildValueClause(object[][] rows, int rowIndex, Column[] columns, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.PrependIndent();
            sqlBuilder.Append(Constants.OpeningParen);

            var row = rows[rowIndex];
            
            for (var i = 0; i < row.Length; i++)
            {
                var seperatorValue = i < (row.Length - 1) ?
                    Constants.Seperator :
                    string.Empty;

                row.TryGetValue(i, out var v);
                if (!columns.TryGetValue(i, out var c))
                {
                    sqlBuilder.Append("{0}{0}{1}", Constants.ValueQuote, seperatorValue);
                    continue;
                }

                var parameter = parameterManager.Add(c, v);
                sqlBuilder.Append("{0}{1}", parameter.Name, seperatorValue);
            }

            sqlBuilder.Append(Constants.ClosingParen);
            
            if(rowIndex < (rows.Length - 1))
                sqlBuilder.Append(Constants.Seperator);
            
            sqlBuilder.AppendLine();
        }

        private void AddValuesFromQuery(IInsertQueryValuesSource source, IPipelineStageInput<IInsertQueryContext> input)
            => SelectBuilder.Execute(source.Query, input);
    }
}

