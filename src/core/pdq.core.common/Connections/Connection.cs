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
            this.State = ConnectionState.Unknown;
		}

        /// <inheritdoc/>
        public ConnectionState State { get; private set; }

        /// <inheritdoc/>
        public void Close()
        {
            if (this.dbConnection == null) return;
            if (this.dbConnection.State == System.Data.ConnectionState.Open ||
                this.dbConnection.State == System.Data.ConnectionState.Connecting ||
                this.dbConnection.State == System.Data.ConnectionState.Fetching ||
                this.dbConnection.State == System.Data.ConnectionState.Executing)
            {
                this.dbConnection.Close();
                this.State = ConnectionState.Closed;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            this.dbConnection.Dispose();
        }

        /// <inheritdoc/>
        public void Open()
        {
            if (this.dbConnection == null)
                this.dbConnection = GetUnderlyingConnection();

            if (this.dbConnection.State == System.Data.ConnectionState.Closed ||
                this.dbConnection.State == System.Data.ConnectionState.Broken)
            {
                this.dbConnection.Open();
                this.State = ConnectionState.Open;
            }
        }

        /// <inheritdoc/>
        public string GetHash()
            => this.connectionDetails.GetHash();

        /// <inheritdoc/>
        public abstract IDbConnection GetUnderlyingConnection();

        /// <inheritdoc/>
        public TConnection GetUnderlyingConnectionAs<TConnection>()
            where TConnection : IDbConnection
            => (TConnection)GetUnderlyingConnection();
    }
}

