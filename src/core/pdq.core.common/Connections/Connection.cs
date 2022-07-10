using System;
using System.Data;
using System.Threading.Tasks;
using pdq.common.Logging;

namespace pdq.common.Connections
{
	public abstract class Connection : IConnection
	{
        protected IDbConnection dbConnection;
        protected readonly ILoggerProxy logger;
        protected IConnectionDetails connectionDetails;

        /// <summary>
        /// Create an instance of a Connection.
        /// </summary>
        /// <param name="logger">The logger to use to log any details.</param>
        /// <param name="connectionDetails">The connection details to use.</param>
		protected Connection(
            ILoggerProxy logger,
            IConnectionDetails connectionDetails)
		{
            this.logger = logger;
            this.connectionDetails = connectionDetails;
		}

        /// <inheritdoc/>
        public void Close()
        {
            if (this.dbConnection == null) return;
            if (this.dbConnection.State == ConnectionState.Open ||
                this.dbConnection.State == ConnectionState.Connecting ||
                this.dbConnection.State == ConnectionState.Fetching ||
                this.dbConnection.State == ConnectionState.Executing)
                this.dbConnection.Close();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // nothing to do here
        }

        /// <inheritdoc/>
        public ValueTask DisposeAsync() => this.connectionDetails.DisposeAsync();

        /// <inheritdoc/>
        public void Open()
        {
            if (this.dbConnection == null)
                this.dbConnection = GetUnderlyingConnection();

            if (this.dbConnection.State == ConnectionState.Closed ||
                this.dbConnection.State == ConnectionState.Broken)
                this.dbConnection.Open();
        }

        /// <inheritdoc/>
        string IConnection.GetHash() => this.connectionDetails.GetHash();

        /// <inheritdoc/>
        public abstract IDbConnection GetUnderlyingConnection();

        /// <inheritdoc/>
        public TConnection GetUnderlyingConnectionAs<TConnection>()
            where TConnection : IDbConnection
        {
            return (TConnection)GetUnderlyingConnection();
        }
    }
}

