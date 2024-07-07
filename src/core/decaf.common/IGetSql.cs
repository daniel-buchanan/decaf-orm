using System.Collections.Generic;

namespace decaf
{
	public interface IGetSql
	{
		/// <summary>
        /// Get the SQL statement for the current query
        /// </summary>
        /// <returns>The generated SQL statement as a <see cref="string"/>.</returns>
		string GetSql();

        /// <summary>
        /// Get the parameter values for the current query.
        /// </summary>
        /// <returns>A dictionary of parameter names and values.</returns>
        Dictionary<string, object> GetSqlParameters();
	}
}

