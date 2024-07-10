using System.Linq;
using decaf.common;
using decaf.common.Templates;
using decaf.db.common.Builders;
using decaf.state.Conditionals;
using StringContains = decaf.common.ValueFunctions.StringContains;
using StringEndsWith = decaf.common.ValueFunctions.StringEndsWith;
using StringStartsWith = decaf.common.ValueFunctions.StringStartsWith;

namespace decaf.db.common.ANSISQL
{
    public abstract class WhereClauseBuilder : IWhereClauseBuilder
    {
        private readonly IQuotedIdentifierBuilder quotedIdentifierBuilder;
        private readonly IConstants constants;

        protected WhereClauseBuilder(
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IConstants constants)
        {
            this.quotedIdentifierBuilder = quotedIdentifierBuilder;
            this.constants = constants;
        }

        public void AddWhere(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            if (clause == null) return;
            sqlBuilder.AppendLine(constants.Where);

            AddWhere(clause, sqlBuilder, parameterManager, 0);
        }

        public void AddJoin(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            if (clause == null) return;
            AddWhere(clause, sqlBuilder, parameterManager, 0);
        }

        private void AddWhere(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager, int level)
        {
            if (clause == null) return;

            if (level > 0)
                level = sqlBuilder.IncreaseIndent();

            if (clause is And ||
                clause is Or ||
                clause is Not)
                AddAndOrNot(clause, sqlBuilder, parameterManager, level);
            else AddClause(clause, sqlBuilder, parameterManager);

            if (level > 0)
                sqlBuilder.DecreaseIndent();

            sqlBuilder.AppendLine();
        }

        private void AddClause(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.PrependIndent();
            sqlBuilder.Append(constants.OpeningParen);
            
            if (clause is IColumn column) AddColumn(column, sqlBuilder, parameterManager);
            else if (clause is ColumnMatch match) AddColumnMatch(match, sqlBuilder);
            else if (clause is IBetween between) AddBetween(between, sqlBuilder, parameterManager);
            else if (clause is IInValues inValues) AddInValues(inValues, sqlBuilder, parameterManager);
            
            sqlBuilder.Append(constants.ClosingParen);
        }

        private void AddAndOrNot(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager, int level)
        {
            sqlBuilder.AppendLine(constants.OpeningParen);

            if (clause is And || clause is Or)
                AddAndOr(clause, sqlBuilder, parameterManager, level);
            else if (clause is Not not)
                AddNot(not, sqlBuilder, parameterManager, level);

            sqlBuilder.PrependIndent();
            sqlBuilder.Append(constants.ClosingParen);
        }

        private void AddAndOr(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager, int level)
        {
            string combinator = null;
            if (clause is And) combinator = constants.And;
            else if (clause is Or) combinator = constants.Or;

            var indentLevel = level + 1;
            var index = 0;
            var noChildren = clause.Children.Count - 1;
            foreach (var w in clause.Children)
            {
                AddWhere(w, sqlBuilder, parameterManager, indentLevel);

                if (index < noChildren)
                {
                    sqlBuilder.IncreaseIndent();
                    sqlBuilder.AppendLine(combinator);
                    sqlBuilder.DecreaseIndent();
                }
                index += 1;
            }
        }

        private void AddNot(Not not, ISqlBuilder sqlBuilder, IParameterManager parameterManager, int level)
        {
            if (not == null) return;
            
            sqlBuilder.IncreaseIndent();
            sqlBuilder.AppendLine(constants.Not);
            sqlBuilder.DecreaseIndent();

            AddWhere(not.Item, sqlBuilder, parameterManager, level);
        }

        private void AddColumn(IColumn column, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            if(column.ValueFunction is StringContains ||
               column.ValueFunction is StringStartsWith ||
               column.ValueFunction is StringEndsWith)
            {
                AddLike(column, sqlBuilder, parameterManager);
                return;
            }

            var parameter = parameterManager.Add(column, column.Value);
            var op = column.EqualityOperator.ToOperatorString();
            quotedIdentifierBuilder.AddColumn(column.Details, sqlBuilder);
            sqlBuilder.Append(" {0} ", op);
            sqlBuilder.Append(parameter.Name);
        }

        private void AddLike(IColumn column, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            string value, format;

            if (column.ValueFunction is StringContains contains)
            {
                format = "%{0}%";
                value = contains.Value;
            }
            else if (column.ValueFunction is StringStartsWith startsWith)
            {
                format = "{0}%";
                value = startsWith.Value;
            }
            else if (column.ValueFunction is StringEndsWith endsWith)
            {
                format = "%{0}";
                value = endsWith.Value;
            }
            else
            {
                return;
            }

            var parameter = parameterManager.Add(column, value);

            quotedIdentifierBuilder.AddColumn(column.Details, sqlBuilder);
            sqlBuilder.Append(" {0} {1}", constants.Like, constants.ValueQuote);
            sqlBuilder.Append(format, parameter.Name);
            sqlBuilder.Append(constants.ValueQuote);
        }

        private void AddColumnMatch(ColumnMatch columnMatch, ISqlBuilder sqlBuilder)
        {
            quotedIdentifierBuilder.AddColumn(columnMatch.Left, sqlBuilder);

            var op = columnMatch.EqualityOperator.ToOperatorString();
            sqlBuilder.Append(" {0} ", op);

            quotedIdentifierBuilder.AddColumn(columnMatch.Right, sqlBuilder);
        }

        private void AddBetween(IBetween between, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            var startParam = parameterManager.Add(ParameterWrapper.Create(between, "start"), between.Start);
            var endParam = parameterManager.Add(ParameterWrapper.Create(between, "end"), between.End);

            quotedIdentifierBuilder.AddColumn(between.Column, sqlBuilder);
            sqlBuilder.Append(" between {0} and {1}", startParam.Name, endParam.Name);
        }

        private void AddInValues(IInValues inValues, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            quotedIdentifierBuilder.AddColumn(inValues.Column, sqlBuilder);
            sqlBuilder.AppendLine(" in {0}", constants.OpeningParen);
            sqlBuilder.IncreaseIndent();

            var values = inValues.GetValues().ToArray();
            var lastValueIndex = values.Length;
            for (var i = 0; i < values.Length; i++)
            {
                var parameter = parameterManager.Add(ParameterWrapper.Create(inValues, i), values[i]);
                var seperator = i <= lastValueIndex ? "," : string.Empty;
                sqlBuilder.AppendLine("{0}{1}", parameter.Name, seperator);
            }

            sqlBuilder.DecreaseIndent();
            sqlBuilder.Append(constants.ClosingParen);
        }
    }
}

