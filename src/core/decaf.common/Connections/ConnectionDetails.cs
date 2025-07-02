using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using decaf.common.Exceptions;
using decaf.common.Utilities;

namespace decaf.common.Connections;

public abstract class ConnectionDetails : IConnectionDetails
{
    private string? connectionString;
    private string? hostname;
    private int? port;
    private string? databaseName;
    private IConnectionAuthentication? authentication;

    protected ConnectionDetails()
        => connectionString = null;

    protected ConnectionDetails(string connectionString)
    {
        this.connectionString = connectionString;
        ParseConnectionString(connectionString);
    }

    /// <summary>
    /// Parse the provided connection string into the properties of this object.
    /// </summary>
    /// <param name="input">The connection string to parse.</param>
    /// <exception cref="ConnectionStringParsingException">An error that occured during the parsing of the connection string.</exception>
    private void ParseConnectionString(string input)
    {
        try
        {
            if(string.IsNullOrWhiteSpace(input))
                throw new ConnectionStringParsingException("No connection string has been provided");

            var validationException = ValidateConnectionString(input);
            if (validationException != null) throw validationException;

            Hostname = MatchAndFetch(HostRegex, input, s => s);
            Port = MatchAndFetch(PortRegex, input, int.Parse);
            DatabaseName = MatchAndFetch(DatabaseRegex, input, s => s);

            ParseConnectionStringInternal(input);
        }
        catch (Exception e)
        {
            throw new ConnectionStringParsingException(e, "Failed to parse connection string, see Inner Exception for more information.");
        }
    }

    /// <summary>
    /// Any additional connection string processing required.
    /// </summary>
    /// <param name="input">The connection string to parse/process.</param>
    protected abstract void ParseConnectionStringInternal(string input);

    /// <summary>
    /// Perform a match against a regular expression and return the result
    /// as a value of the provided tye.
    /// </summary>
    /// <typeparam name="T">The return type to use.</typeparam>
    /// <param name="regex">The regular expression to use.</param>
    /// <param name="input">The string to match.</param>
    /// <param name="parse">A function to conver the match into the output type.</param>
    /// <returns>A match if found, otherwise the default value for the type.</returns>
    protected static T? MatchAndFetch<T>(string regex, string input, Func<string?, T> parse)
    {
        var regExp = new Regex(
            regex, 
            RegexOptions.CultureInvariant, 
            TimeSpan.FromMilliseconds(100));
        var match = regExp.Match(input);
            
        if (!match.Success) return default;
            
        var matchedValue = match.Groups[1].Value;
        if(matchedValue is null) throw new ConnectionStringParsingException("Invalid connection string");
        var nextSeparator = matchedValue.IndexOf(";", StringComparison.Ordinal);
        if (nextSeparator <= 0)
            return string.IsNullOrWhiteSpace(matchedValue) ? default : parse(matchedValue);
        
        var trimmed = matchedValue.Substring(0, nextSeparator);
        return string.IsNullOrWhiteSpace(trimmed) ? default : parse(trimmed);
    }

    /// <summary>
    /// Validate the provided connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to validate.</param>
    /// <returns>Any issues with the connection string that were found.</returns>
    protected abstract ConnectionStringParsingException? ValidateConnectionString(string connectionString);

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
    public string? Hostname
    {
        get => hostname ?? String.Empty;
        set
        {
            if(!string.IsNullOrWhiteSpace(hostname))
            {
                throw new ConnectionModificationException($"{nameof(Hostname)} cannot be modified once ConnectionDetails instance created");
            }

            hostname = value;
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
            port ??= DefaultPort;
            return port.GetValueOrDefault();
        }
        set
        {
            if (port != null && port != 0)
            {
                throw new ConnectionModificationException($"{nameof(Port)} cannot be modified once ConnectionDetails instance created");
            }

            port = value;
        }
    }

    /// <inheritdoc/>
    public string? DatabaseName
    {
        get => databaseName;
        set
        {
            if (!string.IsNullOrWhiteSpace(databaseName))
            {
                throw new ConnectionModificationException($"{nameof(DatabaseName)} cannot be modified once ConnectionDetails instance created");
            }

            databaseName = value;
        }
    }

    /// <inheritdoc/>
    public IConnectionAuthentication? Authentication
    {
        get => authentication;
        set
        {
            if (authentication != null)
                throw new ConnectionModificationException($"{nameof(Authentication)} cannot be modified once ConnectionDetails instance created");
            authentication = value;
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
        connectionString = null;
        hostname = null;
        port = null;
        databaseName = null;
    }

    /// <inheritdoc/>
    public string GetConnectionString()
        => GetConnectionStringAsync(CancellationToken.None).WaitFor();

    /// <inheritdoc/>
    public async Task<string> GetConnectionStringAsync(CancellationToken cancellationToken = default)
    {
        if(!string.IsNullOrWhiteSpace(connectionString))
            return connectionString;

        connectionString = await ConstructConnectionStringAsync(cancellationToken);
        return connectionString;
    }

    /// <summary>
    /// Construct the connections string.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to use (optional).</param>
    /// <returns>The connection string.</returns>
    protected abstract Task<string> ConstructConnectionStringAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public string GetHash() => GetConnectionString().ToBase64String();
}