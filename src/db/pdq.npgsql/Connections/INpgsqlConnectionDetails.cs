using System.Collections.Generic;
using pdq.common;
using pdq.common.Connections;

namespace pdq.npgsql
{
    public interface INpgsqlConnectionDetails : IConnectionDetails
    {
        /// <summary>
        /// The schemas for PostgreSQL to search when finding tables and views etc.
        /// </summary>
        IReadOnlyCollection<string> SchemasToSearch { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        void AddSearchSchema(string schema);
    }
}