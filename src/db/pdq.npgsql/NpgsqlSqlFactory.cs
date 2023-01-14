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
        private readonly IBuilder<ISelectQueryContext> selectBuilder;
        private readonly IBuilder<IInsertQueryContext> insertBuilder;
        private readonly IBuilder<IUpdateQueryContext> updateBuilder;
        private readonly IBuilder<IDeleteQueryContext> deleteBuilder;

        public NpgsqlSqlFactory(
            IBuilder<ISelectQueryContext> selectBuilder,
            IBuilder<IDeleteQueryContext> deleteBuilder)
        {
            this.selectBuilder = selectBuilder;
            this.deleteBuilder = deleteBuilder;
        }

        protected override Dictionary<string, object> ParseDeleteParameters(IQueryContext context)
            => this.deleteBuilder.GetParameters(context as IDeleteQueryContext);

        protected override SqlTemplate ParseDeleteQuery(IQueryContext context)
            => this.deleteBuilder.Build(context as IDeleteQueryContext);

        protected override Dictionary<string, object> ParseInsertParameters(IQueryContext context)
            => this.insertBuilder.GetParameters(context as IInsertQueryContext);

        protected override SqlTemplate ParseInsertQuery(IQueryContext context)
            => this.insertBuilder.Build(context as IInsertQueryContext);

        protected override Dictionary<string, object> ParseSelectParameters(IQueryContext context)
            => this.selectBuilder.GetParameters(context as ISelectQueryContext);

        protected override SqlTemplate ParseSelectQuery(IQueryContext context)
            => this.selectBuilder.Build(context as ISelectQueryContext);

        protected override Dictionary<string, object> ParseUpdateParameters(IQueryContext context)
            => this.updateBuilder.GetParameters(context as IUpdateQueryContext);

        protected override SqlTemplate ParseUpdateQuery(IQueryContext context)
            => this.updateBuilder.Build(context as IUpdateQueryContext);
    }
}

