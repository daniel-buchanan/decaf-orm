using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using pdq.common.Exceptions;
using pdq.common.Utilities;

namespace pdq.common.Connections
{
	public abstract class ConnectionDetails : IConnectionDetails
	{
        private string connectionString;
        private string hostname;
        private int? port;
        private string databaseName;
        private IConnectionAuthentication authentication;

        protected ConnectionDetails()
            => this.connectionString = null;

        protected ConnectionDetails(string connectionString)
            => this.connectionString = connectionString;

        protected virtual void ParseConnectionString(string connectionString, Action<string> additionalParsing = null)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(connectionString))
                    throw new ConnectionStringParsingException("No connection string has been provided");

                var validationException = ValidateConnectionString(connectionString);
                if (validationException != null) throw validationException;

                Hostname = MatchAndFetch(HostRegex, connectionString, s => s);
                Port = MatchAndFetch(PortRegex, connectionString, int.Parse);
                DatabaseName = MatchAndFetch(DatabaseRegex, connectionString, s => s);

                if (additionalParsing != null)
                    additionalParsing(connectionString);
            }
            catch (Exception e)
            {
                throw new ConnectionStringParsingException(e, "Failed to parse connection string, see Inner Exception for more information.");
            }
        }

        protected T MatchAndFetch<T>(string regex, string input, Func<string, T> parse)
        {
            var regExp = new Regex(regex);
            var match = regExp.Match(input);
            if (match.Success)
            {
                var matchedValue = match.Groups[1].Value;
                var nextSeperator = matchedValue?.IndexOf(";") ?? 0;
                if (nextSeperator <= 0) return parse(matchedValue);
                var trimmed = matchedValue?.Substring(0, nextSeperator);
                return parse(trimmed);
            }
            return default(T);
        }

        protected abstract ConnectionStringParsingException ValidateConnectionString(string connectionString);

        /// <summary>
        /// The Regex for obtaining the host of the server.
        /// </summary>
        protected abstract string HostRegex { get; }

        /// <summary>
        /// The Regex for obtaining the port of the server.
        /// </summary>
        protected abstract string PortRegex { get; }

        /// <summary>
        /// The Regex for obtaining the database name.
        /// </summary>
        protected abstract string DatabaseRegex { get; }

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
            get
            {
                if(this.port == null)
                {
                    this.port = DefaultPort;
                    return this.port.GetValueOrDefault();
                }
                return this.port.GetValueOrDefault();
            }
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
        public string GetConnectionString()
            => GetConnectionStringAsync(CancellationToken.None).WaitFor();

        /// <inheritdoc/>
        public async Task<string> GetConnectionStringAsync(CancellationToken cancellationToken = default)
        {
            if(!string.IsNullOrWhiteSpace(this.connectionString))
                return this.connectionString;

            this.connectionString = await ConstructConnectionStringAsync(cancellationToken);
            return this.connectionString;
        }

        /// <summary>
        /// Construct the connections string.
        /// </summary>
        /// <returns>The connection string.</returns>
        protected abstract Task<string> ConstructConnectionStringAsync(CancellationToken cancellationToken = default);

        /// <inheritdoc/>
        public string GetHash() => GetConnectionString().ToBase64String();
    }
}

