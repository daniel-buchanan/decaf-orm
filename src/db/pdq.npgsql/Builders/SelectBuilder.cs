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

        protected override string CommentCharacter => "--";

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
                if (i < lastColumnIndex) delimiter = ",";
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

            var index = 0;
            foreach (var q in context.QueryTargets)
            {
                var delimiter = string.Empty;
                if (index < context.Columns.Count)
                    delimiter = ",";

                if(q is ITableTarget)
                {
                    var tableTarget = q as ITableTarget;
                    this.quotedIdentifierBuilder.AddFromTable(tableTarget, sqlBuilder);
                }
                else if(q is ISelectQueryTarget)
                {
                    
                    sqlBuilder.AppendLine("(");
                    var queryTarget = q as ISelectQueryTarget;
                    var parsedQuery = this.Build(queryTarget.Context);
                    this.quotedIdentifierBuilder.AddFromQuery(parsedQuery.Sql, q.Alias, sqlBuilder);
                }
                
                index += 1;
            }

            sqlBuilder.DecreaseIndent();
        }

        protected override void AddJoins(ISelectQueryContext context, ISqlBuilder sqlBuilder)
        {
            
            /*foreach(var j in context.Joins)
            {
                var alias = string.Empty;
                var formatStr = "join ";
                if (!string.IsNullOrWhiteSpace(j.To.Alias))
                    alias = j.To.Alias;

                if (j.To is ISelectQueryTarget)
                {
                    var selectTarget = j.To as ISelectQueryTarget;
                    formatStr += "(";
                    sqlBuilder.AppendLine(formatStr);
                    sqlBuilder.IncreaseIndent();
                    Build(selectTarget.Context);
                    sqlBuilder.DecreaseIndent();
                    sqlBuilder.AppendLine(") as {0}{1}{0}", this.quote, alias);
                }
                else if(j.To is ITableTarget)
                {
                    var tableTarget = j.To as ITableTarget;
                    var schema = string.Empty;
                    if (!string.IsNullOrWhiteSpace(tableTarget.Schema))
                    {
                        schema = tableTarget.Schema;
                        formatStr = "{0}{1}{0}.";
                    }
                    else
                        formatStr = "{1}";

                    formatStr += "{0}{2}{0}";

                    if (!string.IsNullOrWhiteSpace(alias))
                        formatStr += " {0}{3}{0}";
                    else
                        formatStr += "{3}";

                    formatStr += " on";

                    sqlBuilder.AppendLine(formatStr, this.quote, schema, tableTarget.Name, alias);
                }

                sqlBuilder.IncreaseIndent();
                this.whereBuilder.AddWhere(j.Conditions, sqlBuilder);
                sqlBuilder.DecreaseIndent();
            }*/
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

                this.quotedIdentifierBuilder.AddOrderBy(clauses[i], sqlBuilder);
                sqlBuilder.AppendLine(delimiter);
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

