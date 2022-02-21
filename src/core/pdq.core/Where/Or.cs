using System;
using pdq.core.Where;

namespace pdq.core
{
	public class Or : AbstractWhereItem
	{
		internal Or() : base(WhereItemType.Or) { }

		public Or(params IWhereItem[] children) : this()
		{
			if (children == null) return;
			this.children.AddRange(children);
		}

		public static Or Where(params IWhereItem[] children) => new Or(children);
	}
}

