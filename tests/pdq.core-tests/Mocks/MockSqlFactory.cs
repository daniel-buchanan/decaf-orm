using System;
using pdq.common;

namespace pdq.core_tests.Mocks
{
    public class MockSqlFactory : SqlFactory
    {
        public MockSqlFactory()
        {
        }

        protected override SqlTemplate ParseDelete(IQueryContext context)
            => GetTemplate();

        protected override SqlTemplate ParseInsert(IQueryContext context)
            => GetTemplate();

        protected override SqlTemplate ParseSelect(IQueryContext context)
            => GetTemplate();

        protected override SqlTemplate ParseUpdate(IQueryContext context)
            => GetTemplate();

        private SqlTemplate GetTemplate() => SqlTemplate.Create(string.Empty, null);
    }
}

