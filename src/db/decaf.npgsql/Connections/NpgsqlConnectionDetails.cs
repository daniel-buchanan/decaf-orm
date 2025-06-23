using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Exceptions;
using decaf.db.common.Exceptions;

namespace decaf.npgsql;

public class NpgsqlConnectionDetails :
    ConnectionDetails,
    INpgsqlConnectionDetails
{

    private const string UsernameRegex = @"Username=(.+);";
    private const string PasswordRegex = @"Password=(.+);";
    private readonly List<string> schemasToSearch;

    public NpgsqlConnectionDetails() : base()
        => schemasToSearch = new List<string>();

    private NpgsqlConnectionDetails(string connectionString) : base(connectionString) { }

    /// <inheritdoc />
    protected override void ParseConnectionStringInternal(string input)
    {
        var username = MatchAndFetch(UsernameRegex, input, s => s);
        var password = MatchAndFetch(PasswordRegex, input, s => s);
        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
            return;

        Authentication = new UsernamePasswordAuthentication(username, password);
    }
    
    /// <summary>
    /// Create an <see cref="INpgsqlConnectionDetails"/> from a provided connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to use.</param>
    /// <returns>A new <see cref="INpgsqlConnectionDetails"/> object.</returns>
    public static INpgsqlConnectionDetails FromConnectionString(string connectionString)
        => new NpgsqlConnectionDetails(connectionString);

    /// <summary>
    /// Create an <see cref="INpgsqlConnectionDetails"/> from a provided connection string, using seperately provided credentials
    /// </summary>
    /// <param name="connectionString">The connection string to use.</param>
    /// <param name="authentication">The <see cref="IConnectionAuthentication"/> to use.</param>
    /// <returns>A new <see cref="INpgsqlConnectionDetails"/> object.</returns>
    public static INpgsqlConnectionDetails FromConnectionString(
        string connectionString,
        IConnectionAuthentication authentication)
    {
        var details = new NpgsqlConnectionDetails(connectionString);
        details.Authentication = authentication;
        return details;
    }

    /// <inheritdoc/>
    protected override string HostRegex => @"Host=(.+);";

    /// <inheritdoc/>
    protected override string PortRegex => @"Port=(.+);";

    /// <inheritdoc/>
    protected override string DatabaseRegex => @"Database=(.+);";

    /// <inheritdoc/>
    public IReadOnlyCollection<string> SchemasToSearch
        => schemasToSearch.AsReadOnly();

    /// <inheritdoc/>
    protected override int DefaultPort => 5432;

    /// <inheritdoc/>
    public void AddSearchSchema(string schema)
    {
        if (string.IsNullOrWhiteSpace(schema))
            throw new ArgumentNullException(nameof(schema));

        if (schemasToSearch.Any(s => s.ToLower() == schema.ToLower()))
            return;

        schemasToSearch.Add(schema);
    }

    /// <inheritdoc/>
    protected override async Task<string> ConstructConnectionStringAsync(CancellationToken cancellationToken = default)
    {
        string username, password;
        var auth = Authentication;

        // check for delayed fetch
        if(auth is DelayedFetchAuthentication delayedFetch)
            auth = await delayedFetch.FetchAsync(cancellationToken);

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
        sb.AppendFormat("Host={0};", Hostname);
        sb.AppendFormat("Port={0};", Port);
        sb.AppendFormat("Database={0};", DatabaseName);
        sb.AppendFormat("Username={0};", username);
        sb.AppendFormat("Password={0};", password);

        if (schemasToSearch.Any())
        {
            var schemas = string.Join(",", schemasToSearch);
            sb.AppendFormat("Search Path={0};", schemas);
        }
            
        return sb.ToString();
    }

    protected override ConnectionStringParsingException ValidateConnectionString(string connectionString)
    {
        if (connectionString?.Contains("Host") == false)
            return new ConnectionStringParsingException("Connection String does not contain a \"Host\" parameter.");

        if (connectionString?.Contains("Port") == false)
            return new ConnectionStringParsingException("Connection String does not contain a \"Port\" parameter.");

        if (connectionString?.Contains("Database") == false)
            return new ConnectionStringParsingException("Connection String does not contain a \"Database\" parameter.");

        if (connectionString?.Contains("Username") == false)
            return new ConnectionStringParsingException("Connection String does not contain a \"Username\" parameter.");

        if (connectionString?.Contains("Password") == false)
            return new ConnectionStringParsingException("Connection String does not contain a \"Password\" parameter.");

        return null;
    }
}