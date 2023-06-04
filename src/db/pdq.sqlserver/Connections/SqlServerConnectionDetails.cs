using System.Text;
using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.db.common.Exceptions;

namespace pdq.sqlserver
{
    public class SqlServerConnectionDetails :
        ConnectionDetails,
        ISqlServerConnectionDetails
    {
        private bool isTrustedConnection;
        private bool isMarsEnabled;

        public SqlServerConnectionDetails() : base()
        {
            this.isTrustedConnection = false;
        }

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
                sb.Append("Trusted_Connection=Yes;");
            else
            {
                sb.AppendFormat("User ID={0};", username);
                sb.AppendFormat("Password={0};", password);
            }

            if (this.isMarsEnabled)
                sb.Append("MultipleActiveResultSets=True;");
            
            return sb.ToString();
        }
    }
}

