using System;
using System.Data;
using System.Data.Common;

namespace decaf.tests.common.Mocks
{
    public class MockDbCommand : DbCommand
    {
        private readonly DbParameterCollection parameters = new MockDbParameterCollection();

        /// <inheritdoc/>
        public override string CommandText { get; set; }

        /// <inheritdoc/>
        public override int CommandTimeout { get; set; }

        /// <inheritdoc/>
        public override CommandType CommandType { get; set; }

        /// <inheritdoc/>
        public override UpdateRowSource UpdatedRowSource { get; set; }

        public override bool DesignTimeVisible { get; set; }

        protected override DbConnection DbConnection { get; set; }

        protected override DbParameterCollection DbParameterCollection => this.parameters;

        protected override DbTransaction DbTransaction { get; set; }

        /// <inheritdoc/>
        public override void Cancel() { }

        /// <inheritdoc/>
        public override int ExecuteNonQuery() => 0;

        /// <inheritdoc/>
        public override object ExecuteScalar() => null;

        /// <inheritdoc/>
        public override void Prepare() { }

        /// <inheritdoc/>
        protected override DbParameter CreateDbParameter() => new MockDbParameter();

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => new MockDbDataReader();
    }
}

