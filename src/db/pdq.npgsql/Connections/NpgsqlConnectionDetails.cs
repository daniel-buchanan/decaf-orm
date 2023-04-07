using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pdq.common.Connections;

namespace pdq.npgsql
{
    public class NpgsqlConnectionDetails :
        ConnectionDetails, INpgsqlConnectionDetails
    {
        private readonly List<string> schemasToSearch;
        private string username;
        private string password;

        public NpgsqlConnectionDetails() : base()
        {
            this.schemasToSearch = new List<string>();
        }

        public IReadOnlyCollection<string> SchemasToSearch
            => this.schemasToSearch.AsReadOnly();

        /// <inheritdoc/>
        public string Username
        {
            get => this.username ?? String.Empty;
            protected set
            {
                if (!string.IsNullOrWhiteSpace(this.username))
                {
                    throw new ConnectionModificationException($"{nameof(Username)} cannot be modified once ConnectionDetails instance created");
                }

                this.username = value;
            }
        }

        /// <inheritdoc/>
        public string Password
        {
            get => this.password ?? String.Empty;
            protected set
            {
                if (!string.IsNullOrWhiteSpace(this.password))
                {
                    throw new ConnectionModificationException($"{nameof(Password)} cannot be modified once ConnectionDetails instance created");
                }

                this.password = value;
            }
        }

        /// <inheritdoc/>
        public void AddSearchSchema(string schema)
        {
            if (string.IsNullOrWhiteSpace(schema))
                throw new ArgumentNullException(nameof(schema));

            if (schemasToSearch.Any(s => s.ToLower() == schema.ToLower()))
                return;

            this.schemasToSearch.Add(schema);
        }

        protected override Task<string> ConstructConnectionString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Host={0};", this.Hostname);
            sb.AppendFormat("Port={0};", this.Port);
            sb.AppendFormat("Database={0};", this.DatabaseName);
            sb.AppendFormat("Username={0};", this.Username);
            sb.AppendFormat("Password={0};", this.Password);

            if (this.schemasToSearch.Any())
            {
                var schemas = string.Join(",", this.schemasToSearch);
                sb.AppendFormat("Search Path={0};", schemas);
            }
            
            return Task.FromResult(sb.ToString());
        }
    }
}

