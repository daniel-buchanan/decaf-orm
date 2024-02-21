using System;
using pdq.db.common;

namespace pdq.tests.common.Mocks
{
    public class MockDbImplementationFactory : DbImplementationFactory
    {
        protected override Type ConnectionDetails()
            => typeof(MockConnectionDetails);

        protected override Type ConnectionFactory()
            => typeof(MockConnectionFactory);

        protected override Type SqlFactory()
            => typeof(MockSqlFactory);

        protected override Type DeletePipeline()
            => typeof(MockDeletePipeline);

        protected override Type InsertPipeline()
            => typeof(MockInsertPipeline);

        protected override Type SelectPipeline()
            => typeof(MockSelectPipeline);

        protected override Type TransactionFactory()
            => typeof(MockTransactionFactory);

        protected override Type UpdatePipeline()
            => typeof(MockUpdatePipeline);

        protected override Type ValueParser()
            => typeof(MockValueParser);

        protected override Type WhereBuilder()
            => typeof(MockWhereBuilder);
    }
}