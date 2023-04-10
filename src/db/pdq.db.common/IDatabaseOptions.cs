using System;
using pdq.common;
using pdq.common.Connections;

namespace pdq.db.common
{
	public interface IDatabaseOptions
	{
		/// <summary>
		/// The comment character for this database, e.g. --
		/// </summary>
		string CommentCharacter { get; }

		/// <summary>
		/// The SQL connection details to use.
		/// </summary>
		IConnectionDetails ConnectionDetails { get; }
	}
}

