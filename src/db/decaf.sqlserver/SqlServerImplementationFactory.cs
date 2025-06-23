using System;
using decaf.db.common;

namespace decaf.sqlserver;

public static partial class DecafOptionsBuilderExtensions
{
    internal class SqlServerImplementationFactory : DbImplementationFactory
    {
        /// <inheritdoc/>
        protected override Type ConnectionDetails()
            => typeof(SqlServerConnectionDetails);

        /// <inheritdoc/>
        protected override Type ConnectionFactory()
            => typeof(SqlServerConnectionFactory);

        /// <inheritdoc/>
        protected override Type DeletePipeline()
            => typeof(Builders.DeleteBuilderPipeline);

        /// <inheritdoc/>
        protected override Type InsertPipeline()
            => typeof(Builders.InsertBuilderPipeline);

        /// <inheritdoc/>
        protected override Type SelectPipeline()
            => typeof(Builders.SelectBuilderPipeline);

        /// <inheritdoc/>
        protected override Type TransactionFactory()
            => typeof(SqlServerTransactionFactory);

        /// <inheritdoc/>
        protected override Type UpdatePipeline()
            => typeof(Builders.UpdateBuilderPipeline);

        /// <inheritdoc/>
        protected override Type CreateTablePipeline() => null;

        /// <inheritdoc/>
        protected override Type AlterTablePipeline() => null;

        /// <inheritdoc/>
        protected override Type DropTablePipeline() => null;

        /// <inheritdoc/>
        protected override Type ValueParser()
            => typeof(SqlServerValueParser);

        /// <inheritdoc/>
        protected override Type TypeParser()
            => typeof(SqlServerTypeParser);

        /// <inheritdoc/>
        protected override Type WhereBuilder()
            => typeof(Builders.WhereClauseBuilder);

        /// <inheritdoc/>
        protected override Type Constants()
            => typeof(Builders.Constants);

        /// <inheritdoc/>
        protected override Type QuotedIdentifierBuilder()
            => typeof(Builders.QuotedIdentifierBuilder);
    }
}