using System;
using System.Linq;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.db.common;
using pdq.db.common.Builders;
using pdq.Exceptions;
using pdq.state;

namespace pdq.npgsql.Builders
{
	public class InsertBuilder : db.common.Builders.InsertBuilder
	{
        private readonly QuotedIdentifierBuilder quotedIdentifierBuilder;
        private readonly IValueParser valueParser;
        private readonly IBuilder<ISelectQueryContext> selectBuilder;

        public InsertBuilder(
            db.common.Builders.IWhereBuilder whereBuilder,
            IHashProvider hashProvider,
            QuotedIdentifierBuilder quotedIdentifierBuilder,
            IValueParser valueParser,
            IBuilder<ISelectQueryContext> selectBuilder,
            PdqOptions pdqOptions)
            : base(whereBuilder, hashProvider, pdqOptions)
        {
            this.quotedIdentifierBuilder = quotedIdentifierBuilder;
            this.valueParser = valueParser;
            this.selectBuilder = selectBuilder;
        }

        protected override string CommentCharacter => Constants.Comment;

        protected override void AddColumns(IInsertQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.IncreaseIndent();
            var columns = context.Columns.ToArray();

            sqlBuilder.PrependIndent();
            sqlBuilder.Append(Constants.OpeningParen);
            var lastColumnIndex = columns.Length - 1;
            for (var i = 0; i < columns.Length; i++)
            {
                var delimiter = string.Empty;
                if (i < lastColumnIndex) delimiter = Constants.Seperator;
                this.quotedIdentifierBuilder.AddSelect(columns[i], sqlBuilder);
                sqlBuilder.Append(delimiter);
            }

            sqlBuilder.Append(Constants.ClosingParen);
            sqlBuilder.DecreaseIndent();
            sqlBuilder.AppendLine();
        }

        protected override void AddOutput(IInsertQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
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

        protected override void AddTable(IInsertQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.IncreaseIndent();

            sqlBuilder.PrependIndent();
            this.quotedIdentifierBuilder.AddFromTable(context.Target.Name, sqlBuilder);
            sqlBuilder.AppendLine();

            sqlBuilder.DecreaseIndent();
        }

        protected override void AddValues(IInsertQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            if (context.Source is IInsertStaticValuesSource)
                AddValuesFromStatic(context, sqlBuilder);
            else if (context.Source is IInsertQueryValuesSource queryValuesSource)
                AddValuesFromQuery(queryValuesSource, sqlBuilder);
            else
                throw new ShouldNeverOccurException($"Insert Values should be one of {nameof(IInsertStaticValuesSource)} or {nameof(IInsertQueryValuesSource)}");
        }

        private void AddValuesFromStatic(IInsertQueryContext context, ISqlBuilder sqlBuilder)
        {
            var source = context.Source as IInsertStaticValuesSource;
            var columns = context.Columns.ToArray();
            var sourceWidth = columns.Count();

            sqlBuilder.AppendLine(Constants.Values);
            var values = source.Values?.ToArray();
            var valuesCount = values.Length;
            for(var i = 0; i < values.Length; i++)
            {
                var valueNumber = i + 1;
                var valuesDelimiter = string.Empty;
                if (valueNumber < values.Length)
                    valuesDelimiter = Constants.Seperator;

                sqlBuilder.PrependIndent();
                sqlBuilder.Append(Constants.OpeningParen);
                
                for(var j = 0; j < values[i].Length; j++)
                {
                    var itemNumber = j + 1;
                    var itemDelimiter = string.Empty;
                    if (itemNumber < values[i].Length)
                        itemDelimiter = Constants.Seperator;

                    var quoteCharacter = valueParser.ValueNeedsQuoting(columns[j]) ? Constants.ValueQuote : string.Empty;
                    sqlBuilder.Append("{0}{1}{0}", quoteCharacter, valueParser.ToString(values[i][j]));

                    if (itemDelimiter.Length > 0)
                        sqlBuilder.Append(itemDelimiter);
                }

                sqlBuilder.Append(Constants.ClosingParen);

                if (valuesDelimiter.Length > 0)
                    sqlBuilder.Append(valuesDelimiter);

                sqlBuilder.AppendLine();
            }
        }

        private void AddValuesFromQuery(IInsertQueryValuesSource source, ISqlBuilder sqlBuilder)
        {
            selectBuilder.Build(source.Query, sqlBuilder);
        }
    }
}

