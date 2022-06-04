using System;
using System.Data;
using System.Threading.Tasks;
using pdq.common.Logging;

namespace pdq.common.Connections
{
	public class Connection : IConnection
	{
        protected readonly ILoggerProxy logger;
        protected IConnectionDetails connectionDetails;

        /// <summary>
        /// Create an instance of a Connection.
        /// </summary>
        /// <param name="logger">The logger to use to log any details.</param>
        /// <param name="connectionDetails">The connection details to use.</param>
		public Connection(
            ILoggerProxy logger,
            IConnectionDetails connectionDetails)
		{
            this.logger = logger;
            this.connectionDetails = connectionDetails;
		}

        public void Close()
        {
            throw new NotImplementedException();
        }

        public ITransaction CreateTransaction()
        {
            throw new NotImplementedException();
        }

        public void Dispose() => this.connectionDetails.Dispose();

        public ValueTask DisposeAsync() => this.connectionDetails.DisposeAsync();

        public void Open()
        {
            throw new NotImplementedException();
        }

        string IConnection.GetHash()
        {
            return this.connectionDetails.GetHash();
        }

        IDbConnection IConnection.GetUnderlyingConnection()
        {
            throw new NotImplementedException();
        }

        TConnection IConnection.GetUnderlyingConnectionAs<TConnection>()
        {
            throw new NotImplementedException();
        }
    }
}

