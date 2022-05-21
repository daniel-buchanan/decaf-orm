using System.Collections.Generic;

namespace pdq.core.state
{
	public interface IWhere
	{
		IReadOnlyCollection<IWhere> Children { get; }
	}
}

