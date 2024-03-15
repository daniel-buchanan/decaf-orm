using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using decaf.common.Logging;
using decaf.common.Utilities;

namespace decaf.common.Connections
{
	public abstract class ConnectionFactory : IConnectionFactory
	{
        protected readonly ILoggerProxy logger;
        private IDictionary<string, IConnection> connections;

        /// <summary>
        /// Create an instance of a ConnectionFactory.
        /// </summary>
        /// <param name="logger">The logger to Use.</param>
        protected ConnectionFactory(ILoggerProxy logger)
        {
            this.connections = new Dictionary<string, IConnection>();
            this.logger = logger;
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
            this.connections = null;
        }

        /// <inheritdoc/>
        public IConnection GetConnection(IConnectionDetails connectionDetails)
            => GetConnectionAsync(connectionDetails, CancellationToken.None).WaitFor();

        /// <inheritdoc/>
        public async Task<IConnection> GetConnectionAsync(
            IConnectionDetails connectionDetails,
            CancellationToken cancellationToken = default)
        {
            ValidateConnectionParameters(connectionDetails);
            return await GetOrCreateConnectionAsync(connectionDetails, cancellationToken);
        }

        private void ValidateConnectionParameters(IConnectionDetails connectionDetails)
        {
            if (connectionDetails == null)
                throw new ArgumentNullException(nameof(connectionDetails), $"The {nameof(connectionDetails)} cannot be null, it MUST be provided when creating a connection.");

            if (this.connections == null)
                this.connections = new Dictionary<string, IConnection>();
        }

        private async Task<IConnection> GetOrCreateConnectionAsync(
            IConnectionDetails connectionDetails, 
            CancellationToken cancellationToken)
        {
            string hash;
            
            try
            {
                hash = connectionDetails.GetHash();
            }
            catch (Exception e)
            {
                this.logger.Error(e, "Error Getting connection Hash");
                throw;
            }

            if (this.connections.TryGetValue(hash, out var conn)) return conn;

            try
            {
                var connection = await ConstructConnectionAsync(connectionDetails, cancellationToken);
                this.connections.Add(hash, connection);

                return connection;
            }
            catch (Exception e)
            {
                this.logger.Error(e, $"An error occurred attempting to get the connection.");
                throw;
            }
        }

        /// <summary>
        /// Construct a connection using the provided connection details.
        /// </summary>
        /// <param name="connectionDetails">The connection details to use to create the connection.</param>
        /// <param name="cancellationToken">The cancellation token to use (optional).</param>
        /// <returns>A newly constructed connection.</returns>
        protected abstract Task<IConnection> ConstructConnectionAsync(
            IConnectionDetails connectionDetails,
            CancellationToken cancellationToken = default);
    }
}

