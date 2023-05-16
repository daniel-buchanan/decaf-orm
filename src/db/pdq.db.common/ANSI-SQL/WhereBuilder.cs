using System.Linq;
using pdq.common;
using pdq.common.Templates;
using pdq.db.common.Builders;
using pdq.state.Conditionals;
using ValueFunctions = pdq.common.ValueFunctions;

namespace pdq.db.common.ANSISQL
{
    public abstract class WhereBuilder : db.common.Builders.IWhereBuilder
    {
        protected readonly IQuotedIdentifierBuilder quotedIdentifierBuilder;
        protected readonly IConstants constants;

        protected WhereBuilder(
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
            
            if (clause is IColumn) AddColumn(clause as IColumn, sqlBuilder, parameterManager);
            else if (clause is ColumnMatch) AddColumnMatch(clause as ColumnMatch, sqlBuilder);
            else if (clause is IBetween) AddBetween(clause as IBetween, sqlBuilder, parameterManager);
            else if (clause is IInValues) AddInValues(clause as IInValues, sqlBuilder, parameterManager);
            
            sqlBuilder.Append(constants.ClosingParen);
        }

        private void AddAndOrNot(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager, int level)
        {
            sqlBuilder.AppendLine(constants.OpeningParen);

            if (clause is And ||
               clause is Or)
                AddAndOr(clause, sqlBuilder, parameterManager, level);
            else if (clause is Not)
                AddNot(clause, sqlBuilder, parameterManager, level);

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

                if (index >= 0 && index < noChildren)
                {
                    sqlBuilder.IncreaseIndent();
                    sqlBuilder.AppendLine(combinator);
                    sqlBuilder.DecreaseIndent();
                }
                index += 1;
            }
        }

        private void AddNot(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager, int level)
        {
            var not = clause as Not;
            sqlBuilder.IncreaseIndent();
            sqlBuilder.AppendLine(constants.Not);
            sqlBuilder.DecreaseIndent();

            AddWhere(not.Item, sqlBuilder, parameterManager, level);
        }

        private void AddColumn(IColumn column, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            if(column.ValueFunction is ValueFunctions.StringContains ||
               column.ValueFunction is ValueFunctions.StringStartsWith ||
               column.ValueFunction is ValueFunctions.StringEndsWith)
            {
                AddLike(column, sqlBuilder, parameterManager);
                return;
            }

            var parameter = parameterManager.Add(column, column.Value);
            var op = column.EqualityOperator.ToOperatorString();
            this.quotedIdentifierBuilder.AddColumn(column.Details, sqlBuilder);
            sqlBuilder.Append(" {0} ", op);
            sqlBuilder.Append(parameter.Name);
        }

        private void AddLike(IColumn column, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            string value, format;

            if (column.ValueFunction is ValueFunctions.StringContains contains)
            {
                format = "%{0}%";
                value = contains.Value;
            }
            else if (column.ValueFunction is ValueFunctions.StringStartsWith startsWith)
            {
                format = "{0}%";
                value = startsWith.Value;
            }
            else if (column.ValueFunction is ValueFunctions.StringEndsWith endsWith)
            {
                format = "%{0}";
                value = endsWith.Value;
            }
            else
            {
                return;
            }

            var parameter = parameterManager.Add(column, value);

            this.quotedIdentifierBuilder.AddColumn(column.Details, sqlBuilder);
            sqlBuilder.Append(" {0} {1}", constants.Like, constants.ValueQuote);
            sqlBuilder.Append(format, parameter.Name);
            sqlBuilder.Append(constants.ValueQuote);
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
            var startParam = parameterManager.Add(ParameterWrapper.Create(between, "start"), between.Start);
            var endParam = parameterManager.Add(ParameterWrapper.Create(between, "end"), between.End);

            this.quotedIdentifierBuilder.AddColumn(between.Column, sqlBuilder);
            sqlBuilder.Append(" between {0} and {1}", startParam.Name, endParam.Name);
        }

        private void AddInValues(IInValues inValues, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            this.quotedIdentifierBuilder.AddColumn(inValues.Column, sqlBuilder);
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

