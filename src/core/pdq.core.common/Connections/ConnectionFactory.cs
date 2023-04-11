﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pdq.common.Logging;
using pdq.common.Utilities;

namespace pdq.common.Connections
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
        public IConnection Get(IConnectionDetails connectionDetails)
            => GetAsync(connectionDetails).WaitFor();

        /// <inheritdoc/>
        public Task<IConnection> GetAsync(IConnectionDetails connectionDetails)
        {
            if (connectionDetails == null)
                throw new ArgumentNullException(nameof(connectionDetails), $"The {nameof(connectionDetails)} cannot be null, it MUST be provided when creating a connection.");

            return GetInternalAsync(connectionDetails);
        }

        private async Task<IConnection> GetInternalAsync(IConnectionDetails connectionDetails)
        {
            if (this.connections == null)
                this.connections = new Dictionary<string, IConnection>();
            string hash = null;

            try
            {
                hash = connectionDetails.GetHash();
            }
            catch (Exception e)
            {
                this.logger.Error(e, "Error Getting connection Hash");
                throw;
            }

            if (this.connections.ContainsKey(hash))
            {
                return this.connections[hash];
            }

            try
            {
                var connection = await ConstructConnection(connectionDetails);
                this.connections.Add(hash, connection);

                return connection;
            }
            catch (Exception e)
            {
                this.logger.Error(e, $"An error occurred attempting to get the connection.");
                throw;
            }
        }

        /// <inheritdoc/>
        protected abstract Task<IConnection> ConstructConnection(IConnectionDetails connectionDetails);
    }
}

