using System;
namespace pdq.db.common.Builders
{
	public interface ISqlBuilder
	{
		string LineEnding { get; }

		void IncreaseIndent();

		void DecreaseIndent();

		void PrependIndent();

		void Append(string value);

		void Append(string formatStr, params object[] parameters);

		void AppendLine();

		void AppendLine(string value);

		void AppendLine(string formatStr, params object[] parameters);

        /// <summary>
        /// Get the SQL statement for the current query
        /// </summary>
        /// <returns>The generated SQL statement as a <see cref="string"/>.</returns>
        string GetSql();
    }
}

