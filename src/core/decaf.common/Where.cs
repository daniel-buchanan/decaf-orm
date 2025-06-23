using System.Collections.Generic;
using System.Linq;

namespace decaf.common;

public abstract class Where : IWhere
{
	protected List<IWhere> children;

	protected Where()
	{
		children = new List<IWhere>();
	}

	protected Where(params IWhere[] children)
	{
		this.children = children?.ToList() ?? new List<IWhere>();
	}

	public IReadOnlyCollection<IWhere> Children => children.AsReadOnly();
}