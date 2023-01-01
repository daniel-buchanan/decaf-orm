using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.db.common;
using pdq.db.common.Builders;
using pdq.state;

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

        protected override void AddColumns(ISelectQueryContext context, ISqlBuilder sqlBuilder)
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

        protected override void AddTables(ISelectQueryContext context, ISqlBuilder sqlBuilder)
        {
            sqlBuilder.AppendLine("from");
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

                sqlBuilder.PrependIndent();
                if(q is ITableTarget)
                {
                    var tableTarget = q as ITableTarget;
                    this.quotedIdentifierBuilder.AddFromTable(tableTarget, sqlBuilder);
                }
                else if(q is ISelectQueryTarget)
                {
                    var queryTarget = q as ISelectQueryTarget;
                    var parsedQuery = this.Build(queryTarget.Context);
                    this.quotedIdentifierBuilder.AddFromQuery(parsedQuery.Sql, q.Alias, sqlBuilder);
                }

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

            if (j.To is ISelectQueryTarget selectTarget)
            {
                var qSqlBuilder = SqlBuilder.Create();
                Build(selectTarget.Context, qSqlBuilder, parameterManager);
                this.quotedIdentifierBuilder.AddJoinQuery(qSqlBuilder.GetSql(), selectTarget.Alias, sqlBuilder);
            }
            else if (j.To is ITableTarget tableTarget)
                this.quotedIdentifierBuilder.AddJoinTable(tableTarget, sqlBuilder);

            sqlBuilder.Append(" {0}", Constants.On);
            sqlBuilder.AppendLine();

            sqlBuilder.IncreaseIndent();
            this.whereBuilder.AddJoin(j.Conditions, sqlBuilder, parameterManager);
            sqlBuilder.DecreaseIndent();
        }

        protected override void AddOrderBy(ISelectQueryContext context, ISqlBuilder sqlBuilder)
        {
            var clauses = context.OrderByClauses.ToArray();
            if (clauses.Length == 0) return;

            sqlBuilder.AppendLine("order by");
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

        protected override void AddGroupBy(ISelectQueryContext context, ISqlBuilder sqlBuilder)
        {
            var clauses = context.GroupByClauses.ToArray();
            if (clauses.Length == 0) return;

            sqlBuilder.AppendLine("group by");
            sqlBuilder.IncreaseIndent();

            var lastClauseIndex = clauses.Length - 1;
            for(var i = 0; i < clauses.Length; i++)
            {
                var delimiter = string.Empty;
                if (i < lastClauseIndex)
                    delimiter = ",";

                this.quotedIdentifierBuilder.AddGroupBy(clauses[i], sqlBuilder);
                sqlBuilder.AppendLine(delimiter);
            }

            sqlBuilder.DecreaseIndent();
        }
    }
}

