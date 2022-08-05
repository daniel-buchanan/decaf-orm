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

		protected ConnectionDetails()
		{
            this.connectionString = null;
		}

        /// <inheritdoc/>
        public string Hostname
        {
            get => this.hostname ?? String.Empty;
            protected set
            {
                if(!string.IsNullOrWhiteSpace(this.hostname))
                {
                    throw new ConnectionModificationException($"{nameof(Hostname)} cannot be modified once ConnectionDetails instance created");
                }

                this.hostname = value;
            }
        }

        /// <inheritdoc/>
        public int Port
        {
            get => this.port ?? 0;
            protected set
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
            protected set
            {
                if (!string.IsNullOrWhiteSpace(this.databaseName))
                {
                    throw new ConnectionModificationException($"{nameof(DatabaseName)} cannot be modified once ConnectionDetails instance created");
                }

                this.databaseName = value;
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
            if(this.connectionString != null)
            {
                return this.connectionString;
            }

            this.connectionString = await ConstructConnectionString();
            return this.connectionString;
        }

        protected abstract Task<string> ConstructConnectionString();

        /// <inheritdoc/>
        string IConnectionDetailsInternal.GetHash() => GetConnectionString().ToBase64String();
    }
}

