using System.Collections.Generic;
using pdq.common;

namespace pdq.npgsql
{
    public interface INpgsqlConnectionDetails : IConnectionDetails
    {
        IReadOnlyCollection<string> SchemasToSearch { get; }

        string Username { get; }

        string Password { get; }

        void AddSearchSchema(string schema);
    }
}