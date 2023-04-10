using System;
namespace pdq.db.common
{
	public interface IDatabaseOptions
	{
		/// <summary>
		/// The comment character for this database, e.g. --
		/// </summary>
		string CommentCharacter { get; }
	}
}

