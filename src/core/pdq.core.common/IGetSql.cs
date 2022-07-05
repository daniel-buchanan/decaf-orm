using System;

namespace pdq
{
	public interface IGetSql
	{
		/// <summary>
        /// Get the SQL statement for the current query
        /// </summary>
        /// <returns>The generated SQL statement as a <see cref="string"/>.</returns>
		string GetSql();
	}
}

