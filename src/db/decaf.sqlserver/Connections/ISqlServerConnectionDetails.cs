using decaf.common.Connections;

namespace decaf.sqlserver;

public interface ISqlServerConnectionDetails : IConnectionDetails
{
    /// <summary>
    /// Ensure the connection is a trusted connection.
    /// </summary>
    void IsTrustedConnection();

    /// <summary>
    /// Enable Multiple Active Record Sets (MARS).
    /// </summary>
    void EnableMars();
}