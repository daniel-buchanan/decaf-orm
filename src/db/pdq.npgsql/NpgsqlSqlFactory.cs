using System;
using System.Collections.Generic;
using pdq.common;
using pdq.db.common.Builders;
using pdq.state;

namespace pdq.npgsql
{
    public class NpgsqlSqlFactory : SqlFactory
    {
        private readonly IBuilder<ISelectQueryContext> selectBuilder;
        private readonly IBuilder<IInsertQueryContext> insertBuilder;
        private readonly IBuilder<IUpdateQueryContext> updateBuilder;
        private readonly IBuilder<IDeleteQueryContext> deleteBuilder;

        public NpgsqlSqlFactory(
            IBuilder<ISelectQueryContext> selectBuilder)
        {
            this.selectBuilder = selectBuilder;
        }

        protected override Dictionary<string, object> ParseDeleteParameters(IQueryContext context, SqlTemplate template)
        {
            throw new NotImplementedException();
        }

        protected override SqlTemplate ParseDeleteQuery(IQueryContext context)
            => this.deleteBuilder.Build(context as IDeleteQueryContext);

        protected override Dictionary<string, object> ParseInsertParameters(IQueryContext context, SqlTemplate template)
        {
            throw new NotImplementedException();
        }

        protected override SqlTemplate ParseInsertQuery(IQueryContext context)
            => this.insertBuilder.Build(context as IInsertQueryContext);

        protected override Dictionary<string, object> ParseSelectParameters(IQueryContext context, SqlTemplate template)
        {
            throw new NotImplementedException();
        }

        protected override SqlTemplate ParseSelectQuery(IQueryContext context)
            => this.selectBuilder.Build(context as ISelectQueryContext);

        protected override Dictionary<string, object> ParseUpdateParameters(IQueryContext context, SqlTemplate template)
        {
            throw new NotImplementedException();
        }

        protected override SqlTemplate ParseUpdateQuery(IQueryContext context)
            => this.updateBuilder.Build(context as IUpdateQueryContext);
    }
}

