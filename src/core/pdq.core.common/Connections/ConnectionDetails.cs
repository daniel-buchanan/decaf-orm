using System;
using System.Threading.Tasks;
using pdq.common.Utilities;

namespace pdq.common.Connections
{
	public abstract class ConnectionDetails : IConnectionDetailsInternal
	{
        private string connectionString;
        private string hostname;
        private int? port;
        private string databaseName;
        private IConnectionAuthentication authentication;

		protected ConnectionDetails()
		{
            this.connectionString = null;
		}

        /// <inheritdoc/>
        public string Hostname
        {
            get => this.hostname ?? String.Empty;
            set
            {
                if(!string.IsNullOrWhiteSpace(this.hostname))
                {
                    throw new ConnectionModificationException($"{nameof(Hostname)} cannot be modified once ConnectionDetails instance created");
                }

                this.hostname = value;
            }
        }

        /// <summary>
        /// The default port for this database system.
        /// </summary>
        protected abstract int DefaultPort { get; }

        /// <inheritdoc/>
        public int Port
        {
            get => this.port ?? DefaultPort;
            set
            {
                if (port != null && port != 0)
                {
                    throw new ConnectionModificationException($"{nameof(Port)} cannot be modified once ConnectionDetails instance created");
                }

                this.port = value;
            }
        }

        /// <inheritdoc/>
        public string DatabaseName
        {
            get => this.databaseName ?? String.Empty;
            set
            {
                if (!string.IsNullOrWhiteSpace(this.databaseName))
                {
                    throw new ConnectionModificationException($"{nameof(DatabaseName)} cannot be modified once ConnectionDetails instance created");
                }

                this.databaseName = value;
            }
        }

        /// <inheritdoc/>
        public IConnectionAuthentication Authentication
        {
            get => this.authentication;
            set
            {
                if (this.authentication != null)
                {
                    throw new ConnectionModificationException($"{nameof(Authentication)} cannot be modified once ConnectionDetails instance created");
                }

                this.authentication = value;
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
            this.connectionString = null;
            this.hostname = null;
            this.port = null;
            this.databaseName = null;
        }

        /// <inheritdoc/>
        public string GetConnectionString() => GetConnectionStringAsync().WaitFor();

        /// <inheritdoc/>
        public async Task<string> GetConnectionStringAsync()
        {
            if(!string.IsNullOrWhiteSpace(this.connectionString))
            {
                return this.connectionString;
            }

            this.connectionString = await ConstructConnectionStringAsync();
            return this.connectionString;
        }

        /// <summary>
        /// Construct the connections string.
        /// </summary>
        /// <returns>The connection string.</returns>
        protected abstract Task<string> ConstructConnectionStringAsync();

        /// <inheritdoc/>
        string IConnectionDetailsInternal.GetHash() => GetConnectionString().ToBase64String();
    }
}

