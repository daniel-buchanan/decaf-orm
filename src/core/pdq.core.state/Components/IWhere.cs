using System.Collections.Generic;

namespace pdq.state
{
	public interface IWhere
	{
		IReadOnlyCollection<IWhere> Children { get; }
	}
}

