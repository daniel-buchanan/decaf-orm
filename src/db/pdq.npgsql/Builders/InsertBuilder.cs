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

        protected override void AddColumns(IInsertQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.IncreaseIndent();
            var columns = context.Columns.ToArray();

            sqlBuilder.PrependIndent();
            sqlBuilder.Append(Constants.OpeningParen);

            AppendItems(sqlBuilder, columns, (b, i) => this.quotedIdentifierBuilder.AddSelect(i, b));

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
            var outputs = context.Outputs.ToArray();

            AppendItems(sqlBuilder, outputs, (b, o) =>
            {
                sqlBuilder.PrependIndent();
                this.quotedIdentifierBuilder.AddOutput(o, sqlBuilder);
            });

            sqlBuilder.DecreaseIndent();
            sqlBuilder.AppendLine();
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
                AddValuesFromStatic(context, sqlBuilder, parameterManager);
            else if (context.Source is IInsertQueryValuesSource queryValuesSource)
                AddValuesFromQuery(queryValuesSource, sqlBuilder);
            else
                throw new ShouldNeverOccurException($"Insert Values should be one of {nameof(IInsertStaticValuesSource)} or {nameof(IInsertQueryValuesSource)}");
        }

        private void AddValuesFromStatic(IInsertQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            var source = context.Source as IInsertStaticValuesSource;
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

            sqlBuilder.DecreaseIndent();
        }

        private void AddValuesFromQuery(IInsertQueryValuesSource source, ISqlBuilder sqlBuilder)
            => selectBuilder.Build(source.Query, sqlBuilder);
    }
}

