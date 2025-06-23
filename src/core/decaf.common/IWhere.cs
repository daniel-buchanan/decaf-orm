using System.Collections.Generic;

namespace decaf.common;

public interface IWhere
{
	/// <summary>
	/// Children of this clause.
	/// </summary>
	IReadOnlyCollection<IWhere> Children { get; }
}