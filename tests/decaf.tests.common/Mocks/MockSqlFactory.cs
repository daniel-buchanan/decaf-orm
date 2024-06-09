using System.Collections.Generic;
using decaf.common.Templates;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.tests.common.Mocks
{
    public class MockSqlFactory : SqlFactory
    {
        private readonly IBuilderPipeline<IDeleteQueryContext> _delete;
        private readonly IBuilderPipeline<IInsertQueryContext> _insert;
        private readonly IBuilderPipeline<IUpdateQueryContext> _update;
        private readonly IBuilderPipeline<ISelectQueryContext> _select;
        private readonly IBuilderPipeline<ICreateTableQueryContext> _createTable;

        public MockSqlFactory(
            IBuilderPipeline<IDeleteQueryContext> delete,
            IBuilderPipeline<IInsertQueryContext> insert,
            IBuilderPipeline<IUpdateQueryContext> update,
            IBuilderPipeline<ISelectQueryContext> select,
            IBuilderPipeline<ICreateTableQueryContext> createTable = null)
        {
            _delete = delete;
            _insert = insert;
            _update = update;
            _select = select;
            _createTable = createTable;
        }

        protected override SqlTemplate ParseQuery(IDeleteQueryContext context)
            => _delete.Execute(context);

        protected override SqlTemplate ParseQuery(IInsertQueryContext context)
            => _insert.Execute(context);

        protected override SqlTemplate ParseQuery(ICreateTableQueryContext context)
            => _createTable.Execute(context);

        protected override SqlTemplate ParseQuery(ISelectQueryContext context)
            => _select.Execute(context);

        protected override SqlTemplate ParseQuery(IUpdateQueryContext context)
            => _update.Execute(context);

        protected override IDictionary<string, object> ParseParameters(ISelectQueryContext context, bool includePrefix)
            => _select.GetParameterValues(context, includePrefix);

        protected override IDictionary<string, object> ParseParameters(IDeleteQueryContext context, bool includePrefix)
            => _delete.GetParameterValues(context, includePrefix);

        protected override IDictionary<string, object> ParseParameters(IUpdateQueryContext context, bool includePrefix)
            => _update.GetParameterValues(context, includePrefix);

        protected override IDictionary<string, object> ParseParameters(IInsertQueryContext context, bool includePrefix)
            => _insert.GetParameterValues(context, includePrefix);
    }
}

