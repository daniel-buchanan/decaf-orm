using System;
using System.Collections.Generic;
using pdq.common;
using pdq.common.Templates;
using pdq.db.common.Builders;
using pdq.state;

namespace pdq.npgsql
{
    public class NpgsqlSqlFactory : SqlFactory
    {
        private readonly IBuilderPipeline<ISelectQueryContext> selectBuilder;
        private readonly IBuilderPipeline<IInsertQueryContext> insertBuilder;
        private readonly IBuilderPipeline<IDeleteQueryContext> deleteBuilder;

        public NpgsqlSqlFactory(
            IBuilderPipeline<ISelectQueryContext> selectBuilder,
            IBuilderPipeline<IDeleteQueryContext> deleteBuilder,
            IBuilderPipeline<IInsertQueryContext> insertBuilder)
        {
            this.selectBuilder = selectBuilder;
            this.deleteBuilder = deleteBuilder;
            this.insertBuilder = insertBuilder;
        }

        protected override IDictionary<string, object> ParseDeleteParameters(IQueryContext context)
            => this.deleteBuilder.GetParameterValues(context as IDeleteQueryContext);

        protected override SqlTemplate ParseDeleteQuery(IQueryContext context)
            => this.deleteBuilder.Execute(context as IDeleteQueryContext);

        protected override IDictionary<string, object> ParseInsertParameters(IQueryContext context)
            => this.insertBuilder.GetParameterValues(context as IInsertQueryContext);

        protected override SqlTemplate ParseInsertQuery(IQueryContext context)
            => this.insertBuilder.Execute(context as IInsertQueryContext);

        protected override IDictionary<string, object> ParseSelectParameters(IQueryContext context)
            => this.selectBuilder.GetParameterValues(context as ISelectQueryContext);

        protected override SqlTemplate ParseSelectQuery(IQueryContext context)
            => this.selectBuilder.Execute(context as ISelectQueryContext);

        protected override IDictionary<string, object> ParseUpdateParameters(IQueryContext context)
            => throw new NotImplementedException("Update is not yet implemented");

        protected override SqlTemplate ParseUpdateQuery(IQueryContext context)
            => throw new NotImplementedException("Update is not yet implemented");
    }
}

