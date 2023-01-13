using System;
using System.Collections.Generic;
using pdq.common;
using pdq.common.Templates;

namespace pdq.core_tests.Mocks
{
    public class MockSqlFactory : SqlFactory
    {
        public MockSqlFactory() { }

        protected override SqlTemplate ParseDeleteQuery(IQueryContext context)
            => GetTemplate();

        protected override SqlTemplate ParseInsertQuery(IQueryContext context)
            => GetTemplate();

        protected override SqlTemplate ParseSelectQuery(IQueryContext context)
            => GetTemplate();

        protected override SqlTemplate ParseUpdateQuery(IQueryContext context)
            => GetTemplate();

        private static SqlTemplate GetTemplate() => SqlTemplate.Create(string.Empty, null);

        protected override Dictionary<string, object> ParseSelectParameters(IQueryContext context)
            => new Dictionary<string, object>();

        protected override Dictionary<string, object> ParseDeleteParameters(IQueryContext context)
            => new Dictionary<string, object>();

        protected override Dictionary<string, object> ParseUpdateParameters(IQueryContext context)
            => new Dictionary<string, object>();

        protected override Dictionary<string, object> ParseInsertParameters(IQueryContext context)
            => new Dictionary<string, object>();
    }
}

