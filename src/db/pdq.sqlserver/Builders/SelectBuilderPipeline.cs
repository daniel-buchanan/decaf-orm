using System.Linq;
using pdq.common.Utilities;
using pdq.db.common.Builders;
using pdq.state;

namespace pdq.sqlserver.Builders
{
    public class SelectBuilderPipeline : db.common.Builders.SelectBuilderPipeline
    {
        private readonly QuotedIdentifierBuilder quotedIdentifierBuilder;
        private readonly db.common.Builders.IWhereBuilder whereBuilder;

        public SelectBuilderPipeline(
            PdqOptions options,
            SqlServerOptions dbOptions,
            IHashProvider hashProvider,
            QuotedIdentifierBuilder quotedIdentifierBuilder,
            db.common.Builders.IWhereBuilder whereBuilder)
            : base(options, dbOptions, hashProvider, whereBuilder)
        {
            this.whereBuilder = whereBuilder;
            this.quotedIdentifierBuilder = quotedIdentifierBuilder;
        }

        protected override void AddColumns(IPipelineStageInput<ISelectQueryContext> input)
        {
            input.Builder.IncreaseIndent();
            var columns = input.Context.Columns.ToArray();

            var lastColumnIndex = columns.Length - 1;
            for(var i = 0; i < columns.Length; i++)
            {
                input.Builder.PrependIndent();
                var delimiter = string.Empty;
                if (i < lastColumnIndex) delimiter = Constants.Seperator;
                this.quotedIdentifierBuilder.AddSelect(columns[i], input.Builder);
                input.Builder.Append(delimiter);
                input.Builder.AppendLine();
            }

            input.Builder.DecreaseIndent();
        }

        protected override void AddTables(IPipelineStageInput<ISelectQueryContext> input)
        {
            input.Builder.AppendLine(Constants.From);
            input.Builder.IncreaseIndent();

            var joins = input.Context.Joins.Select(j => j.To);
            var filteredTables = input.Context.QueryTargets.Where(qt => !joins.Any(j => j.IsEquivalentTo(qt))).ToList();
            var index = 0;
            var noTables = filteredTables.Count - 1;
            foreach (var q in filteredTables)
            {
                var delimiter = string.Empty;
                if (index < noTables)
                    delimiter = Constants.Seperator;

                if (q is ITableTarget tableTarget)
                    AddFromTable(tableTarget, input);
                else if (q is ISelectQueryTarget queryTarget)
                    AddFromQuery(queryTarget, input);

                if (delimiter.Length > 0)
                    input.Builder.Append(delimiter);

                input.Builder.AppendLine();
                
                index += 1;
            }

            input.Builder.DecreaseIndent();
        }

        protected override void AddJoins(IPipelineStageInput<ISelectQueryContext> input)
        {
            foreach (var j in input.Context.Joins) AddJoin(j, input);
        }

        private void AddJoin(Join j, IPipelineStageInput<ISelectQueryContext> input)
        {
            input.Builder.Append("{0} ", Constants.Join);

            if (j.To is ISelectQueryTarget queryTarget)
                AddFromQuery(queryTarget, input);
            else if (j.To is ITableTarget tableTarget)
                AddFromTable(tableTarget, input);

            input.Builder.Append(" {0}", Constants.On);
            input.Builder.AppendLine();

            input.Builder.IncreaseIndent();
            this.whereBuilder.AddJoin(j.Conditions, input.Builder, input.Parameters);
            input.Builder.DecreaseIndent();
        }

        private void AddFromTable(ITableTarget target, IPipelineStageInput<ISelectQueryContext> input)
        {
            input.Builder.PrependIndent();
            this.quotedIdentifierBuilder.AddFromTable(target, input.Builder);
        }

        private void AddFromQuery(ISelectQueryTarget target, IPipelineStageInput<ISelectQueryContext> input)
        {
            input.Builder.AppendLine(Constants.OpeningParen);
            input.Builder.IncreaseIndent();
            Execute(target.Context, input);
            input.Builder.DecreaseIndent();
            this.quotedIdentifierBuilder.AddClosingFromQuery(target.Alias, input.Builder);
        }

        protected override void AddOrderBy(IPipelineStageInput<ISelectQueryContext> input)
        {
            var clauses = input.Context.OrderByClauses.ToArray();
            if (clauses.Length == 0) return;

            input.Builder.AppendLine(Constants.OrderBy);
            input.Builder.IncreaseIndent();

            var lastClauseIndex = clauses.Length - 1;
            for(var i = 0; i < clauses.Length; i++)
            {
                var delimiter = string.Empty;
                if (i < lastClauseIndex)
                    delimiter = ",";

                input.Builder.PrependIndent();
                this.quotedIdentifierBuilder.AddOrderBy(clauses[i], input.Builder);
                input.Builder.Append(delimiter);
                input.Builder.AppendLine();
            }

            input.Builder.DecreaseIndent();
        }

        protected override void AddGroupBy(IPipelineStageInput<ISelectQueryContext> input)
        {
            var clauses = input.Context.GroupByClauses.ToArray();
            if (clauses.Length == 0) return;

            input.Builder.AppendLine(Constants.GroupBy);
            input.Builder.IncreaseIndent();

            var lastClauseIndex = clauses.Length - 1;
            for(var i = 0; i < clauses.Length; i++)
            {
                var delimiter = string.Empty;
                if (i < lastClauseIndex)
                    delimiter = Constants.Seperator;

                this.quotedIdentifierBuilder.AddGroupBy(clauses[i], input.Builder);
                input.Builder.AppendLine(delimiter);
            }

            input.Builder.DecreaseIndent();
        }
    }
}

