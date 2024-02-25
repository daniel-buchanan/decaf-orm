using System;
using decaf.db.common;

namespace decaf.npgsql
{
    internal class NpgsqlImplementationFactory : DbImplementationFactory
    {
        /// <inheritdoc/>
        protected override Type ConnectionDetails()
            => typeof(NpgsqlConnectionDetails);

        /// <inheritdoc/>
        protected override Type ConnectionFactory()
            => typeof(NpgsqlConnectionFactory);

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
            => typeof(NpgsqlTransactionFactory);

        /// <inheritdoc/>
        protected override Type UpdatePipeline()
            => typeof(Builders.UpdateBuilderPipeline);

        /// <inheritdoc/>
        protected override Type ValueParser()
            => typeof(NpgsqlValueParser);

        /// <inheritdoc/>
        protected override Type WhereBuilder()
            => typeof(Builders.WhereBuilder);
    }
}

