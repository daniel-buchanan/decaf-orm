using System;
using System.Linq;
using System.Reflection.Emit;
using pdq.common;
using pdq.common.Templates;
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

        public void AddWhere(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            if (clause == null) return;
            sqlBuilder.AppendLine("where");

            AddWhere(clause, sqlBuilder, parameterManager, 0);
        }

        private void AddWhere(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager, int level)
        {
            if (clause == null) return;

            if (level > 0)
                sqlBuilder.IncreaseIndent();

            if (clause.Children.Any()) AddAndOr(clause, sqlBuilder, parameterManager, level);
            else AddClause(clause, sqlBuilder, parameterManager);

            if (level > 0)
                sqlBuilder.DecreaseIndent();

            sqlBuilder.AppendLine();
        }

        private void AddClause(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.AppendLine("(");
            sqlBuilder.IncreaseIndent();
            sqlBuilder.PrependIndent();
            if (clause is IColumn) AddColumn(clause as IColumn, sqlBuilder, parameterManager);
            else if (clause is ColumnMatch) AddColumnMatch(clause as ColumnMatch, sqlBuilder);
            else if (clause is IBetween) AddBetween(clause as IBetween, sqlBuilder, parameterManager);
            else if (clause is IInValues) AddInValues(clause as IInValues, sqlBuilder, parameterManager);
            sqlBuilder.DecreaseIndent();

            sqlBuilder.AppendLine();
            sqlBuilder.Append(")");
        }

        private void AddAndOr(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager, int level)
        {
            sqlBuilder.AppendLine("(");
            string combinator = null;
            if (clause is And) combinator = "and";
            else if (clause is Or) combinator = "or";
            else if (clause is Not) sqlBuilder.Append("NOT ");

            var indentLevel = level + 1;
            var index = 0;
            foreach (var w in clause.Children)
            {
                AddWhere(w, sqlBuilder, parameterManager, indentLevel);

                if (index > 0) sqlBuilder.AppendLine(combinator);
                index += 1;
            }

            sqlBuilder.AppendLine();
            sqlBuilder.Append(")");
        }

        private void AddColumn(IColumn column, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            var parameter = parameterManager.Add(column, column.Value);

            this.quotedIdentifierBuilder.AddColumn(column.Details, sqlBuilder);

            var op = column.EqualityOperator.ToOperatorString();
            sqlBuilder.Append(" {0} ", op);

            if (column.EqualityOperator == EqualityOperator.Like)
                sqlBuilder.Append("'%{0}%'", parameter.Name);
            else if (column.EqualityOperator == EqualityOperator.StartsWith)
                sqlBuilder.Append("'{0}%'", parameter.Name);
            else if (column.EqualityOperator == EqualityOperator.EndsWith)
                sqlBuilder.Append("'%{0}'", parameter.Name);
            else
                sqlBuilder.Append(this.valueParser.QuoteValue(parameter.Name, column.ValueType));
        }

        private void AddColumnMatch(ColumnMatch columnMatch, ISqlBuilder sqlBuilder)
        {
            this.quotedIdentifierBuilder.AddColumn(columnMatch.Left, sqlBuilder);

            var op = columnMatch.EqualityOperator.ToOperatorString();
            sqlBuilder.Append(" {0} ", op);

            this.quotedIdentifierBuilder.AddColumn(columnMatch.Right, sqlBuilder);
        }

        private void AddBetween(IBetween between, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            var betweenStartParameter = parameterManager.Add(ParameterWrapper.Create(between, "start"), between.Start);
            var betweenEndParameter = parameterManager.Add(ParameterWrapper.Create(between, "end"), between.End);

            this.quotedIdentifierBuilder.AddColumn(between.Column, sqlBuilder);

            var start = this.valueParser.QuoteValue(betweenStartParameter.Name, between.ValueType);
            var end = this.valueParser.QuoteValue(betweenEndParameter.Name, between.ValueType);
            sqlBuilder.Append(" between {0} and {1}", start, end);
        }

        private void AddInValues(IInValues inValues, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            this.quotedIdentifierBuilder.AddColumn(inValues.Column, sqlBuilder);
            sqlBuilder.AppendLine(" in (");
            sqlBuilder.IncreaseIndent();

            var values = inValues.GetValues().ToArray();
            var lastValueIndex = values.Length - 1;
            for (var i = 0; i < values.Length; i++)
            {
                var parameter = parameterManager.Add(ParameterWrapper.Create(inValues, i), values[i]);
                var seperator = i <= lastValueIndex ? "," : string.Empty;
                sqlBuilder.AppendLine("{0}{1}",valueParser.QuoteValue(parameter.Name, inValues.ValueType), seperator);
            }

            sqlBuilder.DecreaseIndent();
            sqlBuilder.Append(")");
        }
    }
}

