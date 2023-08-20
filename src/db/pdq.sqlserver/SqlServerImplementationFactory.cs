using System;
using pdq.db.common;

namespace pdq.sqlserver
{
    public static partial class PdqOptionsBuilderExtensions
	{
        internal class SqlServerImplementationFactory : DbImplementationFactory
        {
            protected override Type ConnectionDetails()
                => typeof(SqlServerConnectionDetails);

            protected override Type ConnectionFactory()
                => typeof(SqlServerConnectionFactory);

            protected override Type DeletePipeline()
                => typeof(Builders.DeleteBuilderPipeline);

            protected override Type InsertPipeline()
                => typeof(Builders.InsertBuilderPipeline);

            protected override Type SelectPipeline()
                => typeof(Builders.SelectBuilderPipeline);

            protected override Type TransactionFactory()
                => typeof(SqlServerTransactionFactory);

            protected override Type UpdatePipeline()
                => typeof(Builders.UpdateBuilderPipeline);

            protected override Type ValueParser()
                => typeof(SqlServerValueParser);

            protected override Type WhereBuilder()
                => typeof(Builders.WhereBuilder);

            protected override Type Constants()
                => typeof(Builders.Constants);

            protected override Type QuotedIdentifierBuilder()
                => typeof(Builders.QuotedIdentifierBuilder);
        }
    }
}

