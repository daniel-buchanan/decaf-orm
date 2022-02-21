using System;
using System.Collections.Generic;

namespace pdq.core
{
	public interface IWhereItem
	{
        IEnumerable<IWhereItem> Children { get; }
	}
}

