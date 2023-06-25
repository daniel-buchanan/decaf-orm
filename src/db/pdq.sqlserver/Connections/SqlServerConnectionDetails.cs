using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using pdq.common.Connections;

namespace pdq.sqlserver
{
    public class SqlServerConnectionDetails :
        ConnectionDetails,
        ISqlServerConnectionDetails
    {
        private const string MarsEnabled = "MultipleActiveResultSets=True";
        private const string TrustedConnection = "Trusted_Connection=Yes";

        private bool isTrustedConnection;
        private bool isMarsEnabled;

        public SqlServerConnectionDetails() : base()
            => this.isTrustedConnection = false;

        private SqlServerConnectionDetails(string connectionString)
            : base(connectionString)
        {
            if (connectionString.Contains(MarsEnabled))
                isMarsEnabled = true;

            isTrustedConnection = connectionString.Contains(TrustedConnection);

            ParseConnectionString(connectionString);
        }

        /// <inheritdoc/>
        protected override string HostPortRegex => @"Server=(.+),(\d+);";

        /// <inheritdoc/>
        protected override string DatabaseRegex => @"Database=(.+);";

        /// <summary>
        /// Create a new <see cref="ISqlServerConnectionDetails"/> from a provided connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <returns>A new <see cref="ISqlServerConnectionDetails"/> object.</returns>
        public static ISqlServerConnectionDetails FromConnectionString(string connectionString)
            => new SqlServerConnectionDetails(connectionString);

        /// <inheritdoc/>
        protected override int DefaultPort
            => 1433;

        /// <inheritdoc/>
        public void EnableMars()
            => this.isMarsEnabled = true;

        /// <inheritdoc/>
        public void IsTrustedConnection()
            => this.isTrustedConnection = true;

        /// <inheritdoc/>
        protected override async Task<string> ConstructConnectionStringAsync()
        {
            string username, password;
            username = password = null;
            var auth = Authentication;

            // check for delayed fetch
            if(auth is DelayedFetchAuthentication delayedFetch)
                auth = await delayedFetch.FetchAsync();

            if(auth is UsernamePasswordAuthentication creds)
            {
                username = creds.Username;
                password = creds.Password;
            }

            var sb = new StringBuilder();
            sb.AppendFormat("Server={0},{1};", this.Hostname, this.Port);
            sb.AppendFormat("Database={0};", this.DatabaseName);

            if (this.isTrustedConnection)
                sb.AppendFormat("{0};", TrustedConnection);
            else
            {
                sb.AppendFormat("User ID={0};", username);
                sb.AppendFormat("Password={0};", password);
            }

            if (this.isMarsEnabled)
                sb.AppendFormat("{0};", MarsEnabled);
            
            return sb.ToString();
        }
    }
}

