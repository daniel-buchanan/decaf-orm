using System.Collections.Generic;
using pdq.state;
using pdq.common.Templates;

namespace pdq.tests.common.Mocks
{
    public class MockSqlFactory : SqlFactory
    {
        public MockSqlFactory() { }

        protected override SqlTemplate ParseQuery(IDeleteQueryContext context)
            => GetTemplate();

        protected override SqlTemplate ParseQuery(IInsertQueryContext context)
            => GetTemplate();

        protected override SqlTemplate ParseQuery(ISelectQueryContext context)
            => GetTemplate();

        protected override SqlTemplate ParseQuery(IUpdateQueryContext context)
            => GetTemplate();

        private static SqlTemplate GetTemplate() => SqlTemplate.Create(string.Empty, null);

        protected override IDictionary<string, object> ParseParameters(ISelectQueryContext context)
            => new Dictionary<string, object>();

        protected override IDictionary<string, object> ParseParameters(IDeleteQueryContext context)
            => new Dictionary<string, object>();

        protected override IDictionary<string, object> ParseParameters(IUpdateQueryContext context)
            => new Dictionary<string, object>();

        protected override IDictionary<string, object> ParseParameters(IInsertQueryContext context)
            => new Dictionary<string, object>();
    }
}

