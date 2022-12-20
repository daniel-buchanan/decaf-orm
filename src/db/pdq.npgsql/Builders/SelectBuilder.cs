using System;
using System.Collections.Generic;
using System.Linq;
using pdq.db.common.Builders;
using pdq.state;

namespace pdq.npgsql.Builders
{
	public class SelectBuilder : db.common.Builders.SelectBuilder
	{
        private readonly QuotedIdentifierBuilder quotedIdentifierBuilder;

        protected override string CommentCharacter => "--";

        public SelectBuilder(
            QuotedIdentifierBuilder quotedIdentifierBuilder,
            db.common.Builders.IWhereBuilder whereBuilder)
            : base(new SqlBuilder(), whereBuilder)
        {
            this.quotedIdentifierBuilder = quotedIdentifierBuilder;
        }

        protected override void AddColumns(ISelectQueryContext context)
        {
            this.sqlBuilder.IncreaseIndent();
            var columns = context.Columns.ToArray();

            var lastColumnIndex = columns.Length - 1;
            for(var i = 0; i < columns.Length; i++)
            {
                this.sqlBuilder.PrependIndent();
                var delimiter = string.Empty;
                if (i < lastColumnIndex) delimiter = ",";
                this.quotedIdentifierBuilder.AddSelect(columns[i], this.sqlBuilder);
                this.sqlBuilder.Append(delimiter);
                this.sqlBuilder.AppendLine();
            }

            this.sqlBuilder.DecreaseIndent();
        }

        protected override void AddTables(ISelectQueryContext context, IList<string> parameters)
        {
            this.sqlBuilder.AppendLine("from");
            this.sqlBuilder.IncreaseIndent();

            var index = 0;
            foreach (var q in context.QueryTargets)
            {
                var delimiter = string.Empty;
                if (index < context.Columns.Count)
                    delimiter = ",";

                if(q is ITableTarget)
                {
                    var tableTarget = q as ITableTarget;
                    this.quotedIdentifierBuilder.AddFromTable(tableTarget, this.sqlBuilder);
                }
                else if(q is ISelectQueryTarget)
                {
                    
                    this.sqlBuilder.AppendLine("(");
                    var queryTarget = q as ISelectQueryTarget;
                    var parsedQuery = this.Build(queryTarget.Context, parameters);
                    this.quotedIdentifierBuilder.AddFromQuery(parsedQuery.Sql, q.Alias, this.sqlBuilder);
                }
                
                index += 1;
            }

            this.sqlBuilder.DecreaseIndent();
        }

        protected override void AddJoins(ISelectQueryContext context, IList<string> parameters)
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
                    this.sqlBuilder.AppendLine(formatStr);
                    this.sqlBuilder.IncreaseIndent();
                    Build(selectTarget.Context);
                    this.sqlBuilder.DecreaseIndent();
                    this.sqlBuilder.AppendLine(") as {0}{1}{0}", this.quote, alias);
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

                    this.sqlBuilder.AppendLine(formatStr, this.quote, schema, tableTarget.Name, alias);
                }

                this.sqlBuilder.IncreaseIndent();
                this.whereBuilder.AddWhere(j.Conditions, this.sqlBuilder);
                this.sqlBuilder.DecreaseIndent();
            }*/
        }

        protected override void AddOrderBy(ISelectQueryContext context)
        {
            var clauses = context.OrderByClauses.ToArray();
            if (clauses.Length == 0) return;

            this.sqlBuilder.AppendLine("order by");
            this.sqlBuilder.IncreaseIndent();

            var lastClauseIndex = clauses.Length - 1;
            for(var i = 0; i < clauses.Length; i++)
            {
                var delimiter = string.Empty;
                if (i < lastClauseIndex)
                    delimiter = ",";

                this.quotedIdentifierBuilder.AddOrderBy(clauses[i], this.sqlBuilder);
                this.sqlBuilder.AppendLine(delimiter);
            }

            this.sqlBuilder.DecreaseIndent();
        }

        protected override void AddGroupBy(ISelectQueryContext context)
        {
            var clauses = context.GroupByClauses.ToArray();
            if (clauses.Length == 0) return;

            this.sqlBuilder.AppendLine("group by");
            this.sqlBuilder.IncreaseIndent();

            var lastClauseIndex = clauses.Length - 1;
            for(var i = 0; i < clauses.Length; i++)
            {
                var delimiter = string.Empty;
                if (i < lastClauseIndex)
                    delimiter = ",";

                this.quotedIdentifierBuilder.AddGroupBy(clauses[i], sqlBuilder);
                this.sqlBuilder.AppendLine(delimiter);
            }

            this.sqlBuilder.DecreaseIndent();
        }
    }
}

