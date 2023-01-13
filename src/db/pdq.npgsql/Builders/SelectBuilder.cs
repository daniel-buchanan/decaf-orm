using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.db.common;
using pdq.db.common.Builders;
using pdq.state;
using pdq.state.QueryTargets;

namespace pdq.npgsql.Builders
{
    public class SelectBuilder : db.common.Builders.SelectBuilder
    {
        private readonly QuotedIdentifierBuilder quotedIdentifierBuilder;

        protected override string CommentCharacter => Constants.Comment;

        public SelectBuilder(
            IHashProvider hashProvider,
            QuotedIdentifierBuilder quotedIdentifierBuilder,
            db.common.Builders.IWhereBuilder whereBuilder,
            PdqOptions pdqOptions)
            : base(whereBuilder, hashProvider, pdqOptions)
        {
            this.quotedIdentifierBuilder = quotedIdentifierBuilder;
        }

        protected override void AddColumns(ISelectQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.IncreaseIndent();
            var columns = context.Columns.ToArray();

            var lastColumnIndex = columns.Length - 1;
            for(var i = 0; i < columns.Length; i++)
            {
                sqlBuilder.PrependIndent();
                var delimiter = string.Empty;
                if (i < lastColumnIndex) delimiter = Constants.Seperator;
                this.quotedIdentifierBuilder.AddSelect(columns[i], sqlBuilder);
                sqlBuilder.Append(delimiter);
                sqlBuilder.AppendLine();
            }

            sqlBuilder.DecreaseIndent();
        }

        protected override void AddTables(ISelectQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.AppendLine(Constants.From);
            sqlBuilder.IncreaseIndent();

            var joins = context.Joins.Select(j => j.To);
            var filteredTables = context.QueryTargets.Where(qt => !joins.Any(j => j.IsEquivalentTo(qt))).ToList();
            var index = 0;
            var noTables = filteredTables.Count - 1;
            foreach (var q in filteredTables)
            {
                var delimiter = string.Empty;
                if (index < noTables)
                    delimiter = Constants.Seperator;

                if (q is ITableTarget tableTarget)
                    AddFromTable(tableTarget, sqlBuilder);
                else if (q is ISelectQueryTarget queryTarget)
                    AddFromQuery(queryTarget, sqlBuilder, parameterManager);

                if (delimiter.Length > 0)
                    sqlBuilder.Append(delimiter);

                sqlBuilder.AppendLine();
                
                index += 1;
            }

            sqlBuilder.DecreaseIndent();
        }

        protected override void AddJoins(ISelectQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            foreach (var j in context.Joins) AddJoin(j, sqlBuilder, parameterManager);
        }

        private void AddJoin(Join j, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.Append("{0} ", Constants.Join);

            if (j.To is ISelectQueryTarget queryTarget)
                AddFromQuery(queryTarget, sqlBuilder, parameterManager);
            else if (j.To is ITableTarget tableTarget)
                AddFromTable(tableTarget, sqlBuilder);

            sqlBuilder.Append(" {0}", Constants.On);
            sqlBuilder.AppendLine();

            sqlBuilder.IncreaseIndent();
            this.whereBuilder.AddJoin(j.Conditions, sqlBuilder, parameterManager);
            sqlBuilder.DecreaseIndent();
        }

        private void AddFromTable(ITableTarget target, ISqlBuilder sqlBuilder)
        {
            sqlBuilder.PrependIndent();
            this.quotedIdentifierBuilder.AddFromTable(target, sqlBuilder);
        }

        private void AddFromQuery(ISelectQueryTarget target, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.AppendLine(Constants.OpeningParen);
            sqlBuilder.IncreaseIndent();
            this.Build(target.Context, sqlBuilder, parameterManager);
            sqlBuilder.DecreaseIndent();
            this.quotedIdentifierBuilder.AddClosingFromQuery(target.Alias, sqlBuilder);
        }

        protected override void AddOrderBy(ISelectQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            var clauses = context.OrderByClauses.ToArray();
            if (clauses.Length == 0) return;

            sqlBuilder.AppendLine(Constants.OrderBy);
            sqlBuilder.IncreaseIndent();

            var lastClauseIndex = clauses.Length - 1;
            for(var i = 0; i < clauses.Length; i++)
            {
                var delimiter = string.Empty;
                if (i < lastClauseIndex)
                    delimiter = ",";

                sqlBuilder.PrependIndent();
                this.quotedIdentifierBuilder.AddOrderBy(clauses[i], sqlBuilder);
                sqlBuilder.Append(delimiter);
                sqlBuilder.AppendLine();
            }

            sqlBuilder.DecreaseIndent();
        }

        protected override void AddGroupBy(ISelectQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            var clauses = context.GroupByClauses.ToArray();
            if (clauses.Length == 0) return;

            sqlBuilder.AppendLine(Constants.GroupBy);
            sqlBuilder.IncreaseIndent();

            var lastClauseIndex = clauses.Length - 1;
            for(var i = 0; i < clauses.Length; i++)
            {
                var delimiter = string.Empty;
                if (i < lastClauseIndex)
                    delimiter = Constants.Seperator;

                this.quotedIdentifierBuilder.AddGroupBy(clauses[i], sqlBuilder);
                sqlBuilder.AppendLine(delimiter);
            }

            sqlBuilder.DecreaseIndent();
        }
    }
}

