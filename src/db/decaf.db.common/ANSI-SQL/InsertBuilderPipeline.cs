using System;
using System.Linq;
using decaf.common.Exceptions;
using decaf.common.Templates;
using decaf.common.Utilities;
using decaf.db.common.Builders;
using decaf.state;
using decaf.db.common;
using decaf.Exceptions;

namespace decaf.db.common.ANSISQL
{
    public abstract class InsertBuilderPipeline : db.common.Builders.InsertBuilderPipeline
    {
        protected readonly IQuotedIdentifierBuilder quotedIdentifierBuilder;
        protected readonly IValueParser valueParser;
        protected readonly IBuilderPipeline<ISelectQueryContext> selectBuilder;
        protected readonly IConstants constants;

        protected InsertBuilderPipeline(
            DecafOptions options,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IValueParser valueParser,
            IBuilderPipeline<ISelectQueryContext> selectBuilder,
            IConstants constants)
            : base(options, constants, hashProvider)
        {
            this.quotedIdentifierBuilder = quotedIdentifierBuilder;
            this.valueParser = valueParser;
            this.selectBuilder = selectBuilder;
            this.constants = constants;
        }

        private void AppendItems<T>(ISqlBuilder sqlBuilder, T[] items, Action<ISqlBuilder, T> processMethod, bool appendNewLine = false)
        {
            var lastItemIndex = items.Length - 1;
            for (var i = 0; i < items.Length; i++)
            {
                var delimiter = string.Empty;
                if (i < lastItemIndex) delimiter = constants.Seperator;
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
            input.Builder.Append(constants.OpeningParen);

            AppendItems(input.Builder, columns, (b, i) => this.quotedIdentifierBuilder.AddSelect(i, b));

            input.Builder.Append(constants.ClosingParen);
            input.Builder.DecreaseIndent();
            input.Builder.AppendLine();
        }

        protected override void AddOutput(IPipelineStageInput<IInsertQueryContext> input)
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

        protected override void AddTable(IPipelineStageInput<IInsertQueryContext> input)
        {
            input.Builder.IncreaseIndent();

            input.Builder.PrependIndent();
            this.quotedIdentifierBuilder.AddFromTable(input.Context.Target.Name, input.Builder);
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

            sqlBuilder.AppendLine(constants.Values);
            var values = source.Values?.ToArray();

            if (values == null) return;

            sqlBuilder.IncreaseIndent();

            for (var i = 0; i < values.Length; i++)
                BuildValueClause(values, i, columns, sqlBuilder, parameterManager);

            sqlBuilder.DecreaseIndent();
        }

        private void BuildValueClause(object[][] rows, int rowIndex, Column[] columns, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.PrependIndent();
            sqlBuilder.Append(constants.OpeningParen);

            var row = rows[rowIndex];
            
            for (var i = 0; i < row.Length; i++)
            {
                var seperatorValue = i < (row.Length - 1) ?
                    constants.Seperator :
                    string.Empty;

                row.TryGetValue(i, out var v);
                if (!columns.TryGetValue(i, out var c))
                {
                    sqlBuilder.Append("{0}{0}{1}", constants.ValueQuote, seperatorValue);
                    continue;
                }

                var parameter = parameterManager.Add(c, v);
                sqlBuilder.Append("{0}{1}", parameter.Name, seperatorValue);
            }

            sqlBuilder.Append(constants.ClosingParen);
            
            if(rowIndex < (rows.Length - 1))
                sqlBuilder.Append(constants.Seperator);
            
            sqlBuilder.AppendLine();
        }

        private void AddValuesFromQuery(IInsertQueryValuesSource source, IPipelineStageInput<IInsertQueryContext> input)
            => selectBuilder.Execute(source.Query, input);
    }
}

