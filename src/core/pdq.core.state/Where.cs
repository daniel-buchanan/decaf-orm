using System.Collections.Generic;
using System.Linq;

namespace pdq.core.state
{
	public abstract class Where : IWhere
	{
		protected List<IWhere> children;

		protected Where()
        {
			this.children = new List<IWhere>();
        }

		protected Where(params IWhere[] children)
        {
			this.children = children?.ToList() ?? new List<IWhere>();
        }

		public IReadOnlyCollection<IWhere> Children => this.children.AsReadOnly();
	}
}

