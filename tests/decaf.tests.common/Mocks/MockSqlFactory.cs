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

        public MockSqlFactory(
            IBuilderPipeline<IDeleteQueryContext> delete,
            IBuilderPipeline<IInsertQueryContext> insert,
            IBuilderPipeline<IUpdateQueryContext> update,
            IBuilderPipeline<ISelectQueryContext> select)
        {
            _delete = delete;
            _insert = insert;
            _update = update;
            _select = select;
        }

        protected override SqlTemplate ParseQuery(IDeleteQueryContext context)
            => _delete.Execute(context);

        protected override SqlTemplate ParseQuery(IInsertQueryContext context)
            => _insert.Execute(context);

        protected override SqlTemplate ParseQuery(ISelectQueryContext context)
            => _select.Execute(context);

        protected override SqlTemplate ParseQuery(IUpdateQueryContext context)
            => _update.Execute(context);

        protected override IDictionary<string, object> ParseParameters(ISelectQueryContext context)
            => _select.GetParameterValues(context);

        protected override IDictionary<string, object> ParseParameters(IDeleteQueryContext context)
            => _delete.GetParameterValues(context);

        protected override IDictionary<string, object> ParseParameters(IUpdateQueryContext context)
            => _update.GetParameterValues(context);

        protected override IDictionary<string, object> ParseParameters(IInsertQueryContext context)
            => _insert.GetParameterValues(context);
    }
}

