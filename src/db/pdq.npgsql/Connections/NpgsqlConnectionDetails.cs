using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.db.common.Exceptions;

namespace pdq.npgsql
{
    public class NpgsqlConnectionDetails :
        ConnectionDetails,
        INpgsqlConnectionDetails
    {
        private const string HostPortRegex = @"Host=(.+);.+Port=(.+);";
        private const string DatabaseRegex = @"Database=(.+);";
        private readonly List<string> schemasToSearch;

        public NpgsqlConnectionDetails() : base()
        {
            this.schemasToSearch = new List<string>();
        }

        private NpgsqlConnectionDetails(string connectionString)
            : base(connectionString)
        {
            var hostPortRegex = new Regex(HostPortRegex);
            var match = hostPortRegex.Match(connectionString);
            if (match.Success)
            {
                Hostname = match.Captures[0].Value;
                if (int.TryParse(match.Captures[1].Value, out var port))
                    Port = port;
            }

            var databaseRegex = new Regex(DatabaseRegex);
            match = databaseRegex.Match(connectionString);
            if(match.Success)
            {
                DatabaseName = match.Captures[0].Value;
            }
        }

        public static INpgsqlConnectionDetails FromConnectionString(string connectionString)
            => new NpgsqlConnectionDetails(connectionString);

        /// <inheritdoc/>
        public IReadOnlyCollection<string> SchemasToSearch
            => this.schemasToSearch.AsReadOnly();

        /// <inheritdoc/>
        protected override int DefaultPort => 5432;

        /// <inheritdoc/>
        public void AddSearchSchema(string schema)
        {
            if (string.IsNullOrWhiteSpace(schema))
                throw new ArgumentNullException(nameof(schema));

            if (schemasToSearch.Any(s => s.ToLower() == schema.ToLower()))
                return;

            this.schemasToSearch.Add(schema);
        }

        /// <inheritdoc/>
        protected override async Task<string> ConstructConnectionStringAsync()
        {
            string username, password;
            var auth = Authentication;

            // check for delayed fetch
            if(auth is DelayedFetchAuthentication delayedFetch)
                auth = await delayedFetch.FetchAsync();

            if(auth is UsernamePasswordAuthentication creds)
            {
                username = creds.Username;
                password = creds.Password;
            }
            else
            {
                throw new ConnectionStringConstructException("Provided Credentials are not username an password.");
            }

            var sb = new StringBuilder();
            sb.AppendFormat("Host={0};", this.Hostname);
            sb.AppendFormat("Port={0};", this.Port);
            sb.AppendFormat("Database={0};", this.DatabaseName);
            sb.AppendFormat("Username={0};", username);
            sb.AppendFormat("Password={0};", password);

            if (this.schemasToSearch.Any())
            {
                var schemas = string.Join(",", this.schemasToSearch);
                sb.AppendFormat("Search Path={0};", schemas);
            }
            
            return sb.ToString();
        }
    }
}

