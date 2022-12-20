using System;
using System.Linq;
using pdq.common;
using pdq.db.common;
using pdq.db.common.Builders;
using pdq.state;
using pdq.state.Conditionals;

namespace pdq.npgsql.Builders
{
    public class WhereBuilder : db.common.Builders.IWhereBuilder
    {
        private readonly IValueParser valueParser;
        private readonly QuotedIdentifierBuilder quotedIdentifierBuilder;

        public WhereBuilder(
            IValueParser valueParser,
            QuotedIdentifierBuilder quotedIdentifierBuilder)
        {
            this.valueParser = valueParser;
            this.quotedIdentifierBuilder = quotedIdentifierBuilder;
        }

        public void AddWhere(IWhere context, ISqlBuilder sqlBuilder) => AddWhere(context, sqlBuilder, 0);

        private void AddWhere(IWhere context, ISqlBuilder sqlBuilder, int level)
        {
            if (context == null) return;

            sqlBuilder.AppendLine("where");

            if (level > 0)
                sqlBuilder.IncreaseIndent();

            sqlBuilder.AppendLine("(");

            if(!context.Children.Any())
            {
                sqlBuilder.IncreaseIndent();
                sqlBuilder.PrependIndent();
                if (context is IColumn) AddColumn(context as IColumn, sqlBuilder, level);
                else if (context is ColumnMatch) AddColumnMatch(context as ColumnMatch, sqlBuilder, level);
                else if (context is IBetween) AddBetween(context as IBetween, sqlBuilder, level);
                else if (context is IInValues) AddInValues(context as IInValues, sqlBuilder, level);
                sqlBuilder.DecreaseIndent();

                sqlBuilder.AppendLine();
                sqlBuilder.Append(")");

                return;
            }

            string combinator = null;
            if (context is And) combinator = "and";
            else if (context is Or) combinator = "or";
            else if (context is Not) sqlBuilder.Append("NOT ");

            var indentLevel = level + 1;
            var index = 0;
            foreach (var w in context.Children)
            {
                AddWhere(w, sqlBuilder, indentLevel);

                if (index > 0) sqlBuilder.AppendLine(combinator);
                index += 1;
            }

            sqlBuilder.AppendLine();
            sqlBuilder.Append(")");

            if (level > 0)
                sqlBuilder.DecreaseIndent();
        }

        private void AddColumn(IColumn context, ISqlBuilder sqlBuilder, int level)
        {
            this.quotedIdentifierBuilder.AddColumn(context.Details, sqlBuilder);

            var op = context.EqualityOperator.ToOperatorString();
            sqlBuilder.Append(" {0} ", op);

            var value = this.valueParser.ToString(context.Value, context.ValueType);

            if (context.EqualityOperator == EqualityOperator.Like)
                sqlBuilder.Append("'%{0}%'", value);
            else if (context.EqualityOperator == EqualityOperator.StartsWith)
                sqlBuilder.Append("'{0}%'", value);
            else if (context.EqualityOperator == EqualityOperator.EndsWith)
                sqlBuilder.Append("'%{0}'", value);
            else
                sqlBuilder.Append(this.valueParser.QuoteValue(context.Value, context.ValueType));
        }

        private void AddColumnMatch(ColumnMatch context, ISqlBuilder sqlBuilder, int level)
        {
            this.quotedIdentifierBuilder.AddColumn(context.Left, sqlBuilder);

            var op = context.EqualityOperator.ToOperatorString();
            sqlBuilder.Append(" {0} ", op);

            this.quotedIdentifierBuilder.AddColumn(context.Right, sqlBuilder);
        }

        private void AddBetween(IBetween context, ISqlBuilder sqlBuilder, int level)
        {
            this.quotedIdentifierBuilder.AddColumn(context.Column, sqlBuilder);

            var start = this.valueParser.QuoteValue(context.Start, context.ValueType);
            var end = this.valueParser.QuoteValue(context.End, context.ValueType);
            sqlBuilder.Append(" between {0} and {1}", start, end);
        }

        private void AddInValues(IInValues context, ISqlBuilder sqlBuilder, int level)
        {
            this.quotedIdentifierBuilder.AddColumn(context.Column, sqlBuilder);
            sqlBuilder.AppendLine(" in (");
            sqlBuilder.IncreaseIndent();

            var values = context.GetValues().ToArray();
            var lastValueIndex = values.Length - 1;
            for (var i = 0; i < values.Length; i++)
            {
                var seperator = i <= lastValueIndex ? "," : string.Empty;
                sqlBuilder.AppendLine("{0}{1}",valueParser.QuoteValue(values[i], context.ValueType), seperator);
            }

            sqlBuilder.DecreaseIndent();
            sqlBuilder.Append(")");
        }
    }
}

